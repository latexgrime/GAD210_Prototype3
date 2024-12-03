using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Island3.Moving_Platform_Puzzle
{
    public class MovingPlatformBehaviour : MonoBehaviour
    {
        [Header("- References")] [SerializeField]
        private Transform startPosition;

        [SerializeField] private Transform endPosition;

        [FormerlySerializedAs("movementDuration")]
        [Header("-Movement settings")] 
        [SerializeField] private float changePositionTime = 4f;
        [SerializeField] private float smoothFactor = 0.5f;

        private float _currentTime;
        private bool _positionChangeCheck = true;
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
            if (_currentTime < changePositionTime)
            {
                _currentTime += Time.deltaTime;

                float easedTime = Mathf.SmoothStep(0.0f, smoothFactor, _currentTime);
                
                transform.position = Vector3.Lerp(_currentStartingPosition, _currentTargetPosition, easedTime);

                if (_currentTime >= changePositionTime)
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