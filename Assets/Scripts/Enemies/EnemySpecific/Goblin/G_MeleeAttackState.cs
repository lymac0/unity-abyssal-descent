using UnityEngine;

public class G_MeleeAttackState : MeleeAttackState
{
    private Goblin enemy;
    public G_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Goblin enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }
}
