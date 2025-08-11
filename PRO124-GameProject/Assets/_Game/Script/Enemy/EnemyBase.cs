using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IObserver
{
    protected Transform player;
    public virtual int Health { get; protected set; }
    public virtual int MaxHealth { get; protected set; }
    public virtual float Speed { get; protected set; }
    public virtual int Damage { get; protected set; }
    protected virtual void OnEnable()
    {
        ObserverManager.Instance?.AddObserver(this);
    }

    protected virtual void OnDisable()
    {
        ObserverManager.Instance?.RemoveObserver(this);
    }
    public abstract void TakeDamage(int damage, MonoBehaviour attacker);
    public abstract void Idle();
    public abstract void Move(float speed);
    public abstract void Attack();
    public abstract void Die();
    public virtual void Init()

    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning($"{name}: Player not found!");
            enabled = false;
        }
    }

    public virtual Transform GetPlayer()
    {
        return player;
    }
    public virtual void ReturnPool()
    {
        PoolManager.Instance.Return(gameObject);
        var spawner = GetComponentInParent<EnemySpawner>();
        if (spawner != null)
            spawner.OnEnemyReturned();
    }
    public virtual void OnSpawn()
    {
        Health = MaxHealth;
        gameObject.SetActive(true);
    }


}
