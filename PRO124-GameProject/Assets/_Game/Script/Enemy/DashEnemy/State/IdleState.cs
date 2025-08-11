using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(DashEnemy enemy, DashEnemyStateMachine stateMachine) : base(enemy, stateMachine) { }


    public override void Enter()
    {
        enemy.animator.Play("Idle");
        enemy.col.isTrigger = true;
        enemy.rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        Transform player = enemy.GetPlayer();
        if (player != null)
        {
            float dist = Vector2.Distance(player.position, enemy.transform.position);

            if (dist < 15)
            {
                stateMachine.ChangeState(new DashBackState(enemy, stateMachine));
            }
        }
    }
}



