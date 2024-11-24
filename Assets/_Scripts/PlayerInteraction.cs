using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject player;
    private CardManager cardManager;

    [Header("- Crosshair settings")] [SerializeField]
    private GameObject crosshair;

    private Animator crosshairAnimator;
    private Image crosshairImage;

    [Header("- Item interaction")] [SerializeField]
    private KeyCode pickUpObjectKeycode = KeyCode.E;

    [SerializeField] private float objectInteractionDistance = 3f; 
    [SerializeField] private float heldObjectPositionDistance = 2f;

    [FormerlySerializedAs("objectHeight")] [SerializeField]
    private float heldObjectHeight;

    [SerializeField] private GameObject heldObject;

    /// <summary>
    ///     The force intensity applied to the object when picked up.
    /// </summary>
    [SerializeField] private float movingObjectForce = 500f;

    /// <summary>
    ///     Setting a large number for this so the amount of force applied when picking up an object doesn't break the
    ///     immersion.
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
        CrosshairInteractionCheck();
        CardInteractionChecker();
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
            var heldObjectRb = heldObject.GetComponent<Rigidbody>();
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
                var hits = Physics.RaycastAll(transform.position, transform.forward, objectInteractionDistance);
                var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "CanPickUp");

                if (hitIndex != -1)
                {
                    var hitObject = hits[hitIndex].transform.gameObject;
                    heldObject = hitObject;
                    var heldObjectRb = heldObject.GetComponent<Rigidbody>();
                    // Save the original drag value the object had to apply it later when the player drops the object.
                    heldObjectMainDrag = heldObjectRb.drag;
                    // Set the drag to the drag target value. This is to fix the problem with the object receiving too much force when picked up.
                    heldObjectRb.drag = heldObjectDragTarget;
                    heldObjectRb.useGravity = false;
                }
            }
        }
    }

    private void CardInteractionChecker()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, objectInteractionDistance))
            if (hit.transform.CompareTag("Card"))
                HandleCardInteraction(hit);
    }

    private void PickedUpObjectPhysics()
    {
        // If the player has an object picked up.
        if (heldObject != null)
        {
            var heldObjectRb = heldObject.GetComponent<Rigidbody>();
            var moveObjectTo =
                transform.position + heldObjectPositionDistance * transform.forward + heldObjectHeight * transform.up;
            var positionDifference = moveObjectTo - heldObject.transform.position;

            // Set the position of the grabbed object to be in front of the player.
            heldObjectRb.AddForce(positionDifference * movingObjectForce);

            // Set the rotation of the object to be the same as the player.
            heldObject.transform.rotation = transform.rotation;
        }
    }

    // Cast a ray to check if there is an interactable object in front.
    private void CrosshairInteractionCheck()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.forward, out hit, objectInteractionDistance))
        {
            crosshairAnimator.SetBool("Interacting", false);
            crosshairImage.color = Color.white;
            Debug.Log("No object.");
            return;
        }

        CrosshairInteractionAnimation(hit);
    }

    // Updates the crosshair to tell the player whatever they're looking at is interactable.
    private void CrosshairInteractionAnimation(RaycastHit hit)
    {
        // If the player is not looking at an object with any of those two tags, or not even looking at an object at all, set the crosshair white and with no animation.
        if (hit.transform == null || (!hit.transform.CompareTag("Card") && !hit.transform.CompareTag("CanPickUp")))
        {
            crosshairAnimator.SetBool("Interacting", false);
            crosshairImage.color = Color.white;
            return;
        }

        // Set the crosshair animation if the player is looking at a card and set the crosshair green.
        if (hit.transform.CompareTag("Card"))
        {
            crosshairAnimator.SetBool("Interacting", true);
            crosshairImage.color = Color.green;
        }

        // Set the crosshair animation if the player is looking at an object that can be picked up and set the crosshair blue.
        if (hit.transform.CompareTag("CanPickUp"))
        {
            crosshairAnimator.SetBool("Interacting", true);
            crosshairImage.color = Color.blue;
        }
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