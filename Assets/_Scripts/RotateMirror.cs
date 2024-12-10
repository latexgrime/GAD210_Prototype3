using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMirror : MonoBehaviour
{
    public GameObject tower;

    public float rotationSpeed;

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RotateTowers();
        }
    }

    void RotateTowers()
    {
        tower.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}


