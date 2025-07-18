using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Helper
{

    [MenuItem("helper/脱离父级")]
    public static void SetParent()
    {
        var obj = Selection.activeGameObject;
        if (obj != null)
        {
            obj.transform.SetParent(null);
        }
    }
}
