using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace LHS.Enemy
{
  public class EnemyAttackState : EnemyBaseState
  {
    private float attackRadius;
    private float attackDelay;

    private Transform attackPoint;

    private CharacterBehaviour character;

    private float currentDelayAttack = 0f;

    //=========================================

    public override void EnterState(EnemyStateMachine parState)
    {
      attackRadius = parState.Agent.Config.AttackRadius;
      attackDelay = parState.Agent.Config.AttackDelay;

      attackPoint = parState.Agent.Config.AttackPoint;

      character = parState.Agent.Character;

      currentDelayAttack = 0;
    }

    public override void UpdateState(EnemyStateMachine parState)
    {
      parState.Agent.TurnTarget();

      bool target = Physics.CheckSphere(parState.Agent.Config.AttackPoint.position, parState.Agent.Config.AttackRadius, parState.Agent.Config.LayerMask);

      if (!target)
      {
        parState.SwitchState(parState.FollowState);
        return;
      }

      currentDelayAttack += Time.deltaTime;
      if (currentDelayAttack >= attackDelay)
      {
        currentDelayAttack = 0;
        parState.Agent.OnAttack();
      }
    }

    //=========================================
  }
}