using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    public class CodeGraphObject : MonoBehaviour
    {

        [HideInInspector]
        public CodeGraphAsset graphInstance;
        private CodeGraphAsset graphAsset;


        private CodeGraphNode currentNode;
     
        private void OnEnable()
        {         
            //ExecuteAsset();
        }

        public void ExecuteAsset(CodeGraphAsset graphAsset)
        {
            this.graphAsset = graphAsset; 
            graphInstance = Instantiate(graphAsset);
            graphInstance.Init();
            currentNode = graphInstance.GetStartNode();
            if(currentNode == null)
            {
                Debug.LogError("There is no Start Node");
                Destroy(gameObject);
            }
        }

        public CardTemplate GetNextCard(bool bSwipedLeft = true)
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

        public void RestartGraph()
        {
            currentNode = graphInstance.GetStartNode();
            if (currentNode == null)
            {
                Debug.LogError("There is no Start Node");
                Destroy(gameObject);
            }
        }

        public CodeGraphAsset GetGraphAsset()
        {
            return graphAsset;
        }
    }
}
