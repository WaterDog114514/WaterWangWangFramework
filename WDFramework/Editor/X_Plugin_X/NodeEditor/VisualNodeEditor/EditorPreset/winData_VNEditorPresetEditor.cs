using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WDEditor
{
    [Serializable]
    public class winData_VNEditorPresetEditor : BaseWindowData
    {

      
        //第一次找不到就搜索，之后就不搜索了
        [NonSerialized]
        public VNEditorPreset _preset;
        public string GUID;

        public override string Title =>"节点编辑器预设编辑";
    }
}