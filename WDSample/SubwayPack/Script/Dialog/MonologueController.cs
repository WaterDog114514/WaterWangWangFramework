//using UnityEngine;
//using UnityEngine.UI;
//using DG.Tweening;

//public class MonologueController : MonoBehaviour
//{
//    public static MonologueController Instance { get; private set; }
//    [Header("UI References")]
//    [SerializeField] private CanvasGroup monologuePanel;
//    [SerializeField] private Text monologueText;

//    [Header("Animation Settings")]
//    [SerializeField] private float fadeInDuration = 0.5f;
//    [SerializeField] private float fadeOutDuration = 0.5f;
//    [SerializeField] private float defaultDisplayTime = 3f;

//    private AudioSource audioSource;
//    private Sequence currentSequence;

//    private void Awake()
//    {
//        Instance = this;
//        audioSource = GetComponent<AudioSource>();
//        monologuePanel.alpha = 0f;
//        monologueText.text = "";
//    }

//    public void ShowMonologue(string text, AudioClip voiceClip, float displayTime = -1f)
//    {
//        // 停止当前动画
//        currentSequence?.Kill();
//        // 带发光效果的绿色
//        monologueText.text = "<color=#00FF00><b>我：" + text + "</b></color>";
//        // 播放语音
//        if (voiceClip != null && audioSource != null)
//        {
//            audioSource.clip = voiceClip;
//            audioSource.Play();
//        }
//        // 计算实际显示时间
//        float actualDisplayTime = displayTime > 0 ? displayTime : defaultDisplayTime;
//        // 创建动画序列
//        currentSequence = DOTween.Sequence()
//            .Append(monologuePanel.DOFade(1f, fadeInDuration))
//            .AppendInterval(actualDisplayTime)
//            .Append(monologuePanel.DOFade(0f, fadeOutDuration))
//            .OnComplete(() => monologueText.text = "");
//    }
//}