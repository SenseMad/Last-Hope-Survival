using UnityEngine;

public class CameraLook : MonoBehaviour
{
  [Header("НАСТРОЙКИ")]
  [SerializeField] private Vector2 _sensitivity = new Vector2(1, 1);
  [SerializeField] private Vector2 _angleRotation = new Vector2(-89, 89);

  //-----------------------------------------

  private CharacterBehaviour characterBehaviour;

  private Quaternion rotationCharacter;

  private Quaternion rotationCamera;

  //=========================================

  private void Awake()
  {
    characterBehaviour = CharacterBehaviour.Instance;

    rotationCharacter = characterBehaviour.transform.localRotation;

    rotationCamera = transform.localRotation;
  }

  private void LateUpdate()
  {
    Vector2 frameInput = characterBehaviour.IsCursorLocked() ? characterBehaviour.GetInputLook() : default;
    frameInput *= _sensitivity;

    Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);
    Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);

    rotationCamera *= rotationPitch;
    rotationCamera = Clamp(rotationCamera);
    rotationCharacter *= rotationYaw;

    Quaternion localRotation = transform.localRotation;
    localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime * 45.0f);
    localRotation = Clamp(localRotation);

    characterBehaviour.transform.rotation = Quaternion.Slerp(characterBehaviour.transform.rotation, rotationCharacter, Time.deltaTime * 45.0f);

    transform.localRotation = localRotation;
  }

  //=========================================

  private Quaternion Clamp(Quaternion rotation)
  {
    rotation.x /= rotation.w;
    rotation.y /= rotation.w;
    rotation.z /= rotation.w;
    rotation.w = 1.0f;

    float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

    pitch = Mathf.Clamp(pitch, _angleRotation.x, _angleRotation.y);
    rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

    return rotation;
  }

  //=========================================
}