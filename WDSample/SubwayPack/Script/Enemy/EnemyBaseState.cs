//public abstract class EnemyBaseState
//{
//    public abstract void EnterState(EnemyAI enemy);
//    public abstract void UpdateState(EnemyAI enemy);
//    public abstract void ExitState(EnemyAI enemy);

//    // 新增方法用于处理公共状态转换
//    protected void CheckForPlayer(EnemyAI enemy)
//    {
//        if (enemy.PlayerInSight)
//        {
//            enemy.TransitionToState(enemy.AlertState);
//        }
//    }
//}