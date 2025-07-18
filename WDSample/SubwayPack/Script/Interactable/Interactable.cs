using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    public Vector3 HintOffset;
    [Header("Interaction Settings")]
    [SerializeField] public string hintText = "按E交互";
    [SerializeField] private AnimationClip InteractAnimation;
    [SerializeField] private AudioClip InteractSound;
    public UnityAction interactCallback;

    protected AudioSource audioSource;
    protected Animation animationPlayer;
    public string HintText => hintText;
    public bool IsActive { get; protected set; } = true;

    public virtual void Awake()
    {
        animationPlayer = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }
    public virtual void Interact()
    {
        animationPlayer?.Play();
        audioSource?.PlayOneShot(InteractSound);
        InteractOperator();
        interactCallback?.Invoke();

    }
    public abstract void InteractOperator();
    public void SetActive(bool active)
    {
        IsActive = active;
    }
    private List<Interactable> nearbyInteractables => PlayerInteraction.Instance.nearbyInteractables;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // 找出新出现的交互物体

        if (this.IsActive == false) return;

        if (!nearbyInteractables.Contains(this))
        {
            nearbyInteractables.Add(this);
        }
    }
    void OnTriggerExit(Collider other)
    {
        nearbyInteractables.Remove(this);
    }
}