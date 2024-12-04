using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThrowable : MonoBehaviour
{
    [SerializeField] private GameObject throwable;
    [SerializeField] private GameObject throwablePosition;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InstantiateThrowable(30f));
        throwable = GameObject.Find("PickableObject (1)");
        throwablePosition = GameObject.Find("ThrowablePosition");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator InstantiateThrowable(float f)
    {
        while (true)
        {
            yield return new WaitForSeconds(f);
            Instantiate(throwable, throwablePosition.transform.position, Quaternion.identity);
        }
    }
}
