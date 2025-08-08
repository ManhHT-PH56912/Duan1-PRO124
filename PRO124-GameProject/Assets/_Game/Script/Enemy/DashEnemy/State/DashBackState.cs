using UnityEngine;

public class DashBackState : EnemyState
{
    private float timer = 0.5f;
    private Vector2 dir;

    public DashBackState(DashEnemy enemy, DashEnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.animator.Play("Back");
        dir = (enemy.transform.position - enemy.GetPlayer().position).normalized;

        enemy.rb.linearVelocity = dir * enemy.dashBackSpeed;
        enemy.rb.isKinematic = true;
        enemy.col.isTrigger = true;
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
        enemy.rb.isKinematic = false;
        enemy.col.isTrigger = false;
    }
}
