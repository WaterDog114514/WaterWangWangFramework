using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;
    [Header("Settings")]
    [SerializeField] private float checkRate = 0.2f;
    [SerializeField] public float checkRadius = 5f;
    [SerializeField] public float checkSingleObjDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;
    public Text interactionText;
    public Camera playerCamera;
    private float lastCheckTime;
    public Interactable currentInteractable;
    public List<Interactable> nearbyInteractables = new List<Interactable>();
    void Awake()
    {
        Instance = this;
        HideInteractionText();
    }
    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            CheckForInteractable();
        }

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }

    }

    public void ClearCurrentInteractObj(Interactable obj)
    {
        if(currentInteractable==obj) currentInteractable = null;
        nearbyInteractables.Remove(obj);
        HideInteractionText();
    }
    /// <summary>
    /// 检测准心对准的交互物体
    /// </summary>
    private void CheckForInteractable()
    {
        if (nearbyInteractables.Count == 0)
        {
            if (currentInteractable != null)
            {
                currentInteractable = null;
            }
            return;
        }
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, checkSingleObjDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null && nearbyInteractables.Contains(interactable))
            {
                if (interactable != currentInteractable)
                {
                    currentInteractable = interactable;
                    ShowInteractionText();
                }
            }
            else
            {
                currentInteractable = null;
                HideInteractionText();

            }
        }
        else
        {
            currentInteractable = null;
            HideInteractionText();
        }
    }

    private bool IsInRange(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) <= checkRadius;
    }

    public void ShowInteractionText()
    {
        interactionText.gameObject.SetActive(true);
        interactionText.text = currentInteractable.hintText;
    }
    public void HideInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }
}