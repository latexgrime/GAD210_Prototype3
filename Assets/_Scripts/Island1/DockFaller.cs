using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DockFaller : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject dock;

    [SerializeField] private Animator anim;

    [SerializeField] private UnityEvent onTargetCollisionEvent;

    [SerializeField] private GameObject firstTarget;
    [SerializeField] private GameObject secondTarget;
    [SerializeField] private GameObject thirdTarget;
    [SerializeField] private GameObject fourthTarget;
    #endregion
    
    // Start is called before the first frame update
    private void Start()
    {
        firstTarget = GameObject.Find("Target");
        secondTarget = GameObject.Find("Target_1");
        thirdTarget = GameObject.Find("Target_2");
        fourthTarget = GameObject.Find("Target_3");
    }

    private void Update()
    {
        if (EntranceExit.counter >= 4)
            anim.SetTrigger("GoDown");
    }


     void OnCollisionEnter(Collision other)
    {
        dock = GameObject.Find("DockPivot(1-2)");
        if (name == "Target")
        {
            EntranceExit.counter++;
            Destroy(firstTarget);
            onTargetCollisionEvent.Invoke();
        }
        if (name == "Target_1")
        {
            EntranceExit.counter++;
            Destroy(secondTarget);
            onTargetCollisionEvent.Invoke();
        }
        if (name == "Target_2")
        {
            EntranceExit.counter++;
            Destroy(thirdTarget);
            onTargetCollisionEvent.Invoke();
        }
        if (name == "Target_3")
        {
            EntranceExit.counter++;
            Destroy(fourthTarget);
            onTargetCollisionEvent.Invoke();
        }
    }
    
    
}
