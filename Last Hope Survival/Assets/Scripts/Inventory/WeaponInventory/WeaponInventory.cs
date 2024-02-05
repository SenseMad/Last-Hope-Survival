using System.Collections.Generic;
using UnityEngine;

using Weapons;

namespace Inventory
{
  public sealed class WeaponInventory : WeaponInventoryBehaviour
  {
    [SerializeField, Min(1)] private int _maxAmountStoredWeapons = 2;

    //=========================================

    private List<WeaponBehaviour> listWeapons = new List<WeaponBehaviour>();

    private WeaponBehaviour activeWeapon;

    private int activeWeaponIndex = -1;

    //=========================================

    public override WeaponBehaviour GetActiveWeapon() => activeWeapon;

    public override int GetActiveWeaponIndex() => activeWeaponIndex;

    public override int GetLastIndex()
    {
      int newIndex = activeWeaponIndex - 1;
      if (newIndex < 0)
        newIndex = listWeapons.Count - 1;

      return newIndex;
    }

    public override int GetNextIndex()
    {
      int newIndex = activeWeaponIndex + 1;
      if (newIndex > listWeapons.Count - 1)
        newIndex = 0;

      return newIndex;
    }

    //=========================================

    public override void Initialize(int parIndexStartingWeapon = 0)
    {
      WeaponBehaviour[] weapons = GetComponentsInChildren<WeaponBehaviour>(true);

      foreach (var weapon in weapons)
      {
        weapon.Deactivate();
        listWeapons.Add(weapon);
      }

      Equip(parIndexStartingWeapon);
    }

    public override WeaponBehaviour Equip(int parIndex)
    {
      if (listWeapons == null)
        return activeWeapon;

      if (parIndex > listWeapons.Count - 1 || parIndex < 0)
        return activeWeapon;

      if (parIndex == activeWeaponIndex)
        return activeWeapon;

      if (activeWeapon != null)
        activeWeapon.Deactivate();

      activeWeaponIndex = parIndex;
      activeWeapon = listWeapons[activeWeaponIndex];
      activeWeapon.Activate();

      return activeWeapon;
    }

    public override void Add(WeaponBehaviour parWeapon)
    {
      if (listWeapons.Count > _maxAmountStoredWeapons)
      {
        Replace(parWeapon);
        return;
      }

      listWeapons.Add(parWeapon);
      Equip(listWeapons.Count - 1);
    }

    public override void Remove(WeaponBehaviour parWeapon)
    {
      listWeapons.Remove(parWeapon);
      Destroy(parWeapon.gameObject);
    }

    public override void Replace(WeaponBehaviour parWeapon)
    {
      WeaponBehaviour oldWeapon = listWeapons[activeWeaponIndex];
      oldWeapon.Deactivate();

      int oldWeaponIndex = activeWeaponIndex;
      activeWeaponIndex = -1;

      listWeapons[oldWeaponIndex] = parWeapon;
      Equip(oldWeaponIndex);

      Destroy(oldWeapon.gameObject);
    }

    //=========================================
  }
}