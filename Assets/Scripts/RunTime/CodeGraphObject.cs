using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    public class CodeGraphObject : MonoBehaviour
    {
        [SerializeField]
        private CodeGraphAsset graphAsset;

        private CodeGraphAsset graphInstance;

        private void OnEnable()
        {
            graphInstance = Instantiate(graphAsset);
            ExecuteAsset();
        }

        private void ExecuteAsset()
        {
            graphInstance.Init();
            CodeGraphNode startNode = graphInstance.GetStartNode();
            ProcessAndMoveToNextNode(startNode);
        }

        private void ProcessAndMoveToNextNode(CodeGraphNode currentNode)
        {
            string nextNode = currentNode.OnProcess(graphInstance);
            if(!string.IsNullOrEmpty(nextNode))
            {
                CodeGraphNode node = graphInstance.GetNode(nextNode);
                ProcessAndMoveToNextNode(node);
            }

        }
    }
}
