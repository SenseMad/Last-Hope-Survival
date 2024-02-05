using UnityEngine;
using UnityEngine.InputSystem;

using Weapons;
using Inventory;

public sealed class Character : CharacterBehaviour
{
  [SerializeField] private Camera _cameraWorld;

  //-----------------------------------------

  private MovementBehaviour movementBehaviour;

  private WeaponInventory weaponInventory;

  private WeaponBehaviour equippedWeapon;

  /// <summary>
  /// True, если персонаж бежит
  /// </summary>
  private bool isRunning;
  /// <summary>
  /// True, если игрок прыгает
  /// </summary>
  private bool isJumping;

  /// <summary>
  /// True, если стреляем
  /// </summary>
  private bool holdButtonFire;

  /// <summary>
  /// True, если игровой курсор заблокирован
  /// </summary>
  private bool cursorLocked;

  /// <summary>
  /// Поворот по оси
  /// </summary>
  private Vector2 axisLook;

  //=========================================

  public InputHandler InputHandler { get; private set; }

  public Health Health { get; private set; }

  //-----------------------------------------

  public override Camera GetCameraWorld() => _cameraWorld;

  public override bool IsRunning() => isRunning;

  public override bool IsCursorLocked() => cursorLocked;

  public override Vector2 GetInputMovement() => InputHandler.GetInputMovement();
  public override Vector2 GetInputLook() => InputHandler.GetInputLook();

  //=========================================

  protected override void Awake()
  {
    base.Awake();

    cursorLocked = true;
    UpdateCursorState();

    InputHandler = InputHandler.Instance;

    Health = GetComponent<Health>();

    movementBehaviour = GetComponent<MovementBehaviour>();

    weaponInventory = GetComponent<WeaponInventory>();
  }

  protected override void Start()
  {
    weaponInventory.Initialize();

    equippedWeapon = weaponInventory.GetActiveWeapon();
  }

  protected override void Update()
  {
    if (holdButtonFire)
      equippedWeapon.PerformAttack();
  }

  private void OnEnable()
  {
    InputHandler.IA_Player.Player.Jump.performed += OnTryJump;

    InputHandler.IA_Player.Player.Run.started += OnTryRun;
    InputHandler.IA_Player.Player.Run.canceled += OnTryRun;

    InputHandler.IA_Player.Player.Fire.started += OnTryFire;
    InputHandler.IA_Player.Player.Fire.performed += OnTryFire;
    InputHandler.IA_Player.Player.Fire.canceled += OnTryFire;

    InputHandler.IA_Player.Player.Reload.started += OnTryReload;

    InputHandler.IA_Player.Player.InventoryNextWheel.performed += OnTryInventoryNextWheel;
  }

  private void OnDisable()
  {
    InputHandler.IA_Player.Player.Jump.performed -= OnTryJump;

    InputHandler.IA_Player.Player.Run.started -= OnTryRun;
    InputHandler.IA_Player.Player.Run.canceled -= OnTryRun;

    InputHandler.IA_Player.Player.Fire.started -= OnTryFire;
    InputHandler.IA_Player.Player.Fire.performed -= OnTryFire;
    InputHandler.IA_Player.Player.Fire.canceled -= OnTryFire;

    InputHandler.IA_Player.Player.Reload.started -= OnTryReload;

    InputHandler.IA_Player.Player.InventoryNextWheel.performed -= OnTryInventoryNextWheel;
  }

  //=========================================

  /// <summary>
  /// Обновить состояние курсора
  /// </summary>
  private void UpdateCursorState()
  {
    Cursor.visible = !cursorLocked;
    Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
  }

  private void RefreshWeaponSetup(int parIndex)
  {
    weaponInventory.Equip(parIndex);
    equippedWeapon = weaponInventory.GetActiveWeapon();
  }

  //=========================================

  private void OnTryJump(InputAction.CallbackContext context)
  {
    if (!cursorLocked)
      return;

    switch (context.phase)
    {
      case InputActionPhase.Performed:
        movementBehaviour.Jump();
        break;
    }
  }

  private void OnTryRun(InputAction.CallbackContext context)
  {
    if (!cursorLocked)
      return;

    switch (context.phase)
    {
      case InputActionPhase.Started:
        isRunning = true;
        break;
      case InputActionPhase.Canceled:
        isRunning = false;
        break;
    }
  }

  private void OnTryFire(InputAction.CallbackContext context)
  {
    if (!cursorLocked)
      return;

    if (equippedWeapon == null)
      return;

    switch (context.phase)
    {
      case InputActionPhase.Started:
        holdButtonFire = true;
        break;
      case InputActionPhase.Performed:
        if (equippedWeapon.GetTypeShooting() != TypesShooting.Single)
          break;

        holdButtonFire = false;
        equippedWeapon.PerformAttack();
        break;
      case InputActionPhase.Canceled:
        holdButtonFire = false;
        break;
    }
  }

  private void OnTryReload(InputAction.CallbackContext context)
  {
    if (!cursorLocked)
      return;

    if (equippedWeapon == null)
      return;

    equippedWeapon.PerformRecharge();
  }

  private void OnTryInventoryNextWheel(InputAction.CallbackContext context)
  {
    if (!cursorLocked)
      return;

    if (weaponInventory == null)
      return;

    switch (context.phase)
    {
      case InputActionPhase.Performed:
        float scrollValue = context.valueType.IsEquivalentTo(typeof(Vector2)) ? Mathf.Sign(context.ReadValue<Vector2>().y) : 1.0f;

        int indexNext = scrollValue > 0 ? weaponInventory.GetNextIndex() : weaponInventory.GetLastIndex();
        int currentIndex = weaponInventory.GetActiveWeaponIndex();

        if (currentIndex != indexNext)
        {
          RefreshWeaponSetup(indexNext);
        }
        break;
    }
  }

  //=========================================
}