using UnityEngine;
using UnityEngine.AI;

namespace LHS.Enemy
{
  public class EnemyMeleeAgent : EnemyAgent
  {


    //=========================================



    //=========================================

    public override void OnAttack()
    {
      if (Character == null)
        return;

      if (Character.TryGetComponent(out Health parHealth))
      {
        parHealth.TakeDamage(Config.Damage);
        Debug.Log("Враг нанес урон игроку!");
      }
    }

    //=========================================



    //=========================================
  }
}