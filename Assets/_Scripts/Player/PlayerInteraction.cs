using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private CardManager _cardManager;
        private AudioSource _audioSource;

        [Header("- Crosshair settings")] [SerializeField]
        private GameObject crosshair;

        private Animator _crosshairAnimator;
        private Image _crosshairImage;

        [Header("- Item interaction")] 
        [SerializeField] private float launchForce = 25f;
        [SerializeField] private KeyCode pickUpObjectKeycode = KeyCode.E;
        [SerializeField] private float objectInteractionDistance = 3f; 
        [SerializeField] private float heldObjectPositionDistance = 2f;
        /// <summary>
        ///     The force intensity applied to the object when picked up.
        /// </summary>
        [SerializeField] private float movingObjectForce = 500f;
        /// <summary>
        ///     Setting a large number for this so the amount of force applied when picking up an object doesn't break the
        ///     immersion.
        /// </summary>
        [SerializeField] private float heldObjectDragTarget = 25f;

        [SerializeField] private UnityEvent endGameEvent;
    
        private GameObject _heldObject;
        private float _heldObjectHeight;
        private float _heldObjectMainDrag;
        private bool _throwObject = false;

        [Header("- SFX")] [SerializeField] private AudioClip throwObjectSfx;
        private float _defaultVolume;
        private float _defaultPitch;
    
        private void Start()
        {
            GetComponents();
        }

        // Get the required components.
        private void GetComponents()
        {
            _audioSource = GetComponent<AudioSource>();
            _cardManager = FindObjectOfType<CardManager>();
            _crosshairAnimator = crosshair.GetComponent<Animator>();
            _crosshairImage = crosshair.GetComponent<Image>();
            _crosshairImage.color = Color.white;
            _defaultVolume = _audioSource.volume;
            _defaultPitch = _audioSource.pitch;
        }

        private void Update()
        {
            CrosshairInteractionCheck();
            InteractionChecker();
            PickUpOrDropObjectCheck();
        }

        private void FixedUpdate()
        {
            PickedUpObjectPhysics();
        }

        private void PickUpOrDropObjectCheck()
        {
            // If the player is already holding an object.
            if (_heldObject)
            {
                var heldObjectRb = _heldObject.GetComponent<Rigidbody>();
                // Drop the object.
                if (Input.GetKeyDown(pickUpObjectKeycode))
                {
                    // Set the amount of drag the object initially had.
                    heldObjectRb.drag = _heldObjectMainDrag;
                    heldObjectRb.useGravity = true;
                    _heldObject = null;
                }
                // If the player clicks.
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _throwObject = true;
                }
            }
            else
            {
                // If there is no object that was picked up, and the player presses E...
                if (Input.GetKeyDown(pickUpObjectKeycode))
                {
                    var hits = Physics.RaycastAll(transform.position, transform.forward, objectInteractionDistance);
                    var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "CanPickUp");

                    if (hitIndex != -1)
                    {
                        var hitObject = hits[hitIndex].transform.gameObject;
                        _heldObject = hitObject;
                        var heldObjectRb = _heldObject.GetComponent<Rigidbody>();
                        // Save the original drag value the object had to apply it later when the player drops the object.
                        _heldObjectMainDrag = heldObjectRb.drag;
                        // Set the drag to the drag target value. This is to fix the problem with the object receiving too much force when picked up.
                        heldObjectRb.drag = heldObjectDragTarget;
                        heldObjectRb.useGravity = false;
                    }
                }
            }
        }

        private void InteractionChecker()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, objectInteractionDistance))
                if (hit.transform.CompareTag("Card"))
                    HandleCardInteraction(hit);
                
                else if (hit.transform.CompareTag("EndGame") && Input.GetKey(KeyCode.Mouse0))
                {
                    endGameEvent.Invoke();
                }
        }

        private void PickedUpObjectPhysics()
        {
            // If the player has an object picked up.
            if (_heldObject != null)
            {
                var heldObjectRb = _heldObject.GetComponent<Rigidbody>();
                var moveObjectTo =
                    transform.position + heldObjectPositionDistance * transform.forward + _heldObjectHeight * transform.up;
                var positionDifference = moveObjectTo - _heldObject.transform.position;

                // Set the position of the grabbed object to be in front of the player.
                heldObjectRb.AddForce(positionDifference * movingObjectForce);

                // Set the rotation of the object to be the same as the player.
                _heldObject.transform.rotation = transform.rotation;
            
                if (_throwObject)
                {
                    heldObjectRb.drag = _heldObjectMainDrag;
                    heldObjectRb.useGravity = true;
                    heldObjectRb.AddForce(transform.forward * launchForce);
                    _heldObject = null;
                    _throwObject = !_throwObject;
                    _audioSource.pitch = Random.Range(_defaultPitch - 0.1f, _defaultPitch + 0.1f);
                    _audioSource.volume = Random.Range(_defaultVolume - 0.2f, _defaultVolume + 0.2f);
                    _audioSource.PlayOneShot(throwObjectSfx);
                }
            }
        }

        // Cast a ray to check if there is an interactable object in front.
        private void CrosshairInteractionCheck()
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, transform.forward, out hit, objectInteractionDistance))
            {
                _crosshairAnimator.SetBool("Interacting", false);
                _crosshairImage.color = Color.white;
                return;
            }
            CrosshairInteractionAnimation(hit);
        }

        // Updates the crosshair to tell the player whatever they're looking at is interactable.
        private void CrosshairInteractionAnimation(RaycastHit hit)
        {
            // If the player is not looking at an object with any of those two tags, or not even looking at an object at all, set the crosshair white and with no animation.
            if (hit.transform == null || (!hit.transform.CompareTag("Card") && !hit.transform.CompareTag("CanPickUp") && (!hit.transform.CompareTag("EndGame"))))
            {
                _crosshairAnimator.SetBool("Interacting", false);
                _crosshairImage.color = Color.white;
                return;
            }

            // Set the crosshair animation if the player is looking at a card and set the crosshair green.
            if (hit.transform.CompareTag("Card") || hit.transform.CompareTag("EndGame"))
            {
                _crosshairAnimator.SetBool("Interacting", true);
                _crosshairImage.color = Color.green;
            }

            // Set the crosshair animation if the player is looking at an object that can be picked up and set the crosshair blue.
            if (hit.transform.CompareTag("CanPickUp"))
            {
                _crosshairAnimator.SetBool("Interacting", true);
                _crosshairImage.color = Color.blue;
            }
        }

        // If the player clicks and the cards are not being reset, can flip the card the player is looking at.
        private void HandleCardInteraction(RaycastHit hit)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0) && !_cardManager.isResetting)
            {
                var cardInteracted = hit.transform.GetComponentInParent<Card>();

                // This checks if the card the player is interacting with is not the same they already clicked.
                if (!_cardManager.selectedCards.Contains(cardInteracted))
                {
                    cardInteracted.FlipCard();
                    _cardManager.selectedCards.Add(cardInteracted);
                }
            }
        }
    }
}