using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMOINPUT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        InputMgr.Instance.StartOrCloseInputMgr(true);
        // InputData data = JsonManager.Instance.LoadDataFromPath<InputData>(Application.dataPath + "/jiji.json",  JsonType.JsonNet);

        InputMgr.Instance.RegisterButtonKeyInputInfo(E_InputEvent.Fire, new InputKey( KeyCode.J), E_KeyInputType.Down);
        EventCenterManager.Instance.AddInputEventListener(E_InputEvent.Fire, () =>
        {
            Debug.Log("开火啦");
        });

        //InputMgr.Instance.RegisterButtonKeyInputInfo(E_InputEvent.Skill, KeyCode.T, E_KeyInputType.Always);
        //EventCenterManager.Instance.AddInputEventListener(E_InputEvent.Skill, () => { print("放大招"); });

        InputMgr.Instance.RegisterDirectionKeyInfo(E_InputEvent.Move,new InputKey(KeyCode.A), new InputKey(KeyCode.D));
        EventCenterManager.Instance.AddInputEventListener(E_InputEvent.Move, () =>
        {
           Debug.LogWarning( "负向键:"+ (InputMgr.Instance.dic_Input[E_InputEvent.Move] as DirectionKeyInputInfo).negativeKey.isDown);
        });
        EventCenterManager.Instance.AddInputEventListener(E_InputEvent.Move, () =>
        {
            Debug.LogWarning("正向键:" + (InputMgr.Instance.dic_Input[E_InputEvent.Move] as DirectionKeyInputInfo).positiveKey.isDown);
        });


        InputMgr.Instance.RegisterMouseKeyInputInfo(E_InputEvent.MouseMove, E_MouseInputType.MouseMove);
        EventCenterManager.Instance.AddInputEventListener(E_InputEvent.MouseMove, () =>
        {
          //  print((InputMgr.Instance.dic_Input[E_InputEvent.MouseMove] as MouseMoveOrScrollInfo).mouseDelta);
        });


         InputMgr.Instance.RegisterButtonKeyInputInfo(E_InputEvent.Skill, new InputKey(2), E_KeyInputType.Always);
        EventCenterManager.Instance.AddInputEventListener(E_InputEvent.Skill, () =>
        {
           print("按了哦");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) {

            InputMgr.Instance.ChangeButtonInfoFromInput(E_InputEvent.Skill);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            InputMgr.Instance.ChangeDirctionInfoFromInput(E_InputEvent.Move,true);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            InputMgr.Instance.ChangeDirctionInfoFromInput(E_InputEvent.Move, false);
        }

        //print("移动:" + InputMgr.Instance.dic_Input[E_InputEvent.Move].Value);
        //    print((InputMgr.Instance.dic_Input[E_InputEvent.MouseMove] as MouseMoveOrScrollInfo).mouseDelta);
    }
}
