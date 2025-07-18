using UnityEngine;
[System.Serializable]
public class CameraShaker 
{
  
    public Player3DController playerController;
    public Transform ShakerTransform;
    [Header("晃动设置")]
    public float walkAmplitude = 0.05f;    // 走路晃动幅度
    public float runAmplitude = 0.1f;      // 跑步晃动幅度
    public float frequency = 3f;           // 晃动频率
    public float rotationAmount = 0.5f;    // 旋转晃动幅度(度)
    public float smoothSpeed = 5f;         // 晃动平滑速度

    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;
    private float shakeTimer;
    private Vector3 smoothVelocity;
    public void ApplyHeadBobbing()
    {
        if (playerController == null) return;
        // 只有在地面移动时才有晃动
        if (playerController.IsGrounded && playerController.MoveInput.magnitude > 0.1f)
        {
            float amplitude = playerController.IsRunning ? runAmplitude : walkAmplitude;

            // 计算晃动位置
            shakeTimer += Time.deltaTime * frequency;
            float xOffset = Mathf.Sin(shakeTimer * 2f) * amplitude;
            float yOffset = (Mathf.Sin(shakeTimer * 4f) * amplitude * 0.5f) - (amplitude * 0.5f);

            // 计算晃动旋转
            float zRot = Mathf.Sin(shakeTimer * 1.5f) * rotationAmount;

            // 应用晃动
            Vector3 targetPos = originalLocalPos + new Vector3(xOffset, yOffset, 0);
            Quaternion targetRot = originalLocalRot * Quaternion.Euler(0, 0, zRot);

            ShakerTransform.localPosition = Vector3.SmoothDamp(
                ShakerTransform.localPosition,
                targetPos,
                ref smoothVelocity,
                1f / frequency,
                Mathf.Infinity,
                Time.deltaTime * smoothSpeed);

            ShakerTransform.localRotation = Quaternion.Slerp(
                ShakerTransform.localRotation,
                targetRot,
                Time.deltaTime * smoothSpeed);
        }
        else
        {
            // 平滑回归原位
            ShakerTransform.localPosition = Vector3.SmoothDamp(
                ShakerTransform.localPosition,
                originalLocalPos,
                ref smoothVelocity,
                0.1f);

            ShakerTransform.localRotation = Quaternion.Slerp(
                ShakerTransform.localRotation,
                originalLocalRot,
                Time.deltaTime * smoothSpeed);

            // 重置计时器以获得连贯的循环
            if (playerController.MoveInput.magnitude < 0.1f)
            {
                shakeTimer = 0;
            }
        }
    }
}