using System.Collections;
using UnityEngine;

public class MeeleEnemy : EnemyBase, IObserver
{
    // ====== AI settings ======
    [Header("AI Settings")]
    [SerializeField] private float aggroRange = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private float WakeSpeed = 0.3f;
    [SerializeField] private float returnThreshold = 0.4f;

    [Header("Attack Settings")]
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackCooldown = 1.2f;

    [SerializeField] private Transform visual;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private Vector2 groundCheckOffset = new Vector2(0.5f, 0);

    [Header("Effects")]
    [SerializeField] private BloodEffectSpawner bloodSpawner;

    private enum State { Idle, Move, Attack, Return }
    private State currentState = State.Idle;

    private Vector3 startPos;
    private int patrolDir = 1;
    private float lastAttackTime = -999f;
    private bool isAttacking = false;

    private void Awake()
    {
    }

    private void Start()
    {
        Health = 100;
        MaxHealth = 100;
        Speed = 2f;
        Damage = 10;
        base.Init();
        startPos = transform.position;
    }

    private void Update()
    {
        EvaluateState();
        switch (currentState)
        {
            case State.Idle: Idle(); break;
            case State.Move: Move(Speed); break;
            case State.Attack: Attack(); break;
            case State.Return: ReturnToStart(); break;
        }
    }

    public void EvaluateState()
    {
        if (player == null) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= aggroRange)
        {
            currentState = distToPlayer > attackRange ? State.Move : State.Attack;
        }
        else
        {
            float distToStart = Vector3.Distance(transform.position, startPos);
            if (distToStart > returnThreshold && currentState != State.Idle)
            {
                currentState = State.Return;
            }
            else
            {
                currentState = State.Idle;
            }
        }
    }

    public override void Idle()
    {
        transform.Translate(Vector3.right * patrolDir * Speed * WakeSpeed * Time.deltaTime);

        float offset = transform.position.x - startPos.x;
        if (Mathf.Abs(offset) >= patrolDistance)
        {
            patrolDir *= -1;
            float clampX = startPos.x + Mathf.Clamp(offset, -patrolDistance, patrolDistance);
            transform.position = new Vector3(clampX, transform.position.y, transform.position.z);
        }

        Flip(patrolDir);
    }

    public override void Move(float speed)
    {
        if (isAttacking) return;

        // Raycast kiểm tra phía trước có còn mặt đất không
        if (!IsGroundAhead())
        {
            currentState = State.Return;
            return;
        }

        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dir = (targetPos - transform.position).normalized;

        transform.Translate(dir * speed * Time.deltaTime);

        if (dir.x != 0)
            Flip(Mathf.Sign(dir.x));
    }

    private void ReturnToStart()
    {
        Vector3 dir = (startPos - transform.position).normalized;
        transform.Translate(dir * Speed * Time.deltaTime);

        if (dir.x != 0)
            Flip(Mathf.Sign(dir.x));

        if (Vector3.Distance(transform.position, startPos) < returnThreshold)
        {
            currentState = State.Idle;
        }
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        StartCoroutine(DoMeleeAttack());
        lastAttackTime = Time.time;
    }

    private IEnumerator DoMeleeAttack()
    {
        isAttacking = true;
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        attackHitbox.SetActive(false);
        isAttacking = false;
    }

    public override void TakeDamage(int damage, MonoBehaviour attacker)
    {
        Health -= damage;
        if (bloodSpawner != null && attacker != null)
        {
            bool flipX = attacker.transform.position.x > transform.position.x;
            bloodSpawner.SpawnBlood(transform.position, flipX);
        }
        if (Health <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        ReturnPool();
        Debug.Log("MeeleEnemy died");
    }
    public void OnSpawned()
    {
        Health = MaxHealth;
        currentState = State.Idle;
        isAttacking = false;
        lastAttackTime = -999f;
        transform.position = startPos;
    }


    public void Flip(float dirX)
    {
        if (visual != null)
        {
            Vector3 vScale = visual.localScale;
            vScale.x = dirX * Mathf.Abs(vScale.x);
            visual.localScale = vScale;
        }

        patrolDir = (int)Mathf.Sign(dirX); // Cập nhật hướng tuần tra để IsGroundAhead đúng
    }



    private bool IsGroundAhead()
    {
        Vector2 origin = transform.position;
        origin += new Vector2(patrolDir * groundCheckOffset.x, groundCheckOffset.y);

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
#if UNITY_EDITOR
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, hit.collider ? Color.green : Color.red);
#endif
        return hit.collider != null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
