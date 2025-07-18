//using UnityEngine;

//public class IdleState : EnemyBaseState
//{
//    private float _idleTimer;
//    private float _idleDuration;

//    public override void EnterState(EnemyAI enemy)
//    {
//        // 设置随机空闲时间(2-5秒)
//        _idleDuration = Random.Range(2f, 5f);
//        _idleTimer = 0f;

//        enemy.agent.ResetPath();
//        enemy.agent.isStopped = true;

//        enemy.animator.SetBool("IsWalking", false);
//        enemy.animator.SetBool("IsRunning", false);
//        enemy.animator.SetBool("IsIdle", true);
//    }

//    public override void UpdateState(EnemyAI enemy)
//    {
//        _idleTimer += Time.deltaTime;

//        // 空闲时间结束转回闲逛
//        if (_idleTimer >= _idleDuration)
//        {
//            enemy.TransitionToState(enemy.WanderState);
//            return;
//        }

//        // 持续检查玩家是否可见
//        CheckForPlayer(enemy);
//    }

//    public override void ExitState(EnemyAI enemy)
//    {
//        enemy.agent.isStopped = false;
//        enemy.animator.SetBool("IsIdle", false);
//    }
//}