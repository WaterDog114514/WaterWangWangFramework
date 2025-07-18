using System.Threading;
using UnityEngine;

public class Camera3DController : MonoBehaviour
{
     public Transform CameraController;
    public CameraShaker cameraShaker;
    private float mouseY; 
    public float angleY;               // 垂直视角角度
    public float rotationSpeed = 3f;    // 视角旋转灵敏度
    public float maxViewAngle = 80f;    // 最大仰俯角
    public void Start()
    {
        PlayerCharacterInputProcessor.Instance.InitializedPlayerCamera(this);
    }
    public void GetMouseInput(Vector2 delta)
    {
        mouseY =- delta.y;
    }
    private void ApplyCameraRotation()
    {
        angleY += mouseY * rotationSpeed;
        angleY = Mathf.Clamp(angleY, -maxViewAngle, maxViewAngle);
        CameraController.localRotation = Quaternion.Euler(angleY, 0, 0);
    }
    private void LateUpdate()
    {
        ApplyCameraRotation();
        cameraShaker.ApplyHeadBobbing();
    }
}
