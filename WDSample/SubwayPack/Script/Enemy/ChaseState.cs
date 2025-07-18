//using UnityEngine;

//public class ChaseState : EnemyBaseState
//{
//    private float _chaseCheckTimer;
//    private float _attackCheckTimer;
//    private float _attackRange = 2f;
//    public override void EnterState(EnemyAI enemy)
//    {
//        enemy.agent.speed = 4f;
//        enemy.animator.SetBool("IsRunning", true);
//    }
//    public override void UpdateState(EnemyAI enemy)
//    {
//        enemy.agent.SetDestination(enemy.player.position);

//        _chaseCheckTimer += Time.deltaTime;
//        _attackCheckTimer += Time.deltaTime;
//        if (_chaseCheckTimer >= 5f) // 每秒检查一次
//        {
//            _chaseCheckTimer = 0f;

//            if (!enemy.PlayerInSight)
//            {
//                enemy.TransitionToState(enemy.IdleState);
//                return;
//            }


//        }
//        if (_attackCheckTimer >= 0.1F) // 每0.2秒检查一次攻击
//        {
//            _attackCheckTimer = 0f;
//            // 检查是否在攻击范围内
//            if (enemy.DistanceToPlayer <= _attackRange)
//            {
//                //CD就警觉
//                if (enemy.AttackState.IsAttackCD)
//                {
//                    enemy.TransitionToState(enemy.AlertState);
//                }
//                else
//                    enemy.TransitionToState(enemy.AttackState);
//            }
//        }


//    }

//    public override void ExitState(EnemyAI enemy)
//    {
//        // 清理代码
//        enemy.animator.SetBool("IsRunning", false);
//    }
//}