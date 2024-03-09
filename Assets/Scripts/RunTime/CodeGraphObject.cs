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

        private CodeGraphNode currentNode;

        private void OnEnable()
        {
            graphInstance = Instantiate(graphAsset);
            ExecuteAsset();
        }

        private void ExecuteAsset()
        {
            graphInstance.Init();
            currentNode = graphInstance.GetStartNode();
        }

        public CardTemplate GetNextCard(bool bSwipedLeft)
        {
            string nextNode = currentNode.OnNextNode(graphInstance, bSwipedLeft);
            if (!string.IsNullOrEmpty(nextNode))
            {
                currentNode = graphInstance.GetNode(nextNode);
                CardTemplate card;
                if (currentNode.GetNodeCard(out card))
                {
                    return card;
                }
                else
                {
                    return GetNextCard(bSwipedLeft);
                }   
            }
            Debug.Log("Se me han acabado los nodos Señor.");
            return null;
        }

    }
}
