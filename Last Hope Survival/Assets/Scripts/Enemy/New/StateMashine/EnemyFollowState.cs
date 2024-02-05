using UnityEngine;
using UnityEngine.AI;

namespace LHS.Enemy
{
  public class EnemyFollowState : EnemyBaseState
  {
    private CharacterBehaviour characterBehaviour;

    private NavMeshAgent navMeshAgent;

    //=========================================

    public override void EnterState(EnemyStateMachine parState)
    {
      characterBehaviour = parState.Agent.Character;

      navMeshAgent = parState.Agent.NavMeshAgent;
    }

    public override void UpdateState(EnemyStateMachine parState)
    {
      if (!parState.Agent.Movement())
      {
        parState.SwitchState(parState.AttackState);
      }
      /*NavMeshPath path = new NavMeshPath();
      navMeshAgent.CalculatePath(characterBehaviour.transform.position, path);

      if (path.status != NavMeshPathStatus.PathPartial)
      {
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
          float distance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
          if (distance > parState.Agent.Config.AttackRadius)
          {
            navMeshAgent.SetDestination(characterBehaviour.transform.position);
#if UNITY_EDITOR
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
#endif
          }
          else
          {
            parState.SwitchState(parState.AttackState);
            return;
          }
        }
      }*/
    }

    //=========================================
  }
}