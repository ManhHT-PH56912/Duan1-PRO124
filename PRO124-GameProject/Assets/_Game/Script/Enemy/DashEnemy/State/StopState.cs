using UnityEngine;

public class StopState : EnemyState
{
    private float timer;
    private float duration = 1f;
    private Vector2 initialVelocity;

    public StopState(DashEnemy enemy, DashEnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        initialVelocity = enemy.rb.linearVelocity;
        timer = duration;

        enemy.animator.Play("Stun");
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        float t = 1f - (timer / duration);
        enemy.rb.linearVelocity = Vector2.Lerp(initialVelocity, Vector2.zero, t);

        if (timer <= 0f)
        {
            stateMachine.ChangeState(new StunState(enemy, stateMachine));
        }
    }

    public override void Exit()
    {
        enemy.rb.linearVelocity = Vector2.zero;
        // enemy.col.isTrigger = false; // Bật lại va chạm
        float directionX = enemy.GetPlayer().position.x - enemy.transform.position.x;
        // enemy.Flip(directionX);
    }
}
