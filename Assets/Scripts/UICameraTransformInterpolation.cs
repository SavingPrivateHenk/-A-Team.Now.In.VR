using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraTransformInterpolation : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _interpolation = 0.5f;

    void Update()
    {
        gameObject.transform.position = Vector3.LerpUnclamped(gameObject.transform.position, _camera.transform.position, _interpolation);
        gameObject.transform.rotation = Quaternion.LerpUnclamped(gameObject.transform.rotation, _camera.transform.rotation, _interpolation);
    }
}