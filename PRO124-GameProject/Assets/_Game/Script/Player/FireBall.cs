using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header("Settings")]
    public float damage = 20f;         // Sát thương gây ra
    public float lifeTime = 3f;        // Tự hủy sau X giây nếu không trúng gì
    public LayerMask enemyLayer;       // Layer quái
    public LayerMask wallLayer;        // Layer tường

    private Animator anim;
    private void Start()
    {
        // Tự hủy sau lifeTime
        Destroy(gameObject, lifeTime);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.Play("FireBallMove");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu trúng quái
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
            }
            Destroy(gameObject);
        }

        // Nếu trúng tường
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            // Nếu có animation thì phát
            if (anim != null)
            {
                StartCoroutine(PlayHitAndDestroy());
            }

        }
    }
    private IEnumerator PlayHitAndDestroy()
    {
        if (anim != null)
        {
            anim.Play("FireBallHit");
            yield return new WaitForSeconds(1f); // Chờ animation kết thúc
        }
        Destroy(gameObject);
    }
}