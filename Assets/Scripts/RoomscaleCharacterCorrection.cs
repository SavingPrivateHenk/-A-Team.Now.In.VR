using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(XROrigin))]
public class RoomscaleCharacterCorrection : MonoBehaviour
{
    private CharacterController _characterController;
    private XROrigin _xrOrigin;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _xrOrigin = GetComponent<XROrigin>();
    }

    private void FixedUpdate()
    {
        _characterController.height = _xrOrigin.CameraInOriginSpaceHeight + 0.15f;

        var center = transform.InverseTransformPoint(_xrOrigin.Camera.transform.position);
        center.y = _characterController.height / 2 + _characterController.skinWidth;

        _characterController.center = center;
    }
}