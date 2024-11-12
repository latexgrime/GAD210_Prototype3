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
        crosshairAnimator = crosshair.GetComponent<Animator>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayCastDistance))
        {
            if (hit.transform.CompareTag($"Card"))
            {
                Debug.Log(hit.transform.name);
                crosshairAnimator.SetBool("Interacting",true);
                
            }
            else if (!hit.transform.CompareTag($"Card") || hit.transform == null)
            {
                crosshairAnimator.SetBool("Interacting",false);
            }
        }
    }
}