using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
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
                Debug.Log(hit.transform.name);
                crosshairAnimator.SetBool("Interacting",true);
                crosshairImage.color = Color.green;

            }
            else if (!hit.transform.CompareTag($"Card") || hit.transform == null)
            {
                crosshairAnimator.SetBool("Interacting",false);
                crosshairImage.color = Color.white;
            }
        }
    }
}