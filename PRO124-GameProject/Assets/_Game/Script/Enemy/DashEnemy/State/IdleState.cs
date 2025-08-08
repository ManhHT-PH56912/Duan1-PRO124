using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(DashEnemy enemy, DashEnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.animator.Play("Idle");
        enemy.rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        Transform player = enemy.GetPlayer();
        if (player != null && Mathf.Abs(player.position.x - enemy.transform.position.x) < 6f)
        {
            stateMachine.ChangeState(new DashBackState(enemy, stateMachine));
        }
    }
}
