public abstract class EnemyState
{
    protected DashEnemy enemy;
    protected DashEnemyStateMachine stateMachine;

    protected EnemyState(DashEnemy enemy, DashEnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
