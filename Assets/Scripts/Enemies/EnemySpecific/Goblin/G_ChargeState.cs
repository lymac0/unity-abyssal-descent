using UnityEngine;

public class G_ChargeState : ChargeState
{
    private Goblin enemy;

    public G_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Goblin enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }


}
