using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiMerDemo : MonoBehaviour
{
    TimerObj timerObj;
    TimerObj timerObj2;
    // Start is called before the first frame update
    void Start()
    {
        timerObj = TimerMgr.Instance.StartNewTimer(TimerObj.TimerType.ScaleTime, 6, () =>
        {
            print("完成啦");
        }).SetIntervalCallback(0.15F, () =>
        {
            print("剩余时间：" + timerObj.GetSurplusTime);
        });
        //int i = 0;
        //timerObj2 = TimerMgr.Instance.StartNewTimer(TimerObj.TimerType.ScaleTime, 3, () => {
        //    print("完成啦");
        //}).SetIntervalCallback(0.5F, () => {
        //    print("烈焰风暴：" + i++);
        //});
    }

    private void OnGUI()
    {
        if (GUILayout.Button("开刀"))
        {
            TimerMgr.Instance.StopTimer(timerObj.ID);
        }   if (GUILayout.Button("DADAA刀"))
        {
            TimerMgr.Instance.StartTimer(timerObj.ID);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
