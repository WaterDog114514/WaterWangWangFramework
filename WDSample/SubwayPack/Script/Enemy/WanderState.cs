//using UnityEngine.AI;
//using UnityEngine;

//public class WanderState : EnemyBaseState
//{
//    private float _wanderTimer;
//    private Vector3 _wanderTarget;
//    private float _wanderDuration;

//    public override void EnterState(EnemyAI enemy)
//    {
//        // 设置随机巡逻时间(3-8秒)
//        _wanderDuration = Random.Range(3f, 8f);
//        _wanderTimer = 0f;

//        enemy.agent.speed = 2f;
//        SetNewWanderTarget(enemy);

//        enemy.animator.SetBool("IsWalking", true);
//        enemy.animator.SetBool("IsRunning", false);
//        enemy.animator.SetBool("IsIdle", false);
//    }

//    public override void UpdateState(EnemyAI enemy)
//    {
//        _wanderTimer += Time.deltaTime;

//        // 检查是否到达目的地或巡逻时间结束
//        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance ||
//            _wanderTimer >= _wanderDuration)
//        {
//            enemy.TransitionToState(enemy.IdleState);
//            return;
//        }

//        // 持续检查玩家是否可见
//        CheckForPlayer(enemy);
//    }

//    private void SetNewWanderTarget(EnemyAI enemy)
//    {
//        Vector3 randomDirection = Random.insideUnitSphere * 10f;
//        randomDirection += enemy.transform.position;

//        NavMeshHit hit;
//        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
//        {
//            _wanderTarget = hit.position;
//            enemy.agent.SetDestination(_wanderTarget);
//        }
//    }

//    public override void ExitState(EnemyAI enemy)
//    {
//        enemy.animator.SetBool("IsWalking", false);
//    }
//}