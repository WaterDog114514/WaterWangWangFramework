//using UnityEngine;

//public class AlertState : EnemyBaseState
//{
//    private float _alertTimer;
//    AudioSource source;
//    public AlertState(AudioSource source)
//    {
//         this.source = source;
//    }
//    public override void EnterState(EnemyAI enemy)
//    {
//        _alertTimer = 0f;
//        enemy.agent.isStopped = true;
//        enemy.agent.ResetPath();
//        enemy.animator.SetTrigger("Alert"); // 播放尖叫动画
//        source.Play();
//    }

//    public override void UpdateState(EnemyAI enemy)
//    {
//        _alertTimer += Time.deltaTime;

//        if (enemy.PlayerInSight)
//        {
//            if (_alertTimer >= 2f) // 2秒警觉后进入追逐
//            {
//                enemy.TransitionToState(enemy.ChaseState);
//            }
//        }
//        else
//        {
//            enemy.TransitionToState(enemy.WanderState);
//        }
//    }

//    public override void ExitState(EnemyAI enemy)
//    {
//        enemy.agent.isStopped = false;
//    }
//}