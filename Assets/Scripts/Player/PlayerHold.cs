using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHold : MonoBehaviour
{
    [Header("General Hold Settings")]
    [SerializeField] private Transform holdTransform;
    [SerializeField] private float holdDistance = 0.5f;

    [Header("Physics Hold Settings")]
    [SerializeField] private float positionSpring = 100f;
    [SerializeField] private float positionDamper = 10f;
    [SerializeField] private float maximumForce = 3.402823e+38f;

    private Camera mainCam;
    private GameObject anchorObject;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 holdPosition = ray.GetPoint(holdDistance);
        holdTransform.position = holdPosition;
    }

    public void CreateAnchorPoint(GameObject heldObject)
    {
        anchorObject = new GameObject("Anchor Point");

        Rigidbody newRb = anchorObject.AddComponent<Rigidbody>();
        newRb.isKinematic = true;

        anchorObject.transform.parent = holdTransform;
        anchorObject.transform.position = holdTransform.position;
        heldObject.transform.position = anchorObject.transform.position;

        ConfigurableJoint configurableJoint = anchorObject.AddComponent<ConfigurableJoint>();
        configurableJoint.connectedBody = heldObject.GetComponent<Rigidbody>();

        configurableJoint.xDrive = CreateJointDrive();
        configurableJoint.yDrive = CreateJointDrive();
        configurableJoint.zDrive = CreateJointDrive();

        configurableJoint.slerpDrive = CreateJointDrive();
    }

    public void DestroyAnchorPoint()
    {
        Destroy(anchorObject);
    }

    private JointDrive CreateJointDrive()
    {
        JointDrive jointDrive = new JointDrive
        {
            positionSpring = positionSpring,
            positionDamper = positionDamper,
            maximumForce = maximumForce,
        };
        return jointDrive;
    }
}
