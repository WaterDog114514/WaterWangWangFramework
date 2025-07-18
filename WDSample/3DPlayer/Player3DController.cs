using UnityEngine;

public class Player3DController : MonoBehaviour
{
    [Header("依赖设置")]
    public GameObject PlayerObj;
    public Transform FootPosition;


    public bool IsRunning => isRunning;
    public bool IsGrounded => isGrounded;
    public Vector3 MoveInput => new Vector3(horizontal, 0, vertical);
    [Header("移动设置")]
    public float walkSpeed = 2.5f;      // 正常行走速度 (m/s)
    public float runSpeed = 5.5f;       // 跑步速度 (m/s)
    public float acceleration = 25f;    // 加速到最大速度时间 (ms)
    public float deceleration = 40f;    // 减速到停止时间 (ms)
    public float rotationSpeed = 3f;    // 视角旋转灵敏度
    public float airControl = 0.5f;     // 空中控制系数
    [Header("跳跃设置")]
    public float jumpHeight = 1.2f;     // 跳跃高度
    public float gravity = -15f;        // 重力值
    public float groundCheckDistance = 0.2f; // 地面检测距离
    public float groundCheckRadius = 0.25f; // 地面检测半径
    public LayerMask groundLayer;       // 地面层
    [Header("视角限制")]
    private CharacterController controller;
    private Vector3 currentVelocity;    // 当前水平速度
    private Vector3 verticalVelocity;   // 垂直速度(用于跳跃和重力)
    private bool isGrounded;            // 是否在地面
    public float angleX;               // 水平视角角度
    private bool isRunning;             // 是否在跑步
    //是否可以操作
    public bool IsCanOperator;

    // 在PlayerController类中添加这些变量
    [Header("相机晃动")]
    public float walkShakeAmount = 0.05f;
    public float runShakeAmount = 0.1f;
    public float shakeFrequency = 3f;
    private float horizontal;
    private float vertical;
    private float mouseX;

    #region 获取输入相关
    public void GetMoveInputVertical(float input)
    {
        vertical = input;
    }

    public void GetMoveInputHorizontal(float input)
    {
        horizontal = input;
    }
    public void GetMouseInput(Vector2 delta)
    {
        mouseX = delta.x;
    }
    private void GetJumpInput()
    {
        // 计算跳跃初速度 (v = sqrt(2gh))
        verticalVelocity.y = Mathf.Sqrt(2f * -gravity * jumpHeight);
    }
    #endregion

    #region 应用输入相关
    private void ApplyMovement()
    {
        Vector3 moveInput = new Vector3(horizontal, 0, vertical); // 记录输入值
        // 检测跑步输入
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        //if (moveInput != Vector3.zero)
        //{
        //    PlayerFootStepController.instance.SetMovementState(true);
        //}
        //else
        //{
        //    PlayerFootStepController.instance.SetMovementState(false);
        //}
        //PlayerFootStepController.instance.SetMovenentPitch(isRunning);
        // 计算目标速度
        float targetSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 targetDirection = (PlayerObj.transform.forward * vertical + PlayerObj.transform.right * horizontal).normalized;
        Vector3 targetVelocity = targetDirection * targetSpeed;

        // 计算加速/减速系数
        float smoothTime = (targetDirection.magnitude > 0.1f) ?
            (1f / acceleration) : (1f / deceleration);

        // 空中控制减弱
        float controlFactor = isGrounded ? 1f : airControl;

        // 平滑过渡速度
        currentVelocity = Vector3.Lerp(
            currentVelocity,
            targetVelocity,
            controlFactor * Time.deltaTime / smoothTime);
        // 组合水平和垂直速度
        Vector3 moveVector = currentVelocity + verticalVelocity;
        // 应用移动
        controller.Move(moveVector * Time.deltaTime);
    }

    private void ApplyRotation()
    {
        //float mouseY = -MouseInputVector.y;
        if (IsCanRotateX)
        {
            angleX += mouseX * rotationSpeed;
            PlayerObj.transform.rotation = Quaternion.Euler(0, angleX, 0);
        }

     
    }

    private void ApplyGravity()
    {

        // 改进的地面检测(球形检测)
        isGrounded = Physics.CheckSphere(
            FootPosition.position,
            groundCheckRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore);

        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f; // 轻微下压力确保贴地
        }
        else
        {
            // 应用重力
            verticalVelocity.y += gravity * Time.deltaTime;
        }
    }

    #endregion

    public bool IsCanRotateX;
    public bool IsCanRotateY;
    public Vector3 LastMousePos;
    [Header("Camera Collision Settings")]
    public float cameraRadius = 0.2f; // 摄像机碰撞半径
    public float cameraOffset = 0.1f; // 摄像机与碰撞点的偏移距离
    public LayerMask cameraCollisionMask; // 摄像机碰撞层

    private Vector3 targetCameraLocalPos; // 摄像机目标本地位置
    private Vector3 smoothedCameraPos; // 平滑后的摄像机位置
    private float initialCameraDistance; // 初始摄像机距离
    // 添加获取移动状态的公共方法
    public void Start()
    {
        IsCanOperator = true;
        // 记录摄像机初始本地位置
        //  targetCameraLocalPos = cameraController.localPosition;
        initialCameraDistance = targetCameraLocalPos.magnitude;
        smoothedCameraPos = targetCameraLocalPos;
        controller = PlayerObj.GetComponent<CharacterController>();
        IsCanRotateX = true;
        IsCanRotateY = true;
        //angleY = cameraController.localEulerAngles.x;
        angleX = PlayerObj.transform.eulerAngles.y;
        //注册应用
        PlayerCharacterInputProcessor.Instance.InitializedPlayerController(this);
    }
    public void Update()
    {
        if (!IsCanOperator) return;
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
    }
}