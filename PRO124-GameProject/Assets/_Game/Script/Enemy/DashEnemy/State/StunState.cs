using UnityEngine;

public class StunState : EnemyState
{
    private float timer = 2f;

    public StunState(DashEnemy enemy, DashEnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.animator.Play("Idle");
        enemy.rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            stateMachine.ChangeState(new IdleState(enemy, stateMachine));
        }
    }
}
