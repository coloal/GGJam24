using CodeGraph;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

public class CodeGraphEditorNode : Node
{
    private CodeGraphNode graphNode;
    private Port outputPort;
    private List<Port> ports;
    private List<Port> inputPorts;
    private List<Port> outputPorts;
    private SerializedProperty serializedProperty;
    private SerializedObject serializedObject;

    public CodeGraphNode GraphNode => graphNode;
    public List<Port> Ports => ports;
    public List<Port> InputPorts => inputPorts;
    public List<Port> OutputPorts => outputPorts;
    public CodeGraphEditorNode(CodeGraphNode node, SerializedObject codeGraphObject)
    {
        this.AddToClassList("code-graph-node");
        graphNode = node;
        Type typeInfo = node.GetType();
        NodeInfoAttribute info = typeInfo.GetCustomAttribute<NodeInfoAttribute>();
        this.serializedObject = codeGraphObject;
        title = info.Title;

        ports = new List<Port>();
        inputPorts = new List<Port>();
        outputPorts = new List<Port>();

        string[] depths = info.MenuItem.Split('/');
        foreach (string depth in depths)
        {
            AddToClassList(depth.ToLower().Replace(' ', '-'));
        }

        this.name = typeInfo.Name;

        if(info.HasFlowOutput)
        {
            CreateFlowOutputPort();
        }
        if(info.HasFlowInput)
        {
            CreateFlowInputPort();
        }
        
        foreach(FieldInfo property in typeInfo.GetFields())
        {
            if(property.GetCustomAttribute<ExposedPropertyAttribute>() is ExposedPropertyAttribute exposedProperty)
            {
                PropertyField field = DrawProperty(property.Name);
                //field.RegisterValueChangeCallback(OnFieldChangeCallback);
            }
        }
        RefreshExpandedState();
    }

    private void OnFieldChangeCallback(SerializedPropertyChangeEvent evt)
    {
        throw new NotImplementedException();
    }

    private PropertyField DrawProperty(string propertyName)
    {
        if(serializedProperty == null)
        {
            FetchSerializedProperty();
        }
        SerializedProperty prop = serializedProperty.FindPropertyRelative(propertyName);
        PropertyField field = new PropertyField(prop);
        field.bindingPath = prop.propertyPath;
        extensionContainer.Add(field);
        return field;
    }

    private void FetchSerializedProperty()
    {
        SerializedProperty nodes = serializedObject.FindProperty("nodes");
        if (nodes.isArray)
        {
            int size = nodes.arraySize;
            for(int i = 0; i < size; i++)
            {
                var element = nodes.GetArrayElementAtIndex(i);
                var elementId = element.FindPropertyRelative("guid");
                if(elementId.stringValue == graphNode.id)
                {
                    serializedProperty = element;
                }
            }
        }
    }

    private void CreateFlowOutputPort()
    {
        outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(PortTypes.FlowPort));
        outputPort.portName = "Out";
        outputPort.tooltip = "The flow output";
        ports.Add(outputPort);
        outputPorts.Add(outputPort);
        outputContainer.Add(outputPort);
    }

    private void CreateFlowInputPort()
    {
        Port inputPort;
        inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(PortTypes.FlowPort));
        inputPort.portName = "In";
        inputPort.tooltip = "The flow input";
        inputPorts.Add(inputPort);
        ports.Add(inputPort);
        inputContainer.Add(inputPort);
    }

    public void SavePosition()
    {
        graphNode.SetPosition(GetPosition());   
    }
}
