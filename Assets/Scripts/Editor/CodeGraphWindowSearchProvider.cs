using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace CodeGraph.Editor
{

    public struct SearchContentElement
    {
        public object target { get; private set; }
        public string title { get; private set; }
        public SearchContentElement(object target, string title)
        {
            this.target = target;
            this.title = title;
        }

    }
    public class CodeGraphWindowSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        public CodeGraphView graph;
        public VisualElement target;
        public static List<SearchContentElement> elements;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Nodes"), 0));
            elements = new List<SearchContentElement>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                foreach(Type type in assembly.GetTypes())
                {
                    if(type.CustomAttributes.ToList() != null)
                    {
                        Attribute attribute = type.GetCustomAttribute(typeof(NodeInfoAttribute));
                        if(attribute != null)
                        {
                            NodeInfoAttribute att = (NodeInfoAttribute)attribute;
                            var node = Activator.CreateInstance(type);
                            if(string.IsNullOrEmpty(att.MenuItem))
                            {
                                continue;
                            }
                            elements.Add(new SearchContentElement(node, att.MenuItem));
                        }
                    }
                }
            }

            elements.Sort((entry1, entry2) =>
            {
                string[] splits1 = entry1.title.Split("/");
                string[] splits2 = entry2.title.Split("/");
                for (int i = 0; i < splits1.Length; i++)
                {
                    if( i >= splits2.Length)
                    {
                        return 1;
                    }
                    int value = splits1[i].CompareTo(splits2[i]);
                    if( value != 0)
                    {
                        if(splits1.Length != splits2.Length && (i == splits1.Length - 1 || i == splits2.Length - 1))
                        {
                            return splits1.Length < splits2.Length ? 1 : -1;
                        }
                        return value;
                    }
                }
                return 0;
            });

            List<string> groups = new List<string>();
            foreach (SearchContentElement element in elements)
            {
                string[] entryTitle = element.title.Split("/");
                string groupName = "";
                for (int i = 0;i < entryTitle.Length - 1;i++)
                {
                    groupName = entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()));
                entry.level = entryTitle.Length;
                entry.userData = new SearchContentElement(element.target, element.title);
                tree.Add(entry);
            }
            return tree;

        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {

            var worldMousePosition = graph.Window.rootVisualElement.ChangeCoordinatesTo(graph.Window.rootVisualElement.parent, context.screenMousePosition - graph.Window.position.position);
            var localMousePosition = graph.contentViewContainer.WorldToLocal(worldMousePosition);

            SearchContentElement element = (SearchContentElement)SearchTreeEntry.userData;
            
            CodeGraphNode node = (CodeGraphNode)element.target;
            if(node is StartNode && graph.CodeGraph.GetStartNode() != null)
            {
                Debug.LogWarning("Ya existe un nodo Start!!!");
                node = null;
                return false;
            }
            node.SetPosition(new Rect(localMousePosition, new Vector2()));
            graph.Add(node);
            return true;
        }
    }
}

