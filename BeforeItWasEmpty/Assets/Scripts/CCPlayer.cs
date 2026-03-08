using System;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CCPlayer : MonoBehaviour
{
    #region VARIABLES
    [Header("Movement")]
    public float walkSpeed = 5;
    private CharacterController cc;
    private Vector2 moveInput;
    private float verticalVelocity; 
    private float gravity = -20f; 

    [Header("Camera")]
    public Transform cameraTransform;
    public float lookSensativity = 1f;
    private Vector2 lookInput;
    private float pitch; 

    [Header("Interactable")]
    private GameObject currrentTarget; 
    public Image reticleImage;
    public bool interactPressed;
    public Interactable currentInteractable;
    public static event Action<ObjectData> OnDescriptionRequested;

    [Header("Puzzle")]
    private int PuzzlePiece = 0;
    public TextMeshProUGUI PiecesCollectedText;
    public GameObject box;

    private bool inputDisabled = false;


    #endregion

    private void Awake()
    {
        cc = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        reticleImage = GameObject.Find("Reticle").GetComponent<Image>();
        reticleImage.color = new Color(r: 0, g: 0, b: 0, a: 7f); 
    }

    void Start()
    {

    }

    void Update()
    {
        if (!inputDisabled)
        {
            HandleLook();
            HandleMovement();
            CheckInteract();
        }
        
        HandleInteract();
    }

    #region HANDLES
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
        bool grounded = cc.isGrounded;
       
        if (grounded && verticalVelocity <= 0)
        {
            verticalVelocity = -2f;
        }

        float currentSpeed = walkSpeed;

        Vector3 move = transform.right * moveInput.x * currentSpeed + transform.forward * moveInput.y * currentSpeed;
        verticalVelocity += gravity * Time.deltaTime;
        Vector3 velocity = Vector3.up * verticalVelocity;
        cc.Move(motion: (move + velocity) * Time.deltaTime); 
    }
    #endregion 

    void CheckInteract()
    {
        if (reticleImage != null) reticleImage.color = new Color(0, 0, 0, .7f);
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            currentInteractable = hit.collider.GetComponent<Interactable>();
            if (currentInteractable != null && reticleImage != null)
            {
                reticleImage.color = Color.red;
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);

            }
            else
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);
            }
        }
    }
    void HandleInteract()
    {
        if (!interactPressed) return;
        interactPressed = false;
        if (currentInteractable == null) return;
        currentInteractable.Interact(this);
    }

    #region PLAYERINPUT
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) interactPressed = true;
    }
    #endregion

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.transform.tag == "PuzzlePiece")
    //    {
    //        PuzzlePiece++;
    //        PiecesCollectedText.text = "Collected: " + PuzzlePiece.ToString();
    //        Destroy(other.gameObject);

    //        if(PuzzlePiece == 5)
    //        {
    //            PiecesCollectedText.text = "Find the puzzle box and complete the Puzzle!";
    //            box.SetActive(true);
    //        }
    //    }
    //}

    public void PuzzlePieces()
    {
        if(interactPressed && gameObject.CompareTag("Interactable"))
        {
            PuzzlePiece++;
            PiecesCollectedText.text = "Collected: " + PuzzlePiece.ToString();


            if (PuzzlePiece == 5)
            {
                PiecesCollectedText.text = "Find the puzzle box and complete the Puzzle!";
                box.SetActive(true);
            }
        }
        

    }

    public void RequestDescription(ObjectData objectData)
    {
        OnDescriptionRequested?.Invoke(objectData);
    }

    public void DisableInput()
    {
        inputDisabled = true;
        moveInput = Vector2.zero;
        lookInput = Vector2.zero;
        reticleImage.color = new Color(0, 0, 0, 0f);
    }

    public void EnableInput()
    {
        inputDisabled = false;
    }
}
