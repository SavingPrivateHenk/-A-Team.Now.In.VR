using System.Collections.Generic;
using UnityEngine;

public class SimpleSampleCharacterControl : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void StartWaving()
    {
        animator.SetBool("Wave", true);
    }
    
    public void StopWaving()
    {
        animator.SetBool("Wave", false);
    }
}
