using System;
using UnityEngine;

public class InvokeSceneChangeEvent : MonoBehaviour
{
    [SerializeField] private string m_sceneName;
    [SerializeField] private Vector3 m_playerReturnPosition;
    [SerializeField] private Quaternion m_playerReturnRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.name.Equals("XR Origin", StringComparison.OrdinalIgnoreCase)) return;
        FindObjectOfType<GameManager>().LoadScene(m_sceneName, m_playerReturnPosition, m_playerReturnRotation);
    }
}