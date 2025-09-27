using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Slider sprintSlider;

    private CharacterController characterController;
    private float playerSpeed = 4.0f;
    private float playerSprintSpeed = 7.0f;
    private const float accelerationOfGravity = 9.81f;
    private const float groundedVerticalVelocity = -0.5f;
    private float verticalVelocity = 0f;
    private bool isSprinting = false;

    // Константы для оптимизации
    private const float baseSpeed = 4.0f;
    private const float sprintDrainRate = 0.6f;
    private const float sprintRecoveryRate = 0.1f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // Упрощенная подписка на события
        inputManager.OnSprint += OnSprintEvent;
        inputManager.OnStopSprint += OnStopSprintEvent;
    }

    private void OnDisable()
    {
        inputManager.OnSprint -= OnSprintEvent;
        inputManager.OnStopSprint -= OnStopSprintEvent;
    }

    private void Update()
    {
        Movement();
        UpdateSprint();
    }

    private void Movement()
    {
        bool isGrounded = characterController.isGrounded;
        Vector2 inputVector = inputManager.GetMovementVectorNormalized();
        Vector3 moveDir = transform.forward * inputVector.y + transform.right * inputVector.x;

        verticalVelocity = isGrounded ? groundedVerticalVelocity :
            verticalVelocity - accelerationOfGravity * Time.deltaTime;

        moveDir.y = verticalVelocity;
        characterController.Move(moveDir * (playerSpeed * Time.deltaTime));
    }

    private void UpdateSprint()
    {
        float sprintDelta = (isSprinting ? -sprintDrainRate : sprintRecoveryRate) * Time.deltaTime;
        sprintSlider.value += sprintDelta;

        // Автоматическое отключение спринта при истощении
        if (isSprinting && sprintSlider.value <= 0f)
        {
            StopSprint();
        }
    }

    private void OnSprintEvent(object sender, System.EventArgs args)
    {
        Sprint();
    }

    private void OnStopSprintEvent(object sender, System.EventArgs args)
    {
        StopSprint();
    }

    private void Sprint()
    {
        // Проверяем, есть ли еще выносливость для спринта
        if (sprintSlider.value > 0f)
        {
            isSprinting = true;
            playerSpeed = playerSprintSpeed;
        }
    }

    private void StopSprint()
    {
        isSprinting = false;
        playerSpeed = baseSpeed;
    }
}