using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    private float mouseSensitivity = 30f;
    private float maxYAngle = 80f;
    private float rotationX = 0f;

    [Header("Shake Settings")]
    public float shakeAmount = 0.13f;
    public float shakeSpeed = 20f;
    public float smoothness = 2f;

    private Vector3 originalPosition;
    private Vector3 targetShakePosition;
    private Vector3 currentVelocity;
    private bool isShaking = false;

    private void Start()
    {
        BlockCursor();
    }
    private void Update()
    {
        DeltaLook();
        if (isShaking)
        {
            // Обновление целевой позиции тряски
            float x = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            float y = Mathf.Cos(Time.time * shakeSpeed * 0.8f) * shakeAmount * 0.5f;
            targetShakePosition = originalPosition + new Vector3(x, y, 0);
        }
        else
        {
            targetShakePosition = originalPosition;
        }

        // Плавное перемещение к целевой позиции
        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetShakePosition,
            ref currentVelocity,
            smoothness * Time.deltaTime
        );
    }
    public void DeltaLook()
    {
        Vector2 inputLook = inputManager.GetLookVector();
        float mouseX = inputLook.x * mouseSensitivity * Time.deltaTime;
        float mouseY = inputLook.y * mouseSensitivity * Time.deltaTime;
        if (transform.parent == null)
        {
            Debug.LogError("Камера должна иметь родительский объект!");
            return;
        }
        transform.parent.Rotate(Vector3.up * mouseX);
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    public void BlockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartShake()
    {
        isShaking = true;
    }

    public void StopShake()
    {
        isShaking = false;
    }
}
