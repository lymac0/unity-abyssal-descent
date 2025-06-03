using NUnit.Framework.Interfaces;
using UnityEngine;

public class Necromancer : Entity
{
    public Necro_IdleState idleState { get; private set; }
    public Necro_MoveState moveState { get; private set; }
    public Necro_DeadState deadState { get; private set; }
    public Necro_PlayerDetectedState playerDetectedState { get; private set; }
    public Necro_LookForPlayerState lookForPlayerState { get; private set; }
    //public Necro_ChargeState chargeState { get; private set; }
    public Necro_MeleeAttackState meleeAttackState { get; private set; }
    public Necro_StunState stunState { get; private set; }
    public Necro_DodgeState dodgeState { get; private set; }
    public Necro_RangedAttackState rangedAttackState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerData;
    //[SerializeField]
    //private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    public D_DodgeState dodgeStateData;
    [SerializeField]
    public D_RangedAttackState rangedAttackStateData;

    [SerializeField] 
    private GameObject lightningEffect;
    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform rangedAttackPosition;


    public override void Start()
    {
        base.Start();

        idleState = new Necro_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Necro_MoveState(this, stateMachine, "move", moveStateData, this);
        deadState = new Necro_DeadState(this, stateMachine, "dead", deadStateData, this);
        playerDetectedState = new Necro_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        //chargeState = new Necro_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new Necro_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerData, this);
        meleeAttackState = new Necro_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this, lightningEffect);
        stunState = new Necro_StunState(this, stateMachine, "stun", stunStateData, this);
        dodgeState = new Necro_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangedAttackState = new Necro_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);

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
        else if(CheckPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(rangedAttackState);
        }
        else if (!CheckPlayerInMinAgroRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

}
