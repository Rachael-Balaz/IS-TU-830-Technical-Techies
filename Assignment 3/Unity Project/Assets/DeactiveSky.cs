using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveSky : MonoBehaviour
{
    public GameObject Trigger;
    void OnTriggerEnter(Collider other)
    {
        Trigger.SetActive(false);
    }
}
