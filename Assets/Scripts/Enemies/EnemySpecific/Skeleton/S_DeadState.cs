using System.Collections;
using UnityEngine;

public class S_DeadState : DeadState
{
    private Skeleton enemy;

    public S_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, Skeleton enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.StartCoroutine(WaitForDeathAnimation());
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Aktif animasyonun uzunluðunu al
        AnimatorStateInfo animState = entity.anim.GetCurrentAnimatorStateInfo(0);
        float waitTime = animState.length;


        // Süre geçerli deðilse fallback
        if (waitTime <= 0.01f)
            waitTime = 0.5f;

        yield return new WaitForSeconds(waitTime);

        //GameObject.Instantiate(stateData.deathBloodParticle, entity.aliveGO.transform.position, stateData.deathBloodParticle.transform.rotation);
        GameObject.Instantiate(stateData.deathChunkParticle, entity.aliveGO.transform.position, stateData.deathChunkParticle.transform.rotation);

        entity.gameObject.SetActive(false);
    }
}
