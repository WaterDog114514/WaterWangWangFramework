//using System.Collections.Generic;
//using UnityEngine;

//public class TheEndSwitch : Interactable
//{
//    public bool IsCanLaunched;
//    public static TheEndSwitch instance;
//    public MonologueTrigger NoCanTrigger;
//    public MonologueTrigger CanTrigger;
//    public override void Awake()
//    {
//        instance = this;
//        base.Awake();
//    }
//    public override void InteractOperator()
//    {
//        if (!IsCanLaunched)
//        {
//            NoCanTrigger.InteractTrigger();
//            IsCanLaunched = false;
//            return;
//        }

//        IsActive = false;
//        TheEnd.Instance.EndTrigger();
//        PlayerInteraction.Instance.ClearCurrentInteractObj(this);
//        CanTrigger.InteractTrigger();

//    }
//}