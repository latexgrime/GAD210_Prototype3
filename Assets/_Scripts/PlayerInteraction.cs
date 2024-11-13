using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private CardManager cardManager;
    
    [SerializeField] private float rayCastDistance;
    [SerializeField] private GameObject crosshair;
    private Animator crosshairAnimator;
    private Image crosshairImage;

    private void Start()
    {
        GetComponents();
    }

    // Get the required components.
    private void GetComponents()
    {
        cardManager = FindObjectOfType<CardManager>();
        crosshairAnimator = crosshair.GetComponent<Animator>();
        crosshairImage = crosshair.GetComponent<Image>();
        crosshairImage.color = Color.white;
    }

    private void Update()
    {
        CardInteractionCheck();
    }

    // Cast a ray to check if its hitting any cards and plays a crosshair animation if so.
    private void CardInteractionCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayCastDistance))
        {
            if (hit.transform.CompareTag($"Card"))
            {
                crosshairAnimator.SetBool("Interacting",true);
                crosshairImage.color = Color.green;
                
                // If the player clicks and the cards are not being reset, can flip the card the player is looking at.
                if (Input.GetKeyUp(KeyCode.Mouse0) && !cardManager.isResetting)
                {
                    Card cardInteracted = hit.transform.GetComponentInParent<Card>();
                    
                    // This checks if the card the player is interacting with is not the same they already clicked.
                    if (!cardManager.selectedCards.Contains(cardInteracted))
                    {
                        cardInteracted.FlipCard();
                        cardManager.selectedCards.Add(cardInteracted);
                    }

                }
            }
            else if (!hit.transform.CompareTag($"Card") || hit.transform == null)
            {
                crosshairAnimator.SetBool("Interacting",false);
                crosshairImage.color = Color.white;
            }
        }
    }
}