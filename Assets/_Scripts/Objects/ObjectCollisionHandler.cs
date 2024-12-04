using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectCollisionHandler : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip collisionSFX;
    // The velocity needed to trigger sound. This is a workaround because even if the object is standing still, it still generates collisions.
    [SerializeField] private float collisionVelocityThreshold = 0.5f;

    private float defaultVolume;
    private float defaultPitch;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        defaultVolume = audioSource.volume;
        defaultPitch = audioSource.pitch;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude >= collisionVelocityThreshold)
        {
            PlayCollisionSFX();
        }
    }

    private void PlayCollisionSFX()
    {
        audioSource.pitch = Random.Range(defaultPitch - 0.1f, defaultPitch + 0.1f);
        audioSource.volume = Random.Range(defaultVolume - 0.2f, defaultVolume + 0.2f);
        audioSource.PlayOneShot(collisionSFX);
    }
}
