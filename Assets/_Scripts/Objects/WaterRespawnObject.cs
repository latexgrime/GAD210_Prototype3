using System.Collections;
using GAD210.Leonardo.Player.Movement;
using UnityEngine;

public class WaterRespawnObject : MonoBehaviour
{
    private AudioSource _portalAudioSource;
    [SerializeField] private Transform[] respawnPositions;
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private float respawnTime;
    [SerializeField] private ParticleSystem respawnParticleSystem;
    [SerializeField] private ParticleSystem waterSplashParticleSystem;
    [SerializeField] private AudioClip respawnSfx;
    [SerializeField] private AudioClip waterSplashSfx;
    private bool _wasMoved;

    private void Start()
    {
        if (respawnPoint.TryGetComponent(out AudioSource audioSource))
        {
            _portalAudioSource = audioSource;
        }
        if (respawnPositions.Length > 0)
        {
            respawnPoint.transform.position = respawnPositions[0].position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("PlayerCollider") || other.CompareTag("CanPickUp")) && !_wasMoved)
        {
            StartCoroutine(TeleportObject(other));
        }
    }

    public void SetIslandCheckpoint(int islandNumber)
    {
        if (islandNumber >= 0 && islandNumber < respawnPositions.Length)
        {
            SetNewCheckpointPosition(islandNumber);
        }
    }

    private void SetNewCheckpointPosition(int checkpointNumber)
    {
        if (checkpointNumber >= 0 && checkpointNumber < respawnPositions.Length)
        {
            respawnPoint.transform.position = respawnPositions[checkpointNumber].position;
        }
    }

    private IEnumerator TeleportObject(Collider collidedObject)
    {
        if (collidedObject.CompareTag("PlayerCollider"))
        {
            // Play the SFX and the particle system.
            collidedObject.transform.parent.GetComponent<AudioSource>().PlayOneShot(waterSplashSfx);
            waterSplashParticleSystem.gameObject.transform.position = collidedObject.transform.position;
            waterSplashParticleSystem.Play();

            // Make the player "floaty"
            var parent = collidedObject.transform.parent;
            parent.GetComponent<PlayerMovement>().groundDrag = 20;

            // Teleport the player
            yield return new WaitForSeconds(respawnTime);
            parent.gameObject.transform.position = respawnPoint.transform.position;
            parent.GetComponent<PlayerMovement>().groundDrag = parent.GetComponent<PlayerMovement>().defaultDrag;
        }
        else
        {
            collidedObject.GetComponent<AudioSource>().PlayOneShot(waterSplashSfx);
            waterSplashParticleSystem.gameObject.transform.position = collidedObject.transform.position;
            waterSplashParticleSystem.Play();
            yield return new WaitForSeconds(respawnTime);
            collidedObject.transform.position = respawnPoint.transform.position;
        }

        respawnParticleSystem.Play();
        _portalAudioSource.PlayOneShot(respawnSfx);
    }
}