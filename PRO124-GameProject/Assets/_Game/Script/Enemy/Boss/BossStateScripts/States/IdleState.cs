using UnityEngine;
using System.Collections;

public class ÔMaNhiIdleState : BossBaseState
{
    private float timer;
    private float delay = 2f;

    public ÔMaNhiIdleState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine) { }

    public override void Enter()
    {
        boss.allowFlip = true;
        boss.rb.linearVelocity = Vector2.zero;
        timer = 0;
        boss.animator.Play("Idle");
    }

    public override void LogicUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            int rnd = Random.Range(1, 6);
            switch (rnd)
            {
                case 1:
                    stateMachine.ChangeState(new Attack1State(boss, stateMachine));
                    break;
                case 2:
                    stateMachine.ChangeState(new Attack2State(boss, stateMachine));
                    break;
                case 3:
                    stateMachine.ChangeState(new Attack3State(boss, stateMachine));
                    break;
                case 4:
                    stateMachine.ChangeState(new Attack4State(boss, stateMachine));
                    break;
                case 5:
                    stateMachine.ChangeState(new Attack5State(boss, stateMachine));
                    break;
            }
        }
    }
}