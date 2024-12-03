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
            _wasMoved = true;
            StartCoroutine(MoveObject(other));
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

    private IEnumerator MoveObject(Collider collidedObject)
    {
        if (collidedObject == null) yield break;

        Vector3 collisionPosition = collidedObject.transform.position;
        
        if (waterSplashParticleSystem != null)
        {
            waterSplashParticleSystem.gameObject.transform.position = collisionPosition;
            waterSplashParticleSystem.Play();
        }

        if (collidedObject.CompareTag("PlayerCollider"))
        {
            Transform parent = collidedObject.transform.parent;
            if (parent != null)
            {
                if (parent.TryGetComponent(out AudioSource playerAudio))
                {
                    playerAudio.PlayOneShot(waterSplashSfx);
                }

                if (parent.TryGetComponent(out PlayerMovement playerMovement))
                {
                    // Make the player floaty.
                    playerMovement.groundDrag = 20;

                    // Teleport the player.
                    yield return new WaitForSeconds(respawnTime);
                    parent.position = respawnPoint.transform.position;
                    playerMovement.groundDrag = 1;
                }
            }
        }
        else
        {
            if (collidedObject.TryGetComponent(out AudioSource objectAudio))
            {
                objectAudio.PlayOneShot(waterSplashSfx);
            }

            yield return new WaitForSeconds(respawnTime);
            collidedObject.transform.position = respawnPoint.transform.position;
        }
        
        if (respawnParticleSystem != null)
        {
            respawnParticleSystem.Play();
        }

        if (_portalAudioSource != null && respawnSfx != null)
        {
            _portalAudioSource.PlayOneShot(respawnSfx);
        }

        _wasMoved = false;
    }
}