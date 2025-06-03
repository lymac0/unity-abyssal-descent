using UnityEngine;

public class Necro_MeleeAttackState : MeleeAttackState
{
    private Necromancer enemy;
    private GameObject lightningEffect;
    public Necro_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Necromancer enemy, GameObject lightningEffect) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
        this.lightningEffect = lightningEffect;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isPlayerIsMinAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else if (!isPlayerIsMinAgroRange)
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        GameObject.Instantiate(lightningEffect, attackPosition.position, Quaternion.identity);

    }
}
