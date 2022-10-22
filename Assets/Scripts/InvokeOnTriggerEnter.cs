using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InvokeOnTriggerEnter : MonoBehaviour
{
    [SerializeField]
    private GameObject _targetGameObject;
    [SerializeField, Tooltip("Duration in milliseconds.")]
    private float _delay;
    [SerializeField]
    private UnityEvent _event;

    private void OnTriggerEnter(Collider other)
    {
        if (_targetGameObject == null || IsTargetGameObject(other.gameObject))
        {
            StopAllCoroutines();
            StartCoroutine(InvokeEvent(_event, _delay / 1000f));
        }
    }

    private IEnumerator InvokeEvent(UnityEvent unityEvent, float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }
        unityEvent.Invoke();
    }

    private bool IsTargetGameObject(GameObject gameObject)
    {
        return gameObject == _targetGameObject;
    }
}