using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineController : MonoBehaviour
{
    [Header("Outline Settings")]
    [SerializeField] private Color outlineColor = Color.green;
    [SerializeField] private float outlineWidth = 0.02f;

    private Renderer objectRenderer;
    private Material outlineMaterial;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();

        // 创建高光材质实例
        outlineMaterial = new Material(Shader.Find("Custom/Outline"));
        outlineMaterial.SetColor("_OutlineColor", outlineColor);
        outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
    }

    public void EnableOutline(bool enable)
    {
        if (objectRenderer == null) return;

        List<Material> materials = new List<Material>();
        materials.AddRange(objectRenderer.materials);

        if (enable)
        {
            if (!materials.Contains(outlineMaterial))
            {
                materials.Add(outlineMaterial);
            }
        }
        else
        {
            materials.Remove(outlineMaterial);
        }

        objectRenderer.materials = materials.ToArray();
    }
}