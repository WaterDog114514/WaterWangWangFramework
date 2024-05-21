using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiMerDemo : MonoBehaviour
{
    TimerObj timerObj;
    // Start is called before the first frame update
    void Start()
    {

        timerObj = TimerMgr.Instance.StartNewTimer(TimerObj.TimerType.ScaleTime, 53, () =>
             {
             }).SetIntervalCallback(0.35F, () =>
             {
                 print("±ùËªÄ§·¨£º" + timerObj.GetSurplusTime);
             });


    }

    private void OnGUI()
    {
        Time.timeScale = GUILayout.HorizontalSlider(Time.timeScale, 0, 5);
        if (GUILayout.Button("¿ªµ¶"))
        {
            TimerMgr.Instance.StopTimer(timerObj.ID);
        }
        if (GUILayout.Button("DADAAµ¶"))
        {
            TimerMgr.Instance.StartTimer(timerObj.ID);
        }

        if (GUILayout.Button("Test"))
        {
            ObjectManager.Instance.poolManager.DEMOTEST();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
class suten
{
    string name;
    public suten(string aa) { name = aa; }
    public IEnumerator wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            Debug.Log(name);
        }
    }
}