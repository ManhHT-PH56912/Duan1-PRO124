using UnityEngine;

public class Attack1State : BossBaseState
// Skill 1: hút player lại rồi chém liên tiếp
{
    private float suckDuration = 3f;
    private float suckSpeed = 5f;
    private float suckTimer = 0f;

    private bool isSlashing = false;

    private int slashCount = 0;
    private float slashCooldown = 0.5f;
    private float slashTimer = 0f;

    private Transform player;

    public Attack1State(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
        player = boss.player;
    }

    public override void Enter()
    {

        suckTimer = 0f;
        slashCount = 0;
        slashTimer = 0f;
        isSlashing = false;
    }

    public override void LogicUpdate()
    {
        if (!isSlashing)
        {

            boss.animator.Play("Atk5");
            boss.allowFlip = false;
            // Giai đoạn hút player
            suckTimer += Time.deltaTime;

            Vector2 direction = (boss.transform.position - player.position).normalized;
            player.position += (Vector3)(direction * suckSpeed * Time.deltaTime);

            float distance = Vector2.Distance(boss.transform.position, player.position);

            if (distance < 0.5f)
            {
                boss.allowFlip = true;
                // Nếu player đã đủ gần → bắt đầu chém ngay
                StartSlashing();
                return;
            }

            if (suckTimer >= suckDuration)
            {
                boss.allowFlip = true;
                stateMachine.ChangeState(new ÔMaNhiIdleState(boss, stateMachine));
            }
        }
        else
        {
            // Giai đoạn chém liên tiếp
            slashTimer += Time.deltaTime;

            if (slashTimer >= slashCooldown && slashCount < 3)
            {
                slashTimer = 0f;
                slashCount++;
                boss.animator.Play("Atk" + slashCount);

                boss.EnableSlash();
                boss.StartCoroutine(boss.DisableSlashAfterDelay(0.3f));
            }

            if (slashCount >= 3 && slashTimer >= slashCooldown)
            {
                boss.allowFlip = true;
                stateMachine.ChangeState(new ÔMaNhiIdleState(boss, stateMachine));
            }
        }
    }

    private void StartSlashing()
    {

        boss.animator.Play("Atk1");
        boss.allowFlip = false;
        isSlashing = true;
        slashCount = 1;
        slashTimer = 0f;
        boss.allowFlip = true;
        boss.EnableSlash();
        boss.StartCoroutine(boss.DisableSlashAfterDelay(0.3f));
    }
}
