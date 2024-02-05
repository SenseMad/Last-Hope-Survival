using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

namespace LHS.Enemy
{
  public class EnemyAgent : MonoBehaviour
  {
    [SerializeField] private EnemyConfig _config;

    //-----------------------------------------

    private EnemyStateMachine stateMachine;

    private NavMeshAgent navMeshAgent;

    private CharacterBehaviour character;

    //=========================================

    public EnemyConfig Config { get => _config; private set => _config = value; }

    public NavMeshAgent NavMeshAgent { get => navMeshAgent; private set => navMeshAgent = value; }

    public CharacterBehaviour Character { get => character; private set => character = value; }

    //=========================================

    private void Awake()
    {
      character = CharacterBehaviour.Instance;

      navMeshAgent = GetComponent<NavMeshAgent>();

      stateMachine = GetComponent<EnemyStateMachine>();
    }

    //=========================================

    public void TurnTarget()
    {
      Vector3 lookDirection = Vector3.ProjectOnPlane(character.transform.position - transform.position, Vector3.up).normalized;
      if (lookDirection.sqrMagnitude != 0f)
      {
        Quaternion playerRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Time.deltaTime * _config.MoveSpeed);
      }
    }

    public virtual void OnAttack() { }

    public virtual bool Movement()
    {
      NavMeshPath path = new NavMeshPath();
      navMeshAgent.CalculatePath(Character.transform.position, path);

      if (path.status != NavMeshPathStatus.PathPartial)
      {
        bool target = Physics.CheckSphere(_config.AttackPoint.position, _config.AttackRadius, _config.LayerMask);

        if (!target)
        {
          NavMeshAgent.SetDestination(character.transform.position);

          return true;
        }

        return false;
      }

      return false;
    }

    //=========================================

    /*private void RigidbodyIsKinematicRagdoll(bool parValue)
    {
      animator.enabled = parValue;
      for (int i = 0; i < rigidbodies.Length; i++)
      {
        rigidbodies[i].isKinematic = parValue;
      }
    }

    private void Death()
    {
      enemyAIState = EnemyAIState.Died;
      navMeshAgent.SetDestination(transform.position);

      enemyCollider.enabled = false;
      RigidbodyIsKinematicRagdoll(false);

      Destroy(gameObject, enemyController.DeathDuration);
    }*/

    //=========================================

    public void OnDrawGizmos()
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(_config.AttackPoint.position, _config.AttackRadius);
    }

    //=========================================
  }
}