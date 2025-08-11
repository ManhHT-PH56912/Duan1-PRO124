using UnityEngine;

public class DashState : EnemyState
{
    private float timer;
    private Vector2 dir;

    public DashState(DashEnemy enemy, DashEnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.animator.Play("Atk");

        Transform player = enemy.GetPlayer();
        if (player != null)
        {
            dir = (player.position.x > enemy.transform.position.x) ? Vector2.right : Vector2.left;
            enemy.rb.linearVelocity = dir * enemy.dashSpeed;
        }

        enemy.col.isTrigger = true; // Xuyên khi dash
        timer = enemy.dashDuration;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            stateMachine.ChangeState(new StopState(enemy, stateMachine));
        }
    }
}
