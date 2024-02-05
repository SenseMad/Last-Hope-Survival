using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class Movement : MovementBehaviour
{
  [Header("СКОРОСТЬ")]
  [SerializeField, Tooltip("Скорость игрока при ходьбе")]
  private float _speedWalking = 4.0f;
  [SerializeField, Tooltip("Скорость игрока при беге")]
  private float _speedRunning = 7.0f;
  [SerializeField, Tooltip("Скорость игрока во время прицеливания")]
  private float _speedAiming = 3.0f;

  [Header("УСКОРЕНИЕ")]
  [SerializeField, Tooltip("Насколько быстро увеличивается скорость персонажа")]
  private float _acceleration = 9.0f;
  [SerializeField, Tooltip("Насколько быстро уменьшается скорость персонажа")]
  private float _deceleration = 11.0f;
  [SerializeField, Tooltip("Значение ускорения, используемое, когда персонаж находится в воздухе. Это означает либо прыжок, либо падение")]
  private float _accelerationInAir = 1.7f;

  [Header("МНОЖИТЕЛИ")]
  [SerializeField, Tooltip("Значение, на которое умножается скорость ходьбы, когда персонаж движется боком"), Range(0.0f, 1.0f)]
  private float _walkingMultiplierSideways = 0.9f;
  [SerializeField, Tooltip("Значение, на которое умножается скорость ходьбы, когда персонаж движется назад"), Range(0.0f, 1.0f)]
  private float _walkingMultiplierBackwards = 0.9f;

  [Header("ВОЗДУХ")]
  [SerializeField, Tooltip("Насколько игрок может контролировать изменение направления, пока персонаж находится в воздухе"), Range(0.0f, 1.0f)]
  private float _airControl = 0.15f;
  [SerializeField, Tooltip("Значение силы тяжести персонажа")]
  private float _gravity = 35.0f;
  [SerializeField, Tooltip("Сила прыжка")]
  private float _jumpForce = 1.5f;

  //-----------------------------------------

  private CharacterController characterController;

  private CharacterBehaviour characterBehaviour;

  /// <summary>
  /// Скорость
  /// </summary>
  private Vector3 velocity;

  /// <summary>
  /// True, если персонаж находится на земле
  /// </summary>
  private bool isGrounded;
  /// <summary>
  /// Был ли персонаж стоящим на земле в последнем кадре
  /// </summary>
  private bool wasGrounded;

  /// <summary>
  /// True, если персонаж прыгает
  /// </summary>
  private bool isJumping;

  /// <summary>
  /// Сохраняет значение Time.time, когда персонаж в последний раз прыгал
  /// </summary>
  private float lastJumpTime;

  //=========================================

  public override float GetMultiplierBackwards() => _walkingMultiplierBackwards;

  public override float GetMultiplierSideways() => _walkingMultiplierSideways;

  public override Vector3 GetVelocity() => velocity;

  public override bool IsGrounded() => characterController.isGrounded;
  public override bool WasGrounded() => wasGrounded;

  public override bool IsJumping() => isJumping;

  //=========================================

  protected override void Awake()
  {
    characterController = GetComponent<CharacterController>();

    characterBehaviour = CharacterBehaviour.Instance;
  }

  protected override void Update()
  {
    isGrounded = IsGrounded();

    if (isGrounded && !wasGrounded)
    {
      isJumping = false;
      lastJumpTime = 0.0f;
    }
    else if (wasGrounded && !isGrounded)
      lastJumpTime = Time.time;

    MoveCharacter();

    wasGrounded = isGrounded;
  }

  //=========================================

  /// <summary>
  /// Перемещение персонажа
  /// </summary>
  private void MoveCharacter()
  {
    // Получить ввод данных о движении
    Vector2 frameInput = Vector3.ClampMagnitude(characterBehaviour.GetInputMovement(), 1.0f);
    // Вычислить направление в локальном пространстве, используя вводимые игроком данные
    var desiredDirection = new Vector3(frameInput.x, 0.0f, frameInput.y);

    // Расчет скорости бега
    if (characterBehaviour.IsRunning())
    {
      desiredDirection *= _speedRunning;
    }
    else
    {
      desiredDirection *= _speedWalking;

      desiredDirection.x *= _walkingMultiplierSideways;

      desiredDirection.z *= frameInput.y > 0 ? 1 : _walkingMultiplierBackwards;
    }

    // Расчет мировой космической скорости
    desiredDirection = transform.TransformDirection(desiredDirection);

    velocity = Vector3.Lerp(velocity, new Vector3(desiredDirection.x, velocity.y, desiredDirection.z), Time.deltaTime * (desiredDirection.sqrMagnitude > 0.0f ? _acceleration : _deceleration));
    // Применить гравитацию
    if (!isGrounded)
    {
      // Избавиться от любой восходящей скорости
      if (wasGrounded && !isJumping)
        velocity.y = 0.0f;

      //velocity += desiredDirection * (_accelerationInAir * _airControl * Time.deltaTime);
      // Гравитация
      velocity.y -= _gravity * Time.deltaTime;
    }

    // Приложенная скорость
    Vector3 applied = velocity * Time.deltaTime;
    // Придерживаться наземных сил. Помогает заставить персонажа спускаться по склонам, не плавая
    if (characterController.isGrounded && !isJumping)
      applied.y -= 0.03f;

    characterController.Move(applied);
  }

  //=========================================

  public override void Jump()
  {
    if (!isGrounded)
      return;

    isJumping = true;
    velocity = new Vector3(velocity.x, Mathf.Sqrt(2.0f * _jumpForce * _gravity), velocity.z);

    lastJumpTime = Time.time;
  }

  //=========================================
}