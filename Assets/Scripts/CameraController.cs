using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    private float mouseSensitivity = 30f;
    private float maxYAngle = 80f;
    private float rotationX = 0f;

    private void Start()
    {
        BlockCursor();
    }
    private void Update()
    {
        DeltaLook();
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
}
