using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
public class InverseMask : Image
{
    public override Material materialForRendering {
        get
        {
            Material copy = new Material(base.materialForRendering);
            copy.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return copy;
        }
    }
}
