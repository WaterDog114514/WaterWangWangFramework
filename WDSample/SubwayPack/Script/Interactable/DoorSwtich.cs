//using DG.Tweening;
//using System.Collections;
//using UnityEngine;

//public class DoorSwtich : Interactable
//{
//    public AnimationClip CloseAnimation;
//    public AnimationClip OpenAnimation;
//    public bool IsLook;
//    private bool isOpen;
//    public int OpenCD;
//    Vector3 originalPos;
//    WaitForSeconds wait;
//    public override void Awake()
//    {
//        isOpen = false;
//        hintText = "开门(E)";
//        wait = new WaitForSeconds(OpenCD);
//        base.Awake();
//        // 记录初始位置
//        originalPos = transform.position;
//    }
//    public MonologueTrigger DoorTrigger;
//    public override void Interact()
//    {
//        if (!IsActive)
//        {
//            return;
//        }
//        if (IsLook)
//        {


//            DoorTrigger?.InteractTrigger();
//            LockDoorAnimation();
//            return;
//        }
//        if (isOpen)
//        {
//            animationPlayer.clip = CloseAnimation;
//            animationPlayer.Play();
//            hintText = "开门(E)";
//            GetComponent<Collider>().enabled = true;
//        }
//        else
//        {
//            GetComponent<Collider>().enabled = false;
//            animationPlayer.clip = OpenAnimation;
//            animationPlayer.Play();
//            hintText = "关门(E)";
//        }
//        audioSource?.Play();
//        isOpen = !isOpen;
//        StartCoroutine(waitOpenCD());

//    }
//    public override void InteractOperator()
//    {

//    }
//    private IEnumerator waitOpenCD()
//    {
//        IsActive = false;
//        PlayerInteraction.Instance.currentInteractable = null;
//        yield return wait;
//        IsActive = true;
//    }
//    private bool isAnimating = false;  // 新增：动画状态标志

//    /// <summary>
//    /// 锁门动画 - 原地晃动效果（带平滑返回和播放限制）
//    /// </summary>
//    public void LockDoorAnimation()
//    {
//        // 如果正在播放动画，则直接返回
//        if (isAnimating) return;

//        // 设置动画状态
//        isAnimating = true;

//        // 记录初始位置和旋转
//        Vector3 originalPos = transform.position;
//        Quaternion originalRot = transform.rotation;

//        // 创建晃动序列
//        Sequence lockSequence = DOTween.Sequence();

//        // 1. 添加位置晃动效果（只在X和Z轴）
//        lockSequence.Join(transform.DOShakePosition(
//            0.5f,                      // 持续时间
//            new Vector3(0.08f, 0, 0.08f), // 晃动幅度
//            15,                        // 震动频率
//            50,                        // 随机性
//            false,                     // 不淡出
//            true)                      // 世界坐标系
//            .SetEase(Ease.OutSine));

//        // 2. 添加旋转晃动效果（只在Y轴）
//        lockSequence.Join(transform.DOShakeRotation(
//            0.5f,                      // 持续时间
//            new Vector3(0, 4f, 0),     // 旋转幅度
//            15,                        // 震动频率
//            50,                        // 随机性
//            true)                      // 淡出效果
//            .SetEase(Ease.OutSine));

//        // 3. 平滑返回原位（0.2秒）
//        lockSequence.Append(transform.DOMove(originalPos, 0.2f).SetEase(Ease.InOutQuad));
//        lockSequence.Join(transform.DORotateQuaternion(originalRot, 0.2f).SetEase(Ease.InOutQuad));

//        // 4. 添加音效
//        if (audioSource != null)
//        {
//            lockSequence.InsertCallback(0, () => audioSource.Play());
//        }

//        // 5. 动画完成回调
//        lockSequence.OnComplete(() =>
//        {
//            // 确保完全复位
//            transform.position = originalPos;
//            transform.rotation = originalRot;

//            // 重置动画状态
//            isAnimating = false;
//        });

//        // 播放序列
//        lockSequence.Play();
//    }

//    // ... 其他现有方法 ...
//}
