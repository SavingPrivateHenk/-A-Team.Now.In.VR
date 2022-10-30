using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InvokeOnTriggerExit : MonoBehaviour
{
    [SerializeField]
    private GameObject _source;
    [SerializeField, Tooltip("Delay (ms).")]
    private float _delay;
    [SerializeField]
    private UnityEvent _event;

    private void OnTriggerExit(Collider other)
    {
        if (_source == null || other.gameObject == _source)
        {
            if (_delay > 0)
            {
                StopAllCoroutines();
                StartCoroutine(_event.Invoke(_delay));
            }
            else
            {
                _event.Invoke();
            }
        }
    }
}