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
        private bool hasFlowOutput;
        public string Title => nodeTitle;
        public string MenuItem => menuItem;
        public bool HasFlowInput => hasFlowInput;
        public bool HasFlowOutput => hasFlowOutput;


        public NodeInfoAttribute(string nodeTitle, string menuItem = "", bool hasFlowInput = true, bool hasFlowOutput = true)
        {
            this.nodeTitle = nodeTitle;
            this.menuItem = menuItem;
            this.hasFlowInput = hasFlowInput;
            this.hasFlowOutput = hasFlowOutput;
        }
    }
}
