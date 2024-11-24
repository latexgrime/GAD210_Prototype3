using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject player;
    private CardManager cardManager;

    [SerializeField] private float rayCastDistance;
    [SerializeField] private GameObject crosshair;
    private Animator crosshairAnimator;
    private Image crosshairImage;

    [Header("- Item interaction")] [SerializeField]
    private KeyCode pickUpObjectKeycode = KeyCode.E;
    [SerializeField] private float spherecastRadius = 1f;
    [SerializeField] private float objectDistance = 2f;
    [SerializeField] private float objectHeight = 0f;
    [SerializeField] private GameObject heldObject;
    /// <summary>
    /// The force intensity applied to the object when picked up.
    /// </summary>
    [SerializeField] private float movingObjectForce = 500f;
    /// <summary>
    /// Setting a large number for this so the amount of force applied when picking up an object doesn't break the immersion.
    /// </summary>
    [SerializeField] private float heldObjectDragTarget = 25f;

    private float heldObjectMainDrag;
    
    private void Start()
    {
        GetComponents();
    }

    // Get the required components.
    private void GetComponents()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cardManager = FindObjectOfType<CardManager>();
        crosshairAnimator = crosshair.GetComponent<Animator>();
        crosshairImage = crosshair.GetComponent<Image>();
        crosshairImage.color = Color.white;
    }

    private void Update()
    {
        CardInteractionCheck();
        PickUpOrDropObjectCheck();
    }
    
    private void FixedUpdate()
    {
        PickedUpObjectPhysics();
    }
    
    private void PickUpOrDropObjectCheck()
    {
        // If the player is already holding an object.
        if (heldObject)
        {
            Rigidbody heldObjectRb = heldObject.GetComponent<Rigidbody>();
            // Drop the object.
            if (Input.GetKeyDown(pickUpObjectKeycode))
            {
                // Set the amount of drag the object initially had.
                heldObjectRb.drag = heldObjectMainDrag;
                heldObjectRb.useGravity = true;
                heldObject = null;
            }
        }
        else
        {
            // If there is no object that was picked up, and the player presses E...
            if (Input.GetKeyDown(pickUpObjectKeycode))
            {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position + transform.forward, spherecastRadius, transform.forward, spherecastRadius);
                var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "CanPickUp");

                if (hitIndex != -1)
                {
                    var hitObject = hits[hitIndex].transform.gameObject;
                    heldObject = hitObject;
                    Rigidbody heldObjectRb = heldObject.GetComponent<Rigidbody>();
                    // Save the original drag value the object had to apply it later when the player drops the object.
                    heldObjectMainDrag = heldObjectRb.drag;
                    // Set the drag to the drag target value. This is to fix the problem with the object receiving too much force when picked up.
                    heldObjectRb.drag = heldObjectDragTarget;
                    heldObjectRb.useGravity = false;
                }
            }
        }
    }

    private void PickedUpObjectPhysics()
    {
        // Get references.
        Rigidbody heldObjectRb = heldObject.GetComponent<Rigidbody>();
        Vector3 moveObjectTo =
            transform.position + objectDistance * transform.forward + objectHeight * transform.up;
        Vector3 positionDifference = moveObjectTo - heldObject.transform.position;
            
        // Set the position of the grabbed object to be in front of the player.
        heldObjectRb.AddForce(positionDifference * movingObjectForce);
            
        // Set the rotation of the object to be the same as the player.
        heldObject.transform.rotation = transform.rotation;
    }

    // Cast a ray to check if its hitting any cards and plays a crosshair animation if so.
    private void CardInteractionCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayCastDistance))
        {
            if (hit.transform.CompareTag("Card"))
            {
                CrosshairInteractionAnimation(true);
                HandleCardInteraction(hit);
            }
            else if (!hit.transform.CompareTag("Card") || hit.transform == null)
            {
                CrosshairInteractionAnimation(false);
            }
        }
    }

    // Updates the crosshair to tell the player whatever they're looking at is interactable.
    private void CrosshairInteractionAnimation(bool isInteractable)
    {
        crosshairAnimator.SetBool("Interacting", isInteractable);
        if (isInteractable)
            crosshairImage.color = Color.green;
        else
            crosshairImage.color = Color.white;
    }

    // If the player clicks and the cards are not being reset, can flip the card the player is looking at.
    private void HandleCardInteraction(RaycastHit hit)
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && !cardManager.isResetting)
        {
            var cardInteracted = hit.transform.GetComponentInParent<Card>();

            // This checks if the card the player is interacting with is not the same they already clicked.
            if (!cardManager.selectedCards.Contains(cardInteracted))
            {
                cardInteracted.FlipCard();
                cardManager.selectedCards.Add(cardInteracted);
            }
        }
    }
}