using System.Collections;
using UnityEngine;

namespace Weapons
{
  public sealed class Projectile : MonoBehaviour
  {
    [SerializeField, Tooltip("Время до удаления пули после выстрела")]
    private float _destroyAfter = 2.0f;

    [SerializeField, Tooltip("Время удаления эффекта после попадания")]
    private float _timeDestroyEffect = 1.0f;

    [Header("ЭФФЕКТЫ")]
    [SerializeField, Tooltip("Эффект попадания по врагу")]
    private Transform _effectHittingEnemy;
    [SerializeField, Tooltip("Эффект попадания")]
    private Transform _effectHit;

    //-----------------------------------------

    public Rigidbody Rigidbody { get; private set; }

    /// <summary>
    /// Урон снаряда
    /// </summary>
    private int projectileDamage;

    //=========================================

    private void Awake()
    {
      Rigidbody = GetComponent<Rigidbody>();

      Physics.IgnoreCollision(CharacterBehaviour.Instance.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void Start()
    {
      StartCoroutine(DestroyAfter());
    }

    private void OnCollisionEnter(Collision collision)
    {
      Rigidbody.velocity = Vector3.zero;

      StartCoroutine(DestroyTimer());

      CreateHitEffect(collision);

      /*if (collision.transform.TryGetComponent<Health>(out var health))
      {
        health.TakeHealth(ProjectileDamage);
      }*/
    }

    //=========================================

    /// <summary>
    /// Создать эффект попадания
    /// </summary>
    private void CreateHitEffect(Collision collision)
    {
      if (_effectHit == null)
        return;

      Transform effect = Instantiate(_effectHit, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));

      Destroy(effect.gameObject, _timeDestroyEffect);
    }

    //=========================================

    /// <summary>
    /// Удаление пули после попадания
    /// </summary>
    private IEnumerator DestroyTimer()
    {
      yield return new WaitForSeconds(0.02f);

      Destroy(gameObject);
    }

    /// <summary>
    /// Удаление пули после создания
    /// </summary>
    private IEnumerator DestroyAfter()
    {
      yield return new WaitForSeconds(_destroyAfter);

      Destroy(gameObject);
    }

    //=========================================
  }
}