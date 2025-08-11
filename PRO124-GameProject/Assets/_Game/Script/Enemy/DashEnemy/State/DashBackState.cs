using UnityEngine;

public class DashBackState : EnemyState
{
    private float timer;
    private Vector2 dir;

    public DashBackState(DashEnemy enemy, DashEnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.animator.Play("Back");

        timer = 0.5f;
        Transform player = enemy.GetPlayer();
        if (player != null)
        {
            dir = (enemy.transform.position - player.position).normalized;
            enemy.Flip(-dir.x);
            enemy.rb.linearVelocity = dir * enemy.dashBackSpeed;
        }

        enemy.col.isTrigger = true; // Xuyên va chạm khi lùi
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        float deceleration = enemy.dashBackSpeed / 0.5f * Time.deltaTime;
        enemy.rb.linearVelocity = Vector2.MoveTowards(enemy.rb.linearVelocity, Vector2.zero, deceleration);

        if (timer <= 0f)
        {
            stateMachine.ChangeState(new DashState(enemy, stateMachine));
        }
    }

    public override void Exit()
    {
        enemy.rb.linearVelocity = Vector2.zero;
    }
}
