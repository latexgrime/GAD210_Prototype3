using System;
using TMPro;
using UnityEngine;

namespace _Scripts.Island3.Moving_Platform_Puzzle
{
    public class MovingPlatformBehaviour : MonoBehaviour
    {
        [Header("- References")]
        [SerializeField] private Transform startPosition;
        [SerializeField] private Transform endPosition;

        [Header("-Movement settings")] 
        [SerializeField] private float movementDuration = 4f;

        private float _currentTime;
        private bool _positionChangeCheck;
        private Vector3 _currentStartingPosition;
        private Vector3 _currentTargetPosition;
        
        private void Start()
        {
            _currentTime = 0f;
            _currentStartingPosition = startPosition.position;
            _currentTargetPosition = endPosition.position;
        }

        private void Update()
        {
            if (_currentTime < movementDuration)
            {
                _currentTime += Time.deltaTime;

                transform.position = Vector3.Lerp(_currentStartingPosition, _currentTargetPosition, _currentTime);

                if (_currentTime >= movementDuration)
                {
                    _currentTime = 0f;

                    _positionChangeCheck = !_positionChangeCheck;
                    
                    _currentStartingPosition = _positionChangeCheck ? startPosition.position : endPosition.position;
                    _currentTargetPosition = _positionChangeCheck ? endPosition.position : startPosition.position;
                }
            }
        }
    }
}