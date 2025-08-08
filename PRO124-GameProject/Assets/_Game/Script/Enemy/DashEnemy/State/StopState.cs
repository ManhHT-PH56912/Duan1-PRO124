using UnityEngine;

public class StopState : EnemyState
{
    private float timer;
    private float duration = 1f; // thời gian giảm tốc
    private Vector2 initialVelocity;
    private Vector2 dir;

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
        enemy.rb.isKinematic = false;
        // enemy.col.isTrigger = false;
        float directionX = enemy.GetPlayer().position.x - enemy.transform.position.x;
        enemy.Flip(directionX);
    }
}
