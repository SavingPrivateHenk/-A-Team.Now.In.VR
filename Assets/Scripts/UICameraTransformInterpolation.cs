using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraTransformInterpolation : MonoBehaviour
{
    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private float m_interpolation = 0.5f;

    void Update()
    {
        gameObject.transform.position = Vector3.LerpUnclamped(gameObject.transform.position, m_camera.transform.position, m_interpolation);
        gameObject.transform.rotation = Quaternion.LerpUnclamped(gameObject.transform.rotation, m_camera.transform.rotation, m_interpolation);
    }
}