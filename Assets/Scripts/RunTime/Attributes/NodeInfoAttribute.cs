using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    public class NodeInfoAttribute : Attribute
    {
        private string nodeTitle;
        private string menuItem;
        private bool hasFlowInput;
        private string color; 
     
        public string Title => nodeTitle;
        public string MenuItem => menuItem;
        public bool HasFlowInput => hasFlowInput;
        public string Color => color;
      


        public NodeInfoAttribute(string nodeTitle, string menuItem = "", bool hasFlowInput = true, string color = "#888888")
        {
            this.nodeTitle = nodeTitle;
            this.menuItem = menuItem;
            this.hasFlowInput = hasFlowInput;
            this.color = color;
        }
    }
}
