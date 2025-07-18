using System.Threading;
using UnityEngine;

public class Camera3DController : MonoBehaviour
{
     public Transform CameraController;
    public CameraShaker cameraShaker;
    private float mouseY; 
    public float angleY;               // ��ֱ�ӽǽǶ�
    public float rotationSpeed = 3f;    // �ӽ���ת������
    public float maxViewAngle = 80f;    // ���������
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
