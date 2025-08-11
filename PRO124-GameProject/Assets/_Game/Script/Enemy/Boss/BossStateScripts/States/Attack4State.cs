using UnityEngine;

public class Attack4State : BossBaseState
// Skill 4: chém sóng  
{
    private GameObject slashWavePrefab;

    private int waveCount = 3;
    private int currentWave = 0;

    private float timeBetweenWaves = 0.3f;
    private float waveTimer = 0f;

    private float exitDelay = 1f;
    private float exitTimer = 0f;

    public Attack4State(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
        slashWavePrefab = Resources.Load<GameObject>("SlashWave");
    }

    public override void Enter()
    {
        boss.allowFlip = false;
        currentWave = 0;
        waveTimer = 0f;
        exitTimer = 0f;
    }

    public override void LogicUpdate()
    {
        waveTimer += Time.deltaTime;
        boss.animator.Play("Atk3");
        // Phát slash nếu chưa đủ số lượng
        if (currentWave < waveCount && waveTimer >= timeBetweenWaves)
        {
            waveTimer = 0f;
            FireSlashWave();
            currentWave++;

            // Optional: phát lại animation mỗi slash nếu muốn

        }

        // Khi xong 3 slash, chờ rồi chuyển state
        if (currentWave >= waveCount)
        {
            exitTimer += Time.deltaTime;
            if (exitTimer >= exitDelay)
            {
                stateMachine.ChangeState(new ÔMaNhiIdleState(boss, stateMachine));
            }
        }
    }

    private void FireSlashWave()
    {
        Vector2 spawnPos = boss.firePoint.position;
        Vector2 dir = boss.IsFacingRight ? Vector2.right : Vector2.left;

        GameObject wave = GameObject.Instantiate(slashWavePrefab, spawnPos, Quaternion.identity);
        wave.GetComponent<SlashWave>().SetDirection(dir);
    }
}
