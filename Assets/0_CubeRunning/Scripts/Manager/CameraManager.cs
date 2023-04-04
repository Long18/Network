using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReader;
    public Camera mainCamera;
    public CinemachineFreeLook freeLookVCam;
    [SerializeField, Range(1f, 7f)] private float speed = default;
    private bool isRMBPressed;

    public void SetupProtagonistVirtualCamera(Transform target)
    {
        freeLookVCam.Follow = target;
        freeLookVCam.LookAt = target;
    }

    private void OnEnable()
    {
        inputReader.cameraMoveEvent += OnCameraMove;
        inputReader.enableMouseControlCameraEvent += OnEnableMouseControlCamera;
        inputReader.disableMouseControlCameraEvent += OnDisableMouseControlCamera;
    }

    private void OnDisable()
    {
        inputReader.cameraMoveEvent -= OnCameraMove;
        inputReader.enableMouseControlCameraEvent -= OnEnableMouseControlCamera;
        inputReader.disableMouseControlCameraEvent -= OnDisableMouseControlCamera;
    }

    private void OnEnableMouseControlCamera()
    {
        isRMBPressed = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisableMouseControlCamera()
    {
        isRMBPressed = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // when mouse control is disabled, the input needs to be cleared
        // or the last frame's input will 'stick' until the action is invoked again
        freeLookVCam.m_XAxis.m_InputAxisValue = 0;
        freeLookVCam.m_YAxis.m_InputAxisValue = 0;
    }

    private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
    {
        if (isDeviceMouse && !isRMBPressed)
            return;

        freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * speed;
        freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * speed;
    }
}