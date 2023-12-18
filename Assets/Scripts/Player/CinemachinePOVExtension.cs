using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField]
    private float xSpeed = 10f;
    [SerializeField]
    private float ySpeed = 10f;
    [SerializeField]
    private float clampAngle = 80f;
    [SerializeField]
    private float smoothTime = 0.1f;

    private Vector3 targetRot;
    private Transform camTransform;

    protected override void Awake()
    {
        base.Awake();
        targetRot = transform.localRotation.eulerAngles;
        camTransform = Camera.main.transform;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (!vcam.Follow || stage != CinemachineCore.Stage.Aim || GameInput.Instance == null) return;

        Vector2 deltaInput = GameInput.Instance.GetMouseMovement();
        targetRot.x += deltaInput.x * ySpeed * Time.deltaTime;
        targetRot.y += deltaInput.y * xSpeed * Time.deltaTime;
        targetRot.y = Mathf.Clamp(targetRot.y, -clampAngle, clampAngle);
        // Wrap at 360 degrees in order to prevent floating point imprecision
        targetRot.x %= 360f;

        Quaternion finalRot = Quaternion.Euler(-targetRot.y, targetRot.x, 0f);
        state.RawOrientation = Quaternion.Slerp(camTransform.localRotation, finalRot, smoothTime);
    }
}
