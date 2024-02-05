using UnityEngine;

public abstract class CharacterBehaviour : SingletonInSceneNoInstance<CharacterBehaviour>
{
  /// <summary>
  /// Возвращает основную камеру игрового персонажа
  /// </summary>
  public abstract Camera GetCameraWorld();

  /// <summary>
  /// True, если персонаж бежит
  /// </summary>
  public abstract bool IsRunning();

  /// <summary>
  /// True, если игровой курсор заблокирован
  /// </summary>
  public abstract bool IsCursorLocked();

  /// <summary>
  /// Возвращает вводимые данные о перемещении
  /// </summary>
  public abstract Vector2 GetInputMovement();
  /// <summary>
  /// Возвращает поворот
  /// </summary>
  public abstract Vector2 GetInputLook();

  //=========================================

  #region UNITY

  protected virtual new void Awake()
  {
    base.Awake();
  }

  protected virtual void Start() { }

  protected virtual void Update() { }

  protected virtual void LateUpdate() { }

  #endregion

  //=========================================
}