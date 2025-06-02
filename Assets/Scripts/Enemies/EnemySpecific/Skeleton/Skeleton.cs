using UnityEngine;

public class Skeleton : Entity
{
    public S_IdleState idleState {  get; private set; }
    public S_MoveState moveState { get; private set; }
    public S_DeadState deadState { get; private set; }
    public S_PlayerDetectedState playerDetectedState { get; private set; }
    public S_ChargeState chargeState { get; private set; }
    public S_LookForPlayerState lookForPlayerState { get; private set; }
    public S_MeleeAttackState meleeAttackState { get; private set; }
    public S_StunState stunState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new S_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new S_MoveState(this, stateMachine, "move", moveStateData, this);
        deadState = new S_DeadState(this, stateMachine, "dead", deadStateData, this);
        playerDetectedState = new S_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        chargeState = new S_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new S_LookForPlayerState(this, stateMachine,"lookForPlayer", lookForPlayerData, this);
        meleeAttackState = new S_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition,meleeAttackStateData, this);
        stunState = new S_StunState(this, stateMachine, "stun", stunStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
