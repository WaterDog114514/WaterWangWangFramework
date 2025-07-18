//using UnityEngine;
//using UnityEngine.AI;

//public class EnemyAI : MonoBehaviour
//{
//    [Header("References")]
//    public NavMeshAgent agent;
//    public Animator animator;
//    public Transform player;
//    public LayerMask detectionLayer;
//    [Header("Settings")]
//    public float detectionRadius = 6F;
//    public float fieldOfView = 90f;
//    public LayerMask obstructionLayers;
//    public float minDetectionRadiu;
//    //启用检测距离
//    public float CheckDistance;
//    // 状态变量
//    private EnemyBaseState _currentState;
//    public WanderState WanderState;
//    public AlertState AlertState;
//    public ChaseState ChaseState;
//    public AttackState AttackState;
//    public IdleState IdleState;
//    public AudioSource audioSource;
//    public AudioClip KnockedAudio;
//    // 公共属性
//    public bool PlayerInSight { get; private set; }
//    public float DistanceToPlayer => Vector3.Distance(transform.position, player.position);

//    void Start()
//    {
//        //初始化状态
//        IdleState = new IdleState();
//        WanderState = new WanderState();
//        AlertState = new AlertState(GetComponent<AudioSource>());
//        ChaseState = new ChaseState();
//        AttackState = new AttackState(this.transform, player,audioSource,KnockedAudio);
//        agent = GetComponent<NavMeshAgent>();
//        animator = GetComponent<Animator>();

//        // 初始状态
//        TransitionToState(WanderState);
//    }
//    void Update()
//    {

//        _currentState.UpdateState(this);
//        CheckPlayerVisibility();
//    }
//    public void TransitionToState(EnemyBaseState newState)
//    {
//        _currentState?.ExitState(this);
//        _currentState = newState;
//        _currentState.EnterState(this);
//    }
//    private void CheckPlayerVisibility()
//    {
//        PlayerInSight = IsPlayerInSight();
//    }
//    private bool IsPlayerInSight()
//    {
//        if (player == null)
//        {
//            Debug.LogError("Player reference is missing!");
//            return false;
//        }
//        // 1. 计算基础参数
//        Vector3 enemyBasePos = transform.position;
//        Vector3 playerBasePos = player.position;
//        // 2. 基础距离检查
//        float distanceToPlayer = Vector3.Distance(enemyBasePos, playerBasePos);
//        //最基础检测，检测通过再进行复杂检测
//        if (distanceToPlayer < CheckDistance)
//        {
//            return false;
//        }
//        if (distanceToPlayer > detectionRadius)
//        {
//            return false;
//        }
//        if (distanceToPlayer < minDetectionRadiu)
//        {
//            return true;
//        }

//        // 敌人身体四个检测点 (头顶、胸部、胯部、脚部)
//        Vector3[] enemyCheckPoints = new Vector3[]
//        {
//        enemyBasePos + Vector3.up * 1.8f,  // 头顶 (约1.8米高)
//        enemyBasePos + Vector3.up * 1.2f,  // 胸部 (约1.2米高)
//        enemyBasePos + Vector3.up * 0.7f,  // 胯部 (约0.7米高)
//        enemyBasePos + Vector3.up * 0.1f   // 脚部 (约0.1米高)
//        };

//        // 玩家身体四个对应检测点
//        Vector3[] playerCheckPoints = new Vector3[]
//        {
//        playerBasePos + Vector3.up * 1.8f,
//        playerBasePos + Vector3.up * 1.2f,
//        playerBasePos + Vector3.up * 0.7f,
//        playerBasePos + Vector3.up * 0.1f
//        };

       
      

//        // 3. 高度差检查
//        float heightDifference = Mathf.Abs(playerBasePos.y - enemyBasePos.y);
//        if (heightDifference > 2f)
//        {
//            return false;
//        }

//        // 4. 动态视野角度计算
//        float dynamicFOV = Mathf.Lerp(fieldOfView, fieldOfView * 0.5f,
//                                   distanceToPlayer / detectionRadius);

//        // 5. 角度检查
//        Vector3 directionToPlayer = (playerBasePos - enemyBasePos).normalized;
//        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
//        if (angleToPlayer > dynamicFOV / 2f)
//        {
//            return false;
//        }
//        // 6. 多射线检测
//        for (int i = 0; i < enemyCheckPoints.Length; i++)
//        {
//            Vector3 rayOrigin = enemyCheckPoints[i];
//            Vector3 rayDirection = (playerCheckPoints[i] - rayOrigin).normalized;
//            float rayDistance = Vector3.Distance(rayOrigin, playerCheckPoints[i]);
//            // 绘制调试射线
//            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.cyan, 0.1f);
//            RaycastHit hit;
//            if (!Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance, obstructionLayers))
//            {
//                // 有一条射线未被阻挡，可以看到玩家
//                return true;
//            }
//            else if (hit.transform.CompareTag("Player"))
//            {
//                // 直接命中玩家
//                Debug.Log($"射线{i}直接命中玩家");
//                return true;
//            }
//            else
//            {
//                // 被障碍物阻挡
//                Debug.DrawLine(rayOrigin, hit.point, Color.yellow, 0.1f);
//                Debug.Log($"射线{i}被 {hit.transform.name} 阻挡");
//            }
//        }

//        // 所有射线都被阻挡
//        Debug.Log("所有射线都被阻挡，看不到玩家");
//        return false;
//    }

//}