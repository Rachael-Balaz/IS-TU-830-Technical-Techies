using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSave : MonoBehaviour
{
    public GameObject Trigger;

     void OnTriggerEnter(Collider other)
    {
        Trigger.SetActive(true);

        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.SetSpawn(gameObject.GetComponent<Transform>().position);
        }
    }

}
