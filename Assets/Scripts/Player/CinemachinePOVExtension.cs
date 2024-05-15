using Cinemachine;
using UnityEngine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float sensitivity = 10f;
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private Vector3 startRot = new Vector3(-90f, 0f, 0f);

    private Vector3 targetRot;
    private Transform camTransform;

    protected override void Awake()
    {
        base.Awake();
        camTransform = Camera.main.transform;
        targetRot = startRot;
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {
        if (!vcam.Follow || stage != CinemachineCore.Stage.Aim || GameInput.Instance == null)
            return;

        Vector2 deltaInput = GameInput.Instance.GetMouseMovement();
        targetRot.x += deltaInput.x * sensitivity * Time.deltaTime;
        targetRot.y += deltaInput.y * sensitivity * Time.deltaTime;
        targetRot.y = Mathf.Clamp(targetRot.y, -clampAngle, clampAngle);
        // Wrap at 360 degrees in order to prevent floating point imprecision
        targetRot.x %= 360f;

        Quaternion finalRot = Quaternion.Euler(-targetRot.y, targetRot.x, 0f);
        state.RawOrientation = Quaternion.Slerp(camTransform.localRotation, finalRot, smoothTime);
    }

    public void SetSensitivity(float sensitivity)
    {
        this.sensitivity = sensitivity;
    }
}
