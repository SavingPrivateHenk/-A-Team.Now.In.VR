using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InvokeOnTriggerExit : MonoBehaviour
{
    [SerializeField]
    private GameObject m_source;
    [SerializeField, Tooltip("Delay (ms).")]
    private float m_delay;
    [SerializeField]
    private UnityEvent m_event;

    private void OnTriggerExit(Collider other)
    {
        if (m_source == null || other.gameObject == m_source)
        {
            if (m_delay > 0)
            {
                StopAllCoroutines();
                StartCoroutine(m_event.Invoke(m_delay));
            }
            else
            {
                m_event.Invoke();
            }
        }
    }
}