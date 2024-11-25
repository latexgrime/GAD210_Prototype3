using System;
using System.Collections;
using GAD210.Leonardo.Player.Movement;
using UnityEngine;

public class WaterRespawnObject : MonoBehaviour
{
    private AudioSource portalAudioSource;
    
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private float returnTime;
    [SerializeField] private ParticleSystem respawnParticleSystem;
    [SerializeField] private ParticleSystem waterSplashParticleSystem;
    [SerializeField] private AudioClip respawnSFX;
    [SerializeField] private AudioClip waterSplashSFX;
    private bool wasMoved;

    private void Start()
    {
        portalAudioSource = respawnPoint.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider") || other.CompareTag("CanPickUp"))
        {
            if (!wasMoved)
            {
                wasMoved = true;
                StartCoroutine(MoveObject(other));
            }
        }
    }

    private IEnumerator MoveObject(Collider collidedObject)
    {
        if (collidedObject.CompareTag("PlayerCollider"))
        {
            // Play the SFX and the particle system.
            collidedObject.transform.parent.GetComponent<AudioSource>().PlayOneShot(waterSplashSFX);
            waterSplashParticleSystem.gameObject.transform.position = collidedObject.transform.position;
            waterSplashParticleSystem.Play();
            
            // Make the player "floaty"
            var parent = collidedObject.transform.parent;
            parent.GetComponent<PlayerMovement>().groundDrag = 20;
            
            // Teleport the player
            yield return new WaitForSeconds(returnTime);
            parent.gameObject.transform.position = respawnPoint.transform.position;
            parent.GetComponent<PlayerMovement>().groundDrag = 1;
        }
        else
        {
            collidedObject.GetComponent<AudioSource>().PlayOneShot(waterSplashSFX);
            waterSplashParticleSystem.gameObject.transform.position = collidedObject.transform.position;
            waterSplashParticleSystem.Play();
            yield return new WaitForSeconds(returnTime);
            collidedObject.transform.position = respawnPoint.transform.position;
        }
        respawnParticleSystem.Play();
        portalAudioSource.PlayOneShot(respawnSFX);
        wasMoved = false;
    }
}