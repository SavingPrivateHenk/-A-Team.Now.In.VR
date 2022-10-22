using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InvokeOnTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _targetGameObject;
    [SerializeField, Tooltip("Duration in milliseconds.")]
    private float _delay;
    [SerializeField]
    private UnityEvent _onTriggerEnter;
    [SerializeField]
    private UnityEvent _onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (_targetGameObject == null || IsTargetGameObject(other.gameObject))
        {
            InvokeEventOnTrigger(_onTriggerEnter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_targetGameObject == null || IsTargetGameObject(other.gameObject))
        {
            InvokeEventOnTrigger(_onTriggerExit);
        }
    }

    private void InvokeEventOnTrigger(UnityEvent unityEvent)
    {
        StopAllCoroutines();
        StartCoroutine(InvokeEvent(unityEvent, _delay / 1000f));
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