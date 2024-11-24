using System;
using Unity.VisualScripting;
using UnityEngine;
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

        if (heldObject)
        {
            // Set the position of the grabbed object to be in front of the player.
            heldObject.transform.position = transform.position + objectDistance * transform.forward +
                                            objectHeight * transform.up;
            // Set the rotation of the object to be the same as the player.
            heldObject.transform.rotation = transform.rotation;
            
            if (Input.GetKeyDown(pickUpObjectKeycode))
            {
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
                }
            }
        }
        
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