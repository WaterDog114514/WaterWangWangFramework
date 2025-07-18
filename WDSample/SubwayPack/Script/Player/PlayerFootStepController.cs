using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerFootStepController : MonoBehaviour
{
    public static PlayerFootStepController instance;
    [Header("Footstep Settings")]
    [SerializeField] private AudioClip footstepClips; // 脚步声剪辑数组
    [SerializeField] private float walkPitch = 1f; // 走路音调
    [SerializeField] private float runPitch = 1.2f; // 跑步音调

    private AudioSource audioSource;
    private bool isRunning;

    private void Awake()
    {
        instance = this;    
        audioSource = GetComponent<AudioSource>();
    }
    private bool CurrentMoveState;
    public void SetMovementState(bool moving)
    {
        if (CurrentMoveState==moving)return;
        CurrentMoveState = moving;

        if ((moving))
        {

            // 根据移动状态更新步频和音调
            audioSource.loop = true;
            audioSource.clip = footstepClips;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
    public void SetMovenentPitch(bool isRunning)
    {
        audioSource.pitch = isRunning ? runPitch : walkPitch;
    }
}