using UnityEngine;

namespace Weapons
{
  public abstract class WeaponBehaviour : MonoBehaviour
  {
    /// <summary>
    /// Возвращает общий боекомплект
    /// </summary>
    public abstract int GetAmmunitionTotal();
    /// <summary>
    /// Возвращает текущие боеприпасы
    /// </summary>
    public abstract int GetAmmunitionCurrent();

    /// <summary>
    /// Возвращает урон оружия
    /// </summary>
    public abstract int GetWeaponDamage();
    /// <summary>
    /// Возвращает скорострельность оружия
    /// </summary>
    public abstract float GetRateOfFire();

    /// <summary>
    /// Возвращает тип стрельбы из оружия
    /// </summary>
    public abstract TypesShooting GetTypeShooting();

    //=========================================

    /// <summary>
    /// Выполнить атаку
    /// </summary>
    public abstract void PerformAttack();

    /// <summary>
    /// Выполнить перезарядку
    /// </summary>
    public abstract void PerformRecharge();

    public abstract void Activate();
    public abstract void Deactivate();

    //=========================================
  }
}