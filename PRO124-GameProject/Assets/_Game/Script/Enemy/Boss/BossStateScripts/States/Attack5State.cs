using UnityEngine;

public class Attack5State : BossBaseState
// Skill 5: ném bom lửa
{
    private GameObject fireBombPrefab;
    private bool hasThrown = false;
    private float timer = 0f;

    public Attack5State(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
        fireBombPrefab = Resources.Load<GameObject>("FireBomb");
    }

    public override void Enter()
    {
        boss.animator.Play("Atk");
        hasThrown = false;
        timer = 0f;
    }

    public override void LogicUpdate()
    {
        timer += Time.deltaTime;

        if (!hasThrown && timer >= 0.5f)
        {
            ThrowFireBombs();
            hasThrown = true;
        }

        if (timer >= 1.5f)
        {
            stateMachine.ChangeState(new ÔMaNhiIdleState(boss, stateMachine));
        }
    }

    private void ThrowFireBombs()
    {
        Vector2 spawnPos = boss.firePoint.position;
        Vector2 targetPos = boss.player.position;

        float spacing = 3.5f; // Tăng khoảng cách giữa các firebomb
        Vector2 offset = new Vector2(spacing, 0); // Đặt khoảng cách ngang
        int bombCount = 4; // Số lượng bom lửa sẽ ném
        for (int i = -bombCount / 2; i <= bombCount / 2; i++)
        {
            Vector2 finalTarget = targetPos + i * offset;

            GameObject bomb = GameObject.Instantiate(fireBombPrefab, spawnPos, Quaternion.identity);
            bomb.GetComponent<FireBomb>().LaunchParabola(finalTarget);
        }
    }

}