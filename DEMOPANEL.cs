using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMOPANEL : UIBasePanel
{

    protected override void Awake()
    {
        base.Awake();
        AddListenerButtonClickEvent("EnterBtn", () => {
            Debug.Log("入了！");
        });
        AddListenerChangeSliderEvent("SaoSlider", (value) =>
        {
            Debug.Log("改了"+value);
        });        
        AddListenerChangeToggelEvent("LaoNai", (value) =>
        {
            Debug.Log("开关老奶"+value);
        });
    }
}
