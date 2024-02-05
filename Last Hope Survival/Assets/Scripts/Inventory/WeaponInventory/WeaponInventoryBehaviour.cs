using UnityEngine;

using Weapons;

namespace Inventory
{
  public abstract class WeaponInventoryBehaviour : MonoBehaviour
  {
    public abstract WeaponBehaviour GetActiveWeapon();

    public abstract int GetLastIndex();
    public abstract int GetNextIndex();

    public abstract int GetActiveWeaponIndex();

    //=========================================

    public abstract void Initialize(int parIndexStartingWeapon = 0);

    public abstract WeaponBehaviour Equip(int parIndex);

    public abstract void Add(WeaponBehaviour parWeapon);
    public abstract void Remove(WeaponBehaviour parWeapon);
    public abstract void Replace(WeaponBehaviour parWeapon);

    //=========================================
  }
}