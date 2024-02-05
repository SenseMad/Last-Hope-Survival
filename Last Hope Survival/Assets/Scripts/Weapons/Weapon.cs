using LHS.Enemy;
using System.Collections;
using UnityEngine;

namespace Weapons
{
  public sealed class Weapon : WeaponBehaviour
  {
    [Header("УРОН")]
    [SerializeField, Min(0)] private int _damage = 10;

    [Header("СТРЕЛЬБА")]
    [SerializeField] private TypesShooting _typeShooting;
    [SerializeField, Min(0)] private int _shotCount = 1;
    [SerializeField, Min(0)] private int _shotsPerMinutes = 200;
    [SerializeField] private Transform[] _startPoints;

    [Header("ЛУЧ")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField, Min(0)] private float _distance = Mathf.Infinity;

    [Header("РАЗБРОС")]
    [SerializeField] private bool _useSpread = true;
    [SerializeField, Min(0)] private float _spreadFactor = 1.0f;

    [Header("ПАТРОНЫ")]
    [SerializeField, Min(0)] private int _maxAmountAmmo = 100;
    [SerializeField, Min(0)] private int _maxAmountAmmoInMagazine = 20;

    [Header("ПЕРЕЗАРЯДКА")]
    [SerializeField] private bool _autoRecharge = false;
    [SerializeField, Min(0)] private float _rechargeTime = 1.0f;

    [Header("ЭФФЕКТЫ")]
    [SerializeField] private Transform _muzzleEffectPrefab;
    [SerializeField] private Transform _hitEffectPrefab;
    [SerializeField, Min(0)] private float _hitEffectDestroyDelay = 2.0f;

    [Header("ЗВУКИ")]
    [SerializeField] private AudioClip _audioClipFire;
    [SerializeField] private AudioClip _audioClipRecharge;

    //-----------------------------------------

    private CharacterBehaviour characterBehaviour;

    private AudioSource audioSource;

    private Coroutine coroutineRecharge;

    private int currentAmountAmmo;
    private int currentAmountAmmoInMagazine;

    private float lastShotTime;

    //=========================================

    public override int GetAmmunitionTotal() => _maxAmountAmmoInMagazine;
    public override int GetAmmunitionCurrent() => currentAmountAmmoInMagazine;

    public override int GetWeaponDamage() => _damage;
    public override float GetRateOfFire() => _shotsPerMinutes;

    public override TypesShooting GetTypeShooting() => _typeShooting;

    //=========================================

    private void Awake()
    {
      characterBehaviour = CharacterBehaviour.Instance;

      audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
      currentAmountAmmo = _maxAmountAmmo;
      currentAmountAmmoInMagazine = _maxAmountAmmoInMagazine;
    }

    //=========================================

    public override void PerformAttack()
    {
      if (coroutineRecharge != null)
        return;

      if (currentAmountAmmoInMagazine == 0)
        return;

      if (!(Time.time - lastShotTime > 60.0f / _shotsPerMinutes))
        return;

      for (int i = 0; i < _shotCount; i++)
      {
        PerformRaycast();
      }

      PerformEffects();

      lastShotTime = Time.time;

      currentAmountAmmoInMagazine--;

      if (_autoRecharge && currentAmountAmmoInMagazine == 0)
      {
        PerformRecharge();
      }
    }

    public override void PerformRecharge()
    {
      Recharge(_maxAmountAmmoInMagazine);
    }

    //=========================================

    private void Recharge(int parValue)
    {
      if (coroutineRecharge != null)
      {
        Debug.LogWarning("Перезарадка уже запущена");
        return;
      }

      if (parValue < 0)
      {
        Debug.LogError("Значение перезарядки не может быть меньше 0");
        return;
      }

      if (currentAmountAmmo == 0)
      {
        Debug.LogWarning("Текущее количество патронов равно 0, перезарядка невозможна");
        return;
      }

      if (currentAmountAmmoInMagazine >= _maxAmountAmmoInMagazine)
      {
        Debug.Log("Текущее количество патронов в магазине >= максимального значения");
        return;
      }

      coroutineRecharge = StartCoroutine(CoroutineRecharge(parValue));
    }

    private IEnumerator CoroutineRecharge(int parValue)
    {
      // Количество патронов до перезарядки
      int amountAmmoBefore = currentAmountAmmo;
      // Количество патронов в магазине до перезарядки
      int amountAmmoInMagazineBefore = currentAmountAmmoInMagazine;

      if (parValue + amountAmmoInMagazineBefore > _maxAmountAmmoInMagazine)
        parValue = _maxAmountAmmoInMagazine - amountAmmoInMagazineBefore;

      if (amountAmmoBefore - parValue <= 0)
        parValue = amountAmmoBefore;

      if (audioSource != null && _audioClipRecharge != null)
      {
        audioSource.Stop();
        audioSource.clip = _audioClipRecharge;
        audioSource.Play();
      }

      yield return new WaitForSeconds(_rechargeTime);

      currentAmountAmmo -= parValue;
      currentAmountAmmoInMagazine += parValue;

      // Количество патронов после перезарядки
      //int amountAmmoAfter = currentAmountAmmo - amountAmmoBefore;
      // Количество патронов в магазине после перезарядки
      //int amountAmmoInMagazineAfter = currentAmountAmmoInMagazine - amountAmmoInMagazineBefore;

      coroutineRecharge = null;
      audioSource.Stop();
    }

    private void PerformRaycast()
    {
      Camera camera = characterBehaviour.GetCameraWorld();

      Vector3 direction = _useSpread ? camera.transform.forward + CalculateSpread() : camera.transform.forward;
      Ray ray = new Ray(camera.transform.position, direction);

      if (Physics.Raycast(ray, out RaycastHit hitInfo, _distance, _layerMask))
      {
        var hitCollider = hitInfo.collider;

        if (hitCollider)
        {
          if (hitCollider.TryGetComponent(out Health parHealth))
          {
            if (hitInfo.point.y > 1.5f)
              parHealth.TakeDamage(_damage * 3);
            else
              parHealth.TakeDamage(_damage);
          }
        }

        SpawnEffectOnHit(hitInfo);
      }
    }

    private void PerformEffects()
    {
      if (_muzzleEffectPrefab != null)
      {
        foreach (var point in _startPoints)
        {
          var muzzleEffect = Instantiate(_muzzleEffectPrefab, point);

          Destroy(muzzleEffect.gameObject, _hitEffectDestroyDelay);
        }
      }

      if (audioSource != null && _audioClipFire != null)
      {
        audioSource.Stop();
        audioSource.clip = _audioClipFire;
        audioSource.Play();
      }
    }

    private void SpawnEffectOnHit(RaycastHit parRaycastHit)
    {
      if (_hitEffectPrefab == null)
        return;

      var hitEffect = Instantiate(_hitEffectPrefab, parRaycastHit.point, Quaternion.LookRotation(parRaycastHit.normal));

      Destroy(hitEffect.gameObject, _hitEffectDestroyDelay);
    }

    private Vector3 CalculateSpread()
    {
      return new Vector3
      {
        x = Random.Range(-_spreadFactor, _spreadFactor),
        y = Random.Range(-_spreadFactor, _spreadFactor),
        z = Random.Range(-_spreadFactor, _spreadFactor)
      };
    }

    private void ShootingStates()
    {
      switch (_typeShooting)
      {
        case TypesShooting.Automatic: break;
        case TypesShooting.Single: break;
        case TypesShooting.Series: break;
      }
    }

    public override void Activate()
    {
      gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
      gameObject.SetActive(false);
    }

    //=========================================
  }

  /// <summary>
  /// Типы стрельбы из оружия (Автоматическая, Одиночная, Сериями)
  /// </summary>
  public enum TypesShooting
  {
    /// <summary>
    /// Стрельба автоматическим огнем, при которой автомат выстреливает несколько пуль подряд при одном нажатии на спусковой крючок
    /// </summary>
    Automatic,
    /// <summary>
    /// Стрельба, при которой автомат выстреливает одну пулю за каждое нажатие на спусковой крючок
    /// </summary>
    Single,
    /// <summary>
    /// Стрельба, при которой автомат выстреливает несколько пуль (обычно 2-3) при каждом нажатии на спусковой крючок
    /// </summary>
    Series
  }
}