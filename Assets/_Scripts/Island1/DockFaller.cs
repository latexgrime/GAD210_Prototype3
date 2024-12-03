using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockFaller : MonoBehaviour
{
    public GameObject dock;

    [SerializeField] private Animator anim;
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
        Debug.Log("my name: " + name);
        Debug.Log("collision: " + other.gameObject.name);
        dock = GameObject.Find("DockPivot(1-2)");
        anim.SetTrigger("GoDown");
        Debug.Log("dock: " + dock);
    }
}
