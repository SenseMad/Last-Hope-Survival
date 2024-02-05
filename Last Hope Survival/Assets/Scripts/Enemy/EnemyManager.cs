using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LHS.Enemy
{
  public class EnemyManager : MonoBehaviour
  {
    [SerializeField] private List<Transform> _spawnPoints;

    [SerializeField, Tooltip("Время задержки спавна"), Min(0)]
    private float _spawnDelayTime = 2.0f;

    //=========================================



    //=========================================



    //=========================================

    private void SpawnEnemies()
    {

    }

    //=========================================
  }
}