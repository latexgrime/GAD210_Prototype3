using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Scripts.Island3.Moving_Platform_Puzzle
{
    public class MakeObjectStayInPlatform : MonoBehaviour
    {
        [FormerlySerializedAs("playerColliderTag")] [SerializeField] private string objectToStayTag = "PlayerCollider";
        private Transform _platform;
        [SerializeField] private UnityEvent onPlatformHit;

        private void Start()
        {
            _platform = GetComponent<Transform>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(objectToStayTag))
            {
                other.gameObject.transform.parent = _platform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(objectToStayTag))
            {
                other.gameObject.transform.parent = null;
                onPlatformHit.Invoke();
            }
        }
    }
}
