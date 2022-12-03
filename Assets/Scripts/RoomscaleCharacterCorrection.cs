using Unity.XR.CoreUtils;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(XROrigin))]
public class RoomscaleCharacterCorrection : MonoBehaviour
{
    private CharacterController m_characterController;
    private XROrigin m_xrOrigin;

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_xrOrigin = GetComponent<XROrigin>();
    }

    private void FixedUpdate()
    {
        m_characterController.height = m_xrOrigin.CameraInOriginSpaceHeight + 0.15f;

        var center = transform.InverseTransformPoint(m_xrOrigin.Camera.transform.position);
        center.y = m_characterController.height / 2 + m_characterController.skinWidth;

        m_characterController.center = center;

        // Force recalculation of physics to detect collisions.
        m_characterController.Move(new Vector3(0.001f, -0.001f, 0.001f));
        m_characterController.Move(new Vector3(-0.001f, 0.001f, -0.001f));
    }
}