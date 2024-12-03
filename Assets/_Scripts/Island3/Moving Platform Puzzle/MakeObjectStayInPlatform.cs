using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Island3.Moving_Platform_Puzzle
{
    public class MakeObjectStayInPlatform : MonoBehaviour
    {
        [FormerlySerializedAs("playerColliderTag")] [SerializeField] private string objectToStayTag = "PlayerCollider";
        private Transform _platform;

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
    }
}
