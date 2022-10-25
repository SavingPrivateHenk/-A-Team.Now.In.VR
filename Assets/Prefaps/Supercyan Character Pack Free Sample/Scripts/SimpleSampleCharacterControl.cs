using System.Collections.Generic;
using UnityEngine;

public class SimpleSampleCharacterControl : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnCollisionEnter(Collision collision)
    {
        StartWaving();
    }
    
    [ContextMenu(nameof(StartWaving))]
    private void StartWaving()
    {
        animator.SetBool("Wave", true);
    }
    
    [ContextMenu(nameof(StopWaving))]
    private void StopWaving()
    {
        animator.SetBool("Wave", false);
    }
    
    private void OnCollisionExit(Collision collision)
    {
        StopWaving();
    }
}
