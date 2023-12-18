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

    private Vector3 startingRot;

    protected override void Awake()
    {
        base.Awake();
        startingRot = transform.localRotation.eulerAngles;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (!vcam.Follow || stage != CinemachineCore.Stage.Aim) return;

        Vector2 deltaInput = GameInput.Instance.GetMouseMovement();
        startingRot.x += deltaInput.x * ySpeed * Time.deltaTime;
        startingRot.y += deltaInput.y * xSpeed * Time.deltaTime;
        startingRot.y = Mathf.Clamp(startingRot.y, -clampAngle, clampAngle);
        state.RawOrientation = Quaternion.Euler(-startingRot.y, startingRot.x, 0f);
    }
}
