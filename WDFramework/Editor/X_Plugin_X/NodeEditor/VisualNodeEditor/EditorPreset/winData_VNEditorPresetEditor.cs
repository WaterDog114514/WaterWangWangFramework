using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WDEditor
{
    [Serializable]
    public class winData_VNEditorPresetEditor : BaseWindowData
    {

      
        //��һ���Ҳ�����������֮��Ͳ�������
        [NonSerialized]
        public VNEditorPreset _preset;
        public string GUID;

        public override string Title =>"�ڵ�༭��Ԥ��༭";
    }
}