using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace LHS.Enemy
{
  public class EnemyController : MonoBehaviour
  {
    [SerializeField] private float _heightDeath = -20f;
    [SerializeField, Min(0)] private float _deathDuration = 10.0f;
    [SerializeField, Min(0)] private float _turningSpeed = 10.0f;

    [SerializeField] private EnemyAttackData _enemyAttackData;

    //=========================================

    public float DeathDuration => _deathDuration;

    //=========================================

    public UnityEvent<int> OnAttack { get; } = new UnityEvent<int>();

    //=========================================

    private void Update()
    {
      //CheckHeightDeath();
    }

    //=========================================
    
    private void CheckHeightDeath()
    {
      if (transform.position.y < _heightDeath)
      {

      }
    }

    //=========================================

    public void TurnAround(Vector3 parLookPosition)
    {
      Vector3 lookDirection = Vector3.ProjectOnPlane(parLookPosition - transform.position, Vector3.up).normalized;
      if (lookDirection.sqrMagnitude != 0f)
      {
        Quaternion playerRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Time.deltaTime * _turningSpeed);
      }
    }

    public EnemyAIState GetEnemyAIState(NavMeshAgent parNavMeshAgent, CharacterBehaviour parCharacterBehaviour)
    {
      return _enemyAttackData.GetEnemyAIState(parNavMeshAgent, parCharacterBehaviour);
    }

    public void Attack()
    {
      if (_enemyAttackData.TryAttack(out int parDamage))
      {
        OnAttack?.Invoke(parDamage);
      }
    }

    public void UpdateAttackDelay()
    {
      _enemyAttackData.UpdateAttackDelay();
    }

    //=========================================

    private void OnDrawGizmos()
    {
      _enemyAttackData.OnDrawGizmos();
    }

    //=========================================
  }
}