using UnityEngine;

public class DashState : EnemyState
{
    private float timer;
    private Vector2 dir;

    public DashState(DashEnemy enemy, DashEnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {

        enemy.animator.Play("Atk");

        dir = (enemy.GetPlayer().position.x > enemy.transform.position.x) ? Vector2.right : Vector2.left;

        if (enemy.visual != null)
        {
            Vector3 scale = enemy.visual.localScale;
            scale.x = dir.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            enemy.visual.localScale = scale;
        }

        enemy.rb.linearVelocity = dir * enemy.dashSpeed;
        enemy.rb.isKinematic = true;
        enemy.col.isTrigger = true;

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


    public override void Exit()
    {
    }
}
