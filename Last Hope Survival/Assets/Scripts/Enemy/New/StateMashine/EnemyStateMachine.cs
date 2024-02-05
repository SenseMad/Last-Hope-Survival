using UnityEngine;

namespace LHS.Enemy
{
  public class EnemyStateMachine : MonoBehaviour
  {
    private EnemyBaseState currentState;

    private readonly EnemyIdleState idleState = new EnemyIdleState();
    private readonly EnemyFollowState followState = new EnemyFollowState();
    private readonly EnemyAttackState attackState = new EnemyAttackState();
    private readonly EnemyDiedState diedState = new EnemyDiedState();

    //=========================================

    public EnemyAgent Agent { get; private set; }

    //-----------------------------------------

    public EnemyIdleState IdleState => idleState;
    public EnemyFollowState FollowState => followState;
    public EnemyAttackState AttackState => attackState;
    public EnemyDiedState DiedState => diedState;

    //=========================================

    private void Awake()
    {
      Agent = GetComponent<EnemyAgent>();
    }

    private void Start()
    {
      currentState = followState;

      currentState.EnterState(this);
    }

    private void Update()
    {
      currentState.UpdateState(this);
    }

    //=========================================

    public void SwitchState(EnemyBaseState parState)
    {
      currentState = parState;
      parState.EnterState(this);
    }

    //=========================================
  }
}