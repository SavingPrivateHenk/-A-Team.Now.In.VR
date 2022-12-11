using Unity.XR.CoreUtils;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public XROrigin XROrigin; 
    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = XROrigin.gameObject.AddComponent<AudioSource>();
        m_audioSource.playOnAwake = false;
        m_audioSource.loop = false;
        m_audioSource.volume = 0.2f;
    }

    public void PlayClip(AudioClip clip) => m_audioSource.PlayOneShot(clip);

    public float Volume
    {
        get => m_audioSource.volume;
        set => m_audioSource.volume = Mathf.Clamp01(value);
    }
}