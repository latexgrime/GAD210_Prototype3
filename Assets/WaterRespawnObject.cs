using System;
using System.Collections;
using UnityEngine;

public class WaterRespawnObject : MonoBehaviour
{
    private AudioSource portalAudioSource;
    
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private float returnTime;
    [SerializeField] private ParticleSystem respawnParticleSystem;
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
            collidedObject.transform.parent.GetComponent<AudioSource>().PlayOneShot(waterSplashSFX);
            yield return new WaitForSeconds(returnTime);
            collidedObject.transform.parent.gameObject.transform.position = respawnPoint.transform.position;
        }
        else
        {
            collidedObject.GetComponent<AudioSource>().PlayOneShot(waterSplashSFX);
            yield return new WaitForSeconds(returnTime);
            collidedObject.transform.position = respawnPoint.transform.position;
        }
        respawnParticleSystem.Play();
        portalAudioSource.PlayOneShot(respawnSFX);
        wasMoved = false;
    }
}