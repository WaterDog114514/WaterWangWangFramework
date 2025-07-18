using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class InteractionHintController : MonoBehaviour
{
    public static InteractionHintController Instance;
    [Header("References")]
    public RectTransform hintContainer;
    public GameObject hintPrefab;
    public Camera PlayerCamera;
    [Header("Settings")]
    [SerializeField] private float minSize = 3f;
    [SerializeField] private float maxSize = 5f;
    [SerializeField] private float minAlpha = 0.5f;
    [SerializeField] private float maxAlpha = 0.95f;
    private List<Interactable> nearInterables => PlayerInteraction.Instance.nearbyInteractables;
    private List<GameObject> hintObjs;
    private int LastNearItemCount;
    private void Start()
    {
        Instance = this;
        hintObjs = new List<GameObject>();
    }
    void Update()
    {
        CheckRoundUpdate();
        UpdateHintPos();
    }
    private void CheckRoundUpdate()
    {
        if (nearInterables.Count == LastNearItemCount) return;
        //得到差值
        int difference = Mathf.Abs(nearInterables.Count - LastNearItemCount);
        if (nearInterables.Count > LastNearItemCount)
        {
            //相差则创建
            for (int i = 0; i < difference; i++)
            {

                var obj = Instantiate(hintPrefab);
                obj.transform.SetParent(hintContainer);
                hintObjs.Add(obj);
            }
        }
        else
        {
            //少则删
            for (int i = 0; i < difference; i++)
            {
                var Obj = hintObjs[hintObjs.Count - 1 - i];
                hintObjs.Remove(Obj);
                Destroy(Obj);
            }
        }
        LastNearItemCount = nearInterables.Count;
    }
    private void UpdateHintPos()
    {
        if (hintObjs.Count == 0) return;
        for (int i = 0; i < hintObjs.Count; i++)
        {
            var HintObj = hintObjs[i];
            var InteractionObj = nearInterables[i];

            float distance = Vector3.Distance(transform.position, InteractionObj.transform.position);
            //计算距离百分比
            float DistanceRatio = (1 - distance / PlayerInteraction.Instance.checkRadius);
            float Size = Mathf.Clamp(maxSize * DistanceRatio, minSize, maxSize);
            HintObj.GetComponent<CanvasGroup>().alpha= Mathf.Clamp(maxAlpha * DistanceRatio,minAlpha, maxAlpha);
            //设置位置
            HintObj.transform.position = PlayerCamera.WorldToScreenPoint(InteractionObj.transform.position + InteractionObj.HintOffset);
            //设置大小
            HintObj.transform.localScale = Vector3.one * Size;
        }
    }
}