using UnityEngine;
using System.Collections;

public class Attack3State : BossBaseState
// bắn mũi tên
{
    private float delayBetweenWaves = 0.6f;
    private float waveTimer = 0f;

    private int currentWave = 0;
    private int totalWaves = 3;

    private int arrowCountPerWave = 9;
    private float spacing = 3f;
    private float arrowHeight = 5f;

    private bool done = false;

    private GameObject arrowPrefab;

    public Attack3State(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
        arrowPrefab = Resources.Load<GameObject>("Arrow");
    }

    public override void Enter()
    {

        boss.animator.Play("Atk5");

        currentWave = 0;
        waveTimer = 0f;
        done = false;
    }

    public override void LogicUpdate()
    {
        if (done)
            return;

        waveTimer += Time.deltaTime;

        if (waveTimer >= delayBetweenWaves)
        {
            waveTimer = 0f;

            SpawnArrowWave();

            currentWave++;
            if (currentWave >= totalWaves)
            {
                done = true;
                boss.StartCoroutine(DelayBeforeNextAttack());
            }
        }
    }

    private void SpawnArrowWave()
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("Arrow prefab not found!");
            return;
        }

        float startX = boss.player.position.x - spacing * (arrowCountPerWave - 1) / 2f;
        float yPos = boss.player.position.y + arrowHeight;

        for (int i = 0; i < arrowCountPerWave; i++)
        {
            Vector2 spawnPos = new Vector2(startX + i * spacing, yPos);
            GameObject arrow = GameObject.Instantiate(arrowPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0f;

                // Sau 0.4s mới bắt đầu rơi
                boss.StartCoroutine(DelayedFall(rb, 0.4f));
            }
        }
    }

    private IEnumerator DelayedFall(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (rb != null)
        {
            ArrowBoss arrow = rb.GetComponent<ArrowBoss>();
            if (arrow != null)
            {
                arrow.StartFalling();
            }
        }
    }


    private IEnumerator DelayBeforeNextAttack()
    {
        yield return new WaitForSeconds(1f);
        stateMachine.ChangeState(new ÔMaNhiIdleState(boss, stateMachine));
    }
}
