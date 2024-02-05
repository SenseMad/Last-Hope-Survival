using UnityEngine;

namespace LHS.Enemy
{
  [System.Serializable]
  public class EnemyConfig
  {
    [Header("ATTACK")]
    [SerializeField, Min(0)] private int _damage;
    [SerializeField] private Transform _attackPoint;
    [SerializeField, Min(0)] private float _attackRadius = 1.0f;
    [SerializeField, Min(0)] private float _attackDelay = 1.0f;
    [SerializeField] private LayerMask _layerMask;

    [Header("MOVE")]
    private float _moveSpeed = 9;

    //=========================================

    public int Damage { get => _damage; private set => _damage = value; }
    public Transform AttackPoint { get => _attackPoint; private set => _attackPoint = value; }
    public float AttackRadius { get => _attackRadius; private set => _attackRadius = value; }
    public float AttackDelay { get => _attackDelay; private set => _attackDelay = value; }
    public LayerMask LayerMask { get => _layerMask; private set => _layerMask = value; }

    public float MoveSpeed { get => _moveSpeed; private set => _moveSpeed = value; }

    //=========================================
  }
}