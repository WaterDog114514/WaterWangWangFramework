//using UnityEngine;

//public class MonologueTrigger : MonoBehaviour
//{
//    [System.Serializable]
//    public class MonologueData
//    {
//        [TextArea(3, 5)]
//        public string text;
//        public AudioClip voiceClip;
//        public float displayTime = 3f;
//        public bool useCustomDisplayTime = false;
//    }

//    [Header("Trigger Settings")]
//    [SerializeField] private LayerMask playerLayer;
//    [SerializeField] private float triggerRadius = 3f;
//    [SerializeField] private bool triggerOnce = true;

//    [Header("Monologue Content")]
//    [SerializeField] private MonologueData monologue;

//    private bool hasTriggered;
//    private MonologueController monologueController=> MonologueController.Instance;

   
//    private void OnTriggerEnter(Collider other)
//    {
//        if(!other.CompareTag("Player")||hasTriggered)return;
//        if (monologueController != null)
//        {
//            float displayTime = monologue.useCustomDisplayTime ? monologue.displayTime : -1f;
//            monologueController.ShowMonologue(monologue.text, monologue.voiceClip, displayTime);
//            hasTriggered = true;

//            if (triggerOnce)
//            {
//                enabled = false;
//            }
//        }
//    }
//    public void InteractTrigger()
//    {

//        if (monologueController != null)
//        {
//            float displayTime = monologue.useCustomDisplayTime ? monologue.displayTime : -1f;
//            monologueController.ShowMonologue(monologue.text, monologue.voiceClip, displayTime);
//            hasTriggered = true;

//            if (triggerOnce)
//            {
//                enabled = false;
//            }
//        }
//    }

//}