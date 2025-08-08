using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public Rigidbody2D rb;

    public float moveSpeed = 3f;
    public float attackCooldown = 2f;
    public bool IsFacingRight = true;
    public bool allowFlip = true;



    private BossStateMachine stateMachine;
    public Transform firePoint;

    [Header("Ranged Settings")]
    public GameObject slashHitbox;
    public BossSlash slashScript;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stateMachine = new BossStateMachine();
        stateMachine.Initialize(new ÔMaNhiIdleState(this, stateMachine));
    }

    private float skillCooldownTimer = 0f;
    public float skillDelay = 2f;

    private void Update()
    {
        if (allowFlip)
            FacePlayer();

        skillCooldownTimer -= Time.deltaTime;

        if (skillCooldownTimer <= 0f && stateMachine.CurrentState is ÔMaNhiIdleState)
        {
            UseRandomSkill();
            skillCooldownTimer = skillDelay + 2f;
        }

        stateMachine.CurrentState?.LogicUpdate();
    }



    private void FacePlayer()
    {
        if (!allowFlip) return; // Không xoay nếu đang bị khóa

        if (transform.position.x < player.position.x)
            Flip(true);
        else
            Flip(false);
    }



    void FixedUpdate()
    {
        stateMachine.CurrentState?.PhysicsUpdate();
    }
    private void UseRandomSkill()
    {
        float rand = Random.Range(0f, 100f); // Random giá trị từ 0 đến 100

        if (rand < 15f) // 0 - 18.999...
        {
            Debug.Log("Using Attack 1");
            stateMachine.ChangeState(new Attack1State(this, stateMachine));
        }
        else if (rand < 48f) // 19 - 47.999... (29%)
        {
            Debug.Log("Using Attack 2");
            stateMachine.ChangeState(new Attack2State(this, stateMachine));
        }
        else if (rand < 67f) // 48 - 66.999... (19%)
        {
            Debug.Log("Using Attack 3");
            stateMachine.ChangeState(new Attack3State(this, stateMachine));
        }
        else if (rand < 86f) // 67 - 85.999... (19%)
        {
            Debug.Log("Using Attack 4");
            stateMachine.ChangeState(new Attack4State(this, stateMachine));
        }
        else // 86 - 100 (14%)
        {
            Debug.Log("Using Attack 5");
            stateMachine.ChangeState(new Attack5State(this, stateMachine));
        }
    }

    public void Flip(bool faceRight)
    {
        if (IsFacingRight != faceRight)
        {
            IsFacingRight = faceRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    public void StartSlashing()
    {
        allowFlip = false;
        EnableSlash();
        animator.Play("Atk1");
    }

    public void EnableSlash()
    {
        if (slashHitbox != null) slashHitbox.SetActive(true);
        if (slashScript != null) slashScript.EnableDamage();
    }

    public void DisableSlash()
    {
        if (slashScript != null) slashScript.DisableDamage();
        if (slashHitbox != null) slashHitbox.SetActive(false);
    }

    public IEnumerator DisableSlashAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableSlash();
    }




}