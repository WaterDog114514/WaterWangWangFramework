using System;
using UnityEngine;


namespace WDEditor
{
    [Serializable]
    public class winData_JsonInitializedHelper : BaseWindowData
    {
        public string ClassName;
        public string LastSaveDirectionPath;
        public JsonType jsonType;
        public override string Title => "JsonÅäÖÃÄ£°åÉú³É";
        public override void IntiFirstCreate()
        {
        }


    }
}