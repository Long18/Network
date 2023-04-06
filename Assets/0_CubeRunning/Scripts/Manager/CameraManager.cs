using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    public InputReaderSO InputReader;
    public Camera MainCamera;
    public CinemachineFreeLook FreeLookVCam;
    public CinemachineImpulseSource ImpulseSource;
    private bool isRMBPressed; // is Right Mouse Button pressed

    [SerializeField, Range(.5f, 3f)] private float speedMultiplier = 1f;
    [SerializeField] private TransformAnchor cameraTransformAnchor = default;
    [SerializeField] private TransformAnchor protagonistTransformAnchor = default;

    [Header("Listening event")]
    [Tooltip("The CameraManager listens to this event, fired by protagonist GettingHit state, to shake camera")]
    [SerializeField]
    private VoidEventChannelSO camShakeEvent = default;

    private bool cameraMovementLock = false;

    private void OnEnable()
    {
        InputReader.CameraMoveEvent += OnCameraMove;
        InputReader.EnableMouseControlCameraEvent += OnEnableMouseControlCamera;
        InputReader.DisableMouseControlCameraEvent += OnDisableMouseControlCamera;

        protagonistTransformAnchor.OnAnchorProvied += SetupProtagonistVirtualCamera;
        camShakeEvent.OnEventRaised += ImpulseSource.GenerateImpulse;

        cameraTransformAnchor.Provide(MainCamera.transform);
    }

    private void OnDisable()
    {
        InputReader.CameraMoveEvent -= OnCameraMove;
        InputReader.EnableMouseControlCameraEvent -= OnEnableMouseControlCamera;
        InputReader.DisableMouseControlCameraEvent -= OnDisableMouseControlCamera;

        protagonistTransformAnchor.OnAnchorProvied -= SetupProtagonistVirtualCamera;
        camShakeEvent.OnEventRaised -= ImpulseSource.GenerateImpulse;

        cameraTransformAnchor.Unset();
    }

    private void Start()
    {
        //Setup the camera target if the protagonist is already available
        if (protagonistTransformAnchor.isSet) SetupProtagonistVirtualCamera();
    }

    private void OnEnableMouseControlCamera()
    {
        isRMBPressed = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(DisableMouseControlForFrame());
    }

    private IEnumerator DisableMouseControlForFrame()
    {
        cameraMovementLock = true;
        yield return new WaitForEndOfFrame();
        cameraMovementLock = false;
    }

    private void OnDisableMouseControlCamera()
    {
        isRMBPressed = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // when mouse control is disabled, the input needs to be cleared
        // or the last frame's input will 'stick' until the action is invoked again
        FreeLookVCam.m_XAxis.m_InputAxisValue = 0;
        FreeLookVCam.m_YAxis.m_InputAxisValue = 0;
    }

    private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
    {
        if (cameraMovementLock) return;

        if (isDeviceMouse && !isRMBPressed)
            return;

        float deviceMultiplier = isDeviceMouse ? 0.02f : Time.deltaTime;

        FreeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * deviceMultiplier * speedMultiplier;
        FreeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * deviceMultiplier * speedMultiplier;
    }

    /// <summary>
    /// Provides Cinemachine with its target, taken from the TransformAnchor SO containing a reference to the player's Transform component.
    /// This method is called every time the player is reinstantiated.
    /// </summary>
    public void SetupProtagonistVirtualCamera()
    {
        Transform target = protagonistTransformAnchor.Value;

        FreeLookVCam.Follow = target;
        FreeLookVCam.LookAt = target;
        FreeLookVCam.OnTargetObjectWarped(target, target.position - FreeLookVCam.transform.position - Vector3.forward);
    }
}