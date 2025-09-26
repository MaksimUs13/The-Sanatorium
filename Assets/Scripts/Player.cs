using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] Slider sprintSlider;
    private CharacterController characterController;
    private float playerSpeed = 4.0f;
    private float playerSprintSpeed = 7.0f;
    private const float accelerationOfGravity = 9.81f;
    private float verticalVelocity = 0f;
    private bool isSprinting = false;
    private float sprintScaleAmount = 1;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        inputManager.OnSprint += (sender, args) => { Sprint(); };
        inputManager.OnStopSprint += (sender, args) => { StopSprint(); };
    }
    private void OnDisable()
    {
        inputManager.OnSprint -= (sender, args) => { Sprint(); };
        inputManager.OnStopSprint -= (sender, args) => { StopSprint(); };
    }
    private void Update()
    {
        Movement();
        if(isSprinting)
        {
            sprintScaleAmount = 0.6f * Time.deltaTime;
            sprintSlider.value -= sprintScaleAmount;
            if(sprintSlider.value <= 0f)
                StopSprint();
        }
        if(!isSprinting)
        {
            sprintScaleAmount = 0.1f * Time.deltaTime;
            sprintSlider.value += sprintScaleAmount;
        }
    }

    private void Movement()
    {
        bool isGrounded = characterController.isGrounded;
        Vector2 inputVector = inputManager.GetMovementVectorNormalized();
        Vector3 moveDir = transform.forward * inputVector.y + transform.right * inputVector.x;
        if (isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= accelerationOfGravity * Time.deltaTime;
        }
        moveDir.y = verticalVelocity;
        characterController.Move(moveDir * playerSpeed * Time.deltaTime);
    }
    private void Sprint()
    {
        isSprinting = true;
        if (isSprinting)
        {
            playerSpeed = playerSprintSpeed;
        }      
    }
    private void StopSprint()
    {
        isSprinting = false;
        if (!isSprinting)
            playerSpeed = 4.0f;
    }
}