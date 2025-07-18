using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraCollision : MonoBehaviour
{
    [Header("Trigger Settings")]
    private Rigidbody rid;
    public float returnSpeed = 5f; // 回归原点的速度
    public float angleY;               // 垂直视角角度
    private bool isColliding = false;
    private void Awake()
    {
        rid = GetComponent<Rigidbody>();
    }
    private void Update()
    {
       if(isColliding) return;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            Vector3.zero,
            Time.deltaTime * returnSpeed);

    }
    void OnCollisionStay(Collision collision)
    {
       isColliding = true;
    }
    void OnCollisionExit(Collision collision)
    {
        isColliding = false;
        // 停止所有物理运动
        rid.velocity = Vector3.zero;
        rid.angularVelocity = Vector3.zero;
    }
}