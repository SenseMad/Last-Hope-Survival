using UnityEngine;

public class InputHandler : SingletonInGame<InputHandler>
{
  public IA_Player IA_Player { get; private set; }

  //=========================================

  private new void Awake()
  {
    IA_Player = new IA_Player();
  }

  private void OnEnable()
  {
    IA_Player.Enable();
  }

  private void OnDisable()
  {
    IA_Player.Disable();
  }

  //=========================================

  /// <summary>
  /// True, если можно использовать ввод данных
  /// </summary>
  public bool CanProcessInput()
  {
    return Cursor.lockState == CursorLockMode.Locked;
  }

  //=========================================

  /// <summary>
  /// Получить вводимые данные о перемещении
  /// </summary>
  public Vector2 GetInputMovement()
  {
    return CanProcessInput() ? IA_Player.Player.Move.ReadValue<Vector2>() : default;
  }

  /// <summary>
  /// Получить поворот
  /// </summary>
  public Vector2 GetInputLook()
  {
    return CanProcessInput() ? IA_Player.Player.Look.ReadValue<Vector2>() : default;
  }

  //=========================================
}