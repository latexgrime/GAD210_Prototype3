using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceExit : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
