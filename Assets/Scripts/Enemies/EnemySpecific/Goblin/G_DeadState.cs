using UnityEngine;

public class G_DeadState : DeadState
{
    private Goblin enemy;

    public G_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, Goblin enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    
}
