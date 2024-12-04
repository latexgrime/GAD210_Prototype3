using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceExit : MonoBehaviour
{
    public static int counter = 0;

    private void OnCollisionEnter(Collision other)
    {
        if (gameObject.name == "Entrance")
        {
            if (other.collider.name == "Front Wall")
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }   
        }

        if (gameObject.name == "Exit")
        {
            if (other.collider.name == "Back Wall")
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }  
        }
    }
    
}
