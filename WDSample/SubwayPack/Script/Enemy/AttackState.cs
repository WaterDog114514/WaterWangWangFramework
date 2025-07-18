//using DG.Tweening;
//using System.Collections;
//using UnityEngine;

//public class AttackState : EnemyBaseState
//{
//    public bool IsAttackCD;
//    private Transform enemyTrans;
//    Transform playerTrans;
//    private AudioSource audio;
//    private AudioClip KnockedAudio;
//    public AttackState(Transform enemyTrans, Transform playerTrans, AudioSource audio, AudioClip KnockedAudio)
//    {
//        this.enemyTrans = enemyTrans;
//        this.playerTrans = playerTrans;
//        this.audio = audio;
//        this.KnockedAudio = KnockedAudio;
//    }
//    public float LockEnemyHeight;
//    public override void EnterState(EnemyAI enemy)
//    {
//        if (IsAttackCD)
//        {
//            Debug.Log("攻击CD");
//            enemy.TransitionToState(enemy.AlertState);
//            return;
//        }
//        enemy.agent.isStopped = true;
//        enemy.StartCoroutine(AttackProcess(enemy));
//    }
//    public IEnumerator AttackProcess(EnemyAI enemy)
//    {

//        // 0.5f移动视角
//        LockPlayerCamera();
//        yield return new WaitForSeconds(0.5F);
//        //开始攻击
//        enemy.animator.SetTrigger("Attack");
//        //之后锁定视角
//        float timer = 0F;
//        //攻击持续时间
//        while (timer < 1.0F)
//        {
//            timer += Time.deltaTime;
//            yield return null;
//        }
//        //打飞玩家
//        enemy.StartCoroutine(KnockbackCoroutine());

//        yield return new WaitForSeconds(0.5F);
//        //解锁
//        UnLockPlayerCamera();
//        //结束状态
//        enemy.TransitionToState(enemy.ChaseState);
//        //开始计算CD;
//        IsAttackCD = true;
//        yield return new WaitForSeconds(5f);
//        IsAttackCD = false;

//    }
//    //一直锁定
//    public void LockPlayerCamera()
//    {
//        var controller = playerTrans.GetComponent<PlayerController>();
//        controller.IsCanRotateX = false;
//        Quaternion oldRotation = playerTrans.rotation;
//        //计算玩家面朝角
//        Vector3 directionToEnemy = enemyTrans.position + Vector3.up * LockEnemyHeight - playerTrans.transform.position;
//        playerTrans.rotation = Quaternion.LookRotation(directionToEnemy);
//        float angle = playerTrans.rotation.eulerAngles.y;
//        controller.angleX = angle;
//        //算完后还原
//        playerTrans.rotation = oldRotation;
//        //设置玩家视角
//        playerTrans.transform.DORotateQuaternion(Quaternion.Euler(0, angle, 0), 0.5f)
//            .SetEase(Ease.InOutQuad);
//    }
//    private void UnLockPlayerCamera()
//    {
//        var controller = playerTrans.GetComponent<PlayerController>();
//        controller.IsCanRotateX = true;
//        //还原玩家视角转身
//        playerTrans.transform.DORotateQuaternion(Quaternion.LookRotation(-playerTrans.forward), 0.5f)
//            .SetEase(Ease.InOutQuad).onComplete = () =>
//            {
//                controller.angleX = playerTrans.rotation.eulerAngles.y;
//            };

//    }


//    public override void UpdateState(EnemyAI enemy)
//    {
//    }

//    public override void ExitState(EnemyAI enemy)
//    {
//        enemy.agent.isStopped = false;
//    }


//    private IEnumerator KnockbackCoroutine()
//    {
//        //播放挨打
//        audio.PlayOneShot(KnockedAudio);
//        var player = playerTrans;
//        CharacterController controller = player.GetComponent<CharacterController>();

//        // 计算击飞方向 (后方30度)
//        Vector3 knockbackDirection = -player.forward;
//        knockbackDirection = Quaternion.Euler(0, 30, 0) * knockbackDirection;

//        float duration = 0.8f;
//        float timer = 0f;
//        float height = 0.5f;
//        float distance = 3f;

//        Vector3 startPos = player.position;
//        Vector3 endPos = player.position + knockbackDirection * distance;

//        while (timer < duration)
//        {
//            timer += Time.deltaTime;
//            float progress = timer / duration;

//            // 抛物线运动
//            float vertical = Mathf.Sin(progress * Mathf.PI) * height;
//            Vector3 newPos = Vector3.Lerp(startPos, endPos, progress) + Vector3.up * vertical;

//            // 使用Move移动，会自动处理碰撞
//            controller.Move(newPos - player.position);

//            yield return null;
//        }
//    }
//}