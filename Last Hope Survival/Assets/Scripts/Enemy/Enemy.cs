using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

namespace LHS.Enemy
{
  public class Enemy : MonoBehaviour
  {
    private EnemyAIState enemyAIState;

    private Collider enemyCollider;

    private CharacterBehaviour characterBehaviour;
    private EnemyController enemyController;
    private Health health;

    private NavMeshAgent navMeshAgent;

    private Animator animator;

    private Rigidbody[] rigidbodies;

    //=========================================

    private void Awake()
    {
      enemyCollider = GetComponent<Collider>();

      enemyController = GetComponent<EnemyController>();
      health = GetComponent<Health>();

      navMeshAgent = GetComponent<NavMeshAgent>();

      animator = GetComponentInChildren<Animator>();

      rigidbodies = GetComponentsInChildren<Rigidbody>();

      RigidbodyIsKinematicRagdoll(true);
    }

    private void Start()
    {
      characterBehaviour = CharacterBehaviour.Instance;
    }

    private void OnEnable()
    {
      enemyController.OnAttack.AddListener(Attack);

      health.OnInstantlyKill.AddListener(Death);
    }

    private void OnDisable()
    {
      enemyController.OnAttack.RemoveListener(Attack);

      health.OnInstantlyKill.RemoveListener(Death);
    }

    private void Update()
    {
      UpdateEnemyAIState();
    }

    //=========================================

    private void UpdateEnemyAIState()
    {
      if (enemyAIState == EnemyAIState.Died)
        return;

      enemyAIState = enemyController.GetEnemyAIState(navMeshAgent, characterBehaviour);

      bool enemyAIStatesAnim = false;

      switch (enemyAIState)
      {
        case EnemyAIState.Idle:
          break;
        case EnemyAIState.Follow:
          navMeshAgent.SetDestination(characterBehaviour.transform.position);
          enemyController.TurnAround(characterBehaviour.transform.position);
          enemyController.UpdateAttackDelay();

          enemyAIStatesAnim = false;
          break;
        case EnemyAIState.Attack:
          navMeshAgent.SetDestination(transform.position);
          enemyController.Attack();
          enemyController.TurnAround(characterBehaviour.transform.position);

          enemyAIStatesAnim = true;
          break;
        case EnemyAIState.Died:
          break;
      }

      animator.SetBool("EnemyAIStates", enemyAIStatesAnim);
    }

    private void RigidbodyIsKinematicRagdoll(bool parValue)
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
    }

    private void Attack(int parDamage)
    {
      if (characterBehaviour == null)
        return;

      if (characterBehaviour.TryGetComponent(out Health parHealth))
      {
        parHealth.TakeDamage(parDamage);
        Debug.Log("Враг нанес урон");
      }
    }

    //=========================================
  }
}