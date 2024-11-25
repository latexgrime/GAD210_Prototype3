using System;
using System.Collections;
using UnityEngine;

public class WaterRespawnObject : MonoBehaviour
{
    private AudioSource audioSource;
    
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private float returnTime;
    [SerializeField] private ParticleSystem respawnParticleSystem;
    [SerializeField] private AudioClip respawnSFX;
    private bool wasMoved;

    private void Start()
    {
        audioSource = respawnPoint.GetComponent<AudioSource>();
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
        yield return new WaitForSeconds(returnTime);
        if (collidedObject.CompareTag("PlayerCollider"))
        {
            collidedObject.transform.parent.gameObject.transform.position = respawnPoint.transform.position;
        }
        else
        {
            collidedObject.transform.position = respawnPoint.transform.position;
        }
        respawnParticleSystem.Play();
        audioSource.PlayOneShot(respawnSFX);
        wasMoved = false;
    }
}