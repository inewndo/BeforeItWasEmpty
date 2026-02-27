using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CCPlayer : MonoBehaviour
{
    #region VARIABLES

    [Header("Movement")]
    private CharacterController cc;
    public float walkSpeed = 3;
    private Vector2 moveInput;


    [Header("Camera")]
    private Vector2 lookInput;
    public Transform cameraTransform;
    public float lookSensativity;
    private float pitch;

    [Header("Interactables")]
    public Image reticleImage;

    #endregion

    private void Awake()
    {
        cc = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        reticleImage = GameObject.Find("Reticle").GetComponent<Image>();
        reticleImage.color = new Color(r: 0, g: 0, b: 0, a: 7f);
    }

    private void HandleLook()
    {
        float yaw = lookInput.x * lookSensativity;
        float pitchDelta = lookInput.y * lookSensativity;
        transform.Rotate(eulers: Vector3.up * yaw);
        pitch -= pitchDelta;
        pitch = Mathf.Clamp(pitch, min: -90, max: 90);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    private void HandleMovement()
    {
        float currentSpeed = walkSpeed;
        Vector3 move = transform.right * moveInput.x * currentSpeed;
        cc.Move(motion: (move) * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

}
