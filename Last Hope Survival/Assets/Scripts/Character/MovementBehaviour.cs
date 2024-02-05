using UnityEngine;

public abstract class MovementBehaviour : MonoBehaviour
{
  /// <summary>
  /// Возвращает значение множителя в боковом направлении
  /// </summary>
  public abstract float GetMultiplierSideways();
  /// <summary>
  /// Возвращает значение множителя в обратном направлении
  /// </summary>
  public abstract float GetMultiplierBackwards();

  /// <summary>
  /// Возвращает текущую скорость персонажа
  /// </summary>
  public abstract Vector3 GetVelocity();
  /// <summary>
  /// True, если персонаж заземлен
  /// </summary>
  public abstract bool IsGrounded();
  /// <summary>
  /// Возвращает значение isGrounded последнего кадра
  /// </summary>
  public abstract bool WasGrounded();

  /// <summary>
  /// True, если персонаж прыгает
  /// </summary>
  public abstract bool IsJumping();

  //=========================================

  #region UNITY

  protected virtual void Awake() { }

  protected virtual void Start() { }

  protected virtual void Update() { }

  protected virtual void FixedUpdate() { }

  protected virtual void LateUpdate() { }

  #endregion

  //=========================================

  /// <summary>
  /// Вызов этого параметра заставит персонажа подпрыгнуть
  /// </summary>
  public abstract void Jump();

  //=========================================
}