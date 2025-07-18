using System.Collections.Generic;
using UnityEngine;

public class LeverSwitch : Interactable
{
    [Header("Lever Switch Settings")]
    public GameObject controlledLights;
    public Transform materialController;
    public Material DarkMaterial;
    private List<Material> LightMaterials;
    public override void Awake()
    {
        base.Awake();
        InitializeMaterial();
        controlledLights.SetActive(false);
    }
    public override void InteractOperator()
    {
        IsActive = false;
        PlayerInteraction.Instance.ClearCurrentInteractObj(this);
        //TaskSystem.instance.UpdateCount();
        if (controlledLights != null)
        {
            controlledLights.SetActive(true);
            SetLightMaterial();
        }
    }
    private void InitializeMaterial()
    {
        LightMaterials = new List<Material>();
        for (int i = 0; i < materialController.childCount; i++)
        {
            var child = materialController.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                var materialChild = child.GetChild(j);
                var render = materialChild.GetComponent<MeshRenderer>();
                if (render != null)
                {
                    LightMaterials.Add(render.materials[0]);
                    render.material = DarkMaterial;
                }
            }

        }
    }
    public void SetLightMaterial()
    {
        int index = 0;
        for (int i = 0; i < materialController.childCount; i++)
        {
            var child = materialController.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                var materialChild = child.GetChild(j);
                var render = materialChild.GetComponent<MeshRenderer>();
                if (render != null)
                {
                    render.material = LightMaterials[index];
                    index++;
                }
            }

        }
    }
}