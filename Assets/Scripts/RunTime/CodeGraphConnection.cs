using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [Serializable]
    public struct CodeGraphConnection
    {
        public CodeGraphConnectionPort outputPort;
        public CodeGraphConnectionPort inputPort;

        public CodeGraphConnection(CodeGraphConnectionPort outputPort, CodeGraphConnectionPort inputPort)
        {
            this.outputPort = outputPort;
            this.inputPort = inputPort;
        }

        public CodeGraphConnection(string inputPortId, int inputIndex, string outputPortId, int outputPortIndex)
        {
            inputPort = new CodeGraphConnectionPort(inputPortId, inputIndex);
            outputPort = new CodeGraphConnectionPort(outputPortId, outputPortIndex);
        }
    }
    [Serializable]
    public struct CodeGraphConnectionPort
    {
        public string nodeId;
        public int portIndex;

        public CodeGraphConnectionPort(string id, int index)
        {
            nodeId = id;
            portIndex = index;
        }
    }
}

