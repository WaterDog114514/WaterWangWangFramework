using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/// <summary>
/// 场景切换管理器
/// </summary>
public class GameSceneManager : Singleton_UnMono<GameSceneManager>
{

    //同步切换场景的方法
    public void LoadScene(E_Scene sceneName, UnityAction callBack = null)
    {
        //切换场景
        SceneManager.LoadScene(sceneName.ToString());
        //调用回调
        callBack?.Invoke();
        callBack = null;
    }

    //异步切换场景的方法
    public void LoadSceneAsyn(E_Scene sceneName, UnityAction callBack = null)
    {
        MonoManager.Instance.StartCoroutine(ReallyLoadSceneAsyn(sceneName.ToString(), callBack));
        EventCenterManager.Instance.TriggerGameEvent<E_Scene>(E_GameEvent.LoadScene, sceneName);
    }

    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction callBack)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //不停的在协同程序中每帧检测是否加载结束 如果加载结束就不会进这个循环每帧执行了
        //加载进度的处理，等后面弄
        while (!ao.isDone)
        {
            //可以在这里利用事件中心 每一帧将进度发送给想要得到的地方
            yield return 0;
        }
        //避免最后一帧直接结束了 没有同步1出去

        callBack?.Invoke();
    }
}
