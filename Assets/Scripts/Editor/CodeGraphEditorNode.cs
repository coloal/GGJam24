using CodeGraph;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CodeGraphEditorNode : Node
{
    private CodeGraphNode graphNode;
    private Port outputPort;
    private List<Port> ports;
    public CodeGraphNode GraphNode => graphNode;
    public List<Port> Ports => ports;
    public CodeGraphEditorNode(CodeGraphNode node)
    {
        this.AddToClassList("code-graph-node");
        graphNode = node;
        Type typeInfo = node.GetType();
        NodeInfoAttribute info = typeInfo.GetCustomAttribute<NodeInfoAttribute>();

        title = info.Title;

        ports = new List<Port>();

        string[] depths = info.MenuItem.Split('/');
        foreach (string depth in depths)
        {
            AddToClassList(depth.ToLower().Replace(' ', '-'));
        }

        this.name = typeInfo.Name;
        if(info.HasFlowInput)
        {
            CreateFlowInputPort();
        }
        if(info.HasFlowOutput)
        {
            CreateFlowOutputPort();
        }
    }

    private void CreateFlowOutputPort()
    {
        outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(PortTypes.FlowPort));
        outputPort.portName = "Out";
        outputPort.tooltip = "The flow output";
        ports.Add(outputPort);
        outputContainer.Add(outputPort);
    }

    private void CreateFlowInputPort()
    {
        Port inputPort;
        inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(PortTypes.FlowPort));
        inputPort.portName = "In";
        inputPort.tooltip = "The flow input";
        ports.Add(inputPort);
        inputContainer.Add(inputPort);
    }

    public void SavePosition()
    {
        graphNode.SetPosition(GetPosition());   
    }
}
