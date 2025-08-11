using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class DashEnemy : EnemyBase
{
    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashBackSpeed = 6f;
    public float dashDuration = 0.3f;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D col;
    [HideInInspector] public Animator animator;

    public Transform visual;

    public DashEnemyStateMachine stateMachine;
    public EnemyState CurrentState { get; private set; }
    [SerializeField] private int startHealth = 100;
    public override int MaxHealth { get; protected set; }
    public override int Health { get; protected set; }
    public override float Speed { get; protected set; }
    public override int Damage { get; protected set; }
    public override void Idle() { }

    public override void Move(float speed) { }

    public override void Attack() { }

    private void Awake()
    {
        Damage = 10;
        MaxHealth = startHealth;
        Health = MaxHealth;
        Speed = 2f;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        stateMachine = new DashEnemyStateMachine();
        stateMachine.Initialize(new IdleState(this, stateMachine));
    }

    private void Update()
    {
        stateMachine.CurrentState?.Update();
    }


    public override void TakeDamage(int damage, MonoBehaviour attacker)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (stateMachine.CurrentState is DashState && collision.CompareTag("Player"))
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(Damage);
                Debug.Log("Player bị húc!");
            }
        }
    }

    public void Flip(float directionX)
    {
        if (visual == null) return;
        Vector3 scale = visual.localScale;
        scale.x = directionX > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        visual.localScale = scale;
    }
    public void Initialize(EnemyState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }
    public Transform GetPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        return playerObj != null ? playerObj.transform : null;
    }


}
