using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
  [SerializeField] private int _maxHealth = 100;

  //=========================================

  private int currentHealth;

  //=========================================

  public UnityEvent<int> OnAddHealth { get; } = new UnityEvent<int>();

  public UnityEvent<int> OnTakeHealth { get; } = new UnityEvent<int>();
  
  public UnityEvent OnInstantlyKill { get; } = new UnityEvent();

  //=========================================

  private void Start()
  {
    currentHealth = _maxHealth;
  }

  //=========================================

  public void AddHealth(int parHealth)
  {
    int healthBefore = currentHealth;
    currentHealth += parHealth;

    if (currentHealth > _maxHealth)
      currentHealth = _maxHealth;

    int healthAmount = currentHealth - healthBefore;
    if (healthAmount > 0)
      OnAddHealth?.Invoke(healthAmount);
  }

  public void TakeDamage(int parHealth)
  {
    int healthBefore = currentHealth;
    currentHealth -= parHealth;

    if (currentHealth < 0)
      currentHealth = 0;

    int damageAmount = healthBefore - currentHealth;
    if (damageAmount > 0)
      OnTakeHealth?.Invoke(damageAmount);

    if (currentHealth <= 0)
      OnInstantlyKill?.Invoke();
  }

  public void InstantlyKill()
  {
    currentHealth = 0;

    OnTakeHealth?.Invoke(_maxHealth);

    OnInstantlyKill?.Invoke();
  }

  //=========================================
}