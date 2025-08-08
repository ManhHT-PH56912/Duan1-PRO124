using UnityEngine;

public class Attack2State : BossBaseState
{
    private enum Phase { BackStep, Pause, Dash, End }
    private Phase currentPhase;

    private float backStepDuration = 0.3f;
    private float pauseDuration = 0.3f;
    private float dashDuration = 0.7f;

    private float timer;
    private Vector2 dashDir;
    private bool hasDashed = false;

    public Attack2State(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine) { }

    public override void Enter()
    {
        currentPhase = Phase.BackStep;
        timer = 0f;
        hasDashed = false;
        boss.animator.Play("Flash");

        // Xoay mặt về phía player trước khi lùi
        if (boss.player.position.x < boss.transform.position.x)
            boss.Flip(false);
        else
            boss.Flip(true);
    }

    public override void LogicUpdate()
    {
        timer += Time.deltaTime;

        switch (currentPhase)
        {
            case Phase.BackStep:
                DoBackStep();
                break;

            case Phase.Pause:
                if (timer >= pauseDuration)
                {
                    StartDash();
                }
                break;

            case Phase.Dash:
                if (timer >= dashDuration)
                {
                    EndDash();
                }
                break;

            case Phase.End:
                if (timer >= 0.3f)
                {
                    stateMachine.ChangeState(new ÔMaNhiIdleState(boss, stateMachine));
                }
                break;
        }
    }

    private void DoBackStep()
    {
        Vector2 backDir = boss.IsFacingRight ? Vector2.left : Vector2.right;
        boss.rb.linearVelocity = backDir * 5f;

        if (timer >= backStepDuration)
        {
            boss.rb.linearVelocity = Vector2.zero;
            timer = 0f;
            currentPhase = Phase.Pause;
        }
    }

    private void StartDash()
    {
        boss.animator.Play("Atk");

        dashDir = boss.IsFacingRight ? Vector2.right : Vector2.left;
        boss.rb.linearVelocity = dashDir * 20f;

        boss.EnableSlash(); // Bật chém
        boss.StartCoroutine(boss.DisableSlashAfterDelay(dashDuration)); // Tự tắt sau khi dash

        timer = 0f;
        currentPhase = Phase.Dash;
    }

    private void EndDash()
    {
        boss.rb.linearVelocity = Vector2.zero;
        timer = 0f;
        currentPhase = Phase.End;
    }
}
