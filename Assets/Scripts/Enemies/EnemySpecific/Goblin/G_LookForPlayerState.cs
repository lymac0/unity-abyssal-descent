using UnityEngine;

public class G_LookForPlayerState : LookForPlayerState
{
    private Goblin enemy;

    public G_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayer stateData, Goblin enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    
}
