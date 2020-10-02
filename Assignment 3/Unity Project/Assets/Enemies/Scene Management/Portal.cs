using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        //Create a drop down menu to identity Portal when use as a SerializeField
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;



        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag == "Player")
            {
                //Coroutine runs between scene
                StartCoroutine(Transition());
            }  
        }

        private IEnumerator Transition()
        {
            //print debug error if sceneToLoad is not set
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }
            //Before the scene Load
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            //Fade Out in "fadeOutTime"
            yield return fader.FadeOut(fadeOutTime);

            //Save current level 
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            //Return Async opertation and call Coroutine again when the scene load
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // Load current level
            wrapper.Load();
            //After the scene Load
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);

            //Fade In in "fadeOutTime"
            yield return fader.FadeIn(fadeInTime);
            Destroy(gameObject);
            
        }

        //Update player position after enter portal
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            //Update player position & Rotation
            //using Warp() to warp Player to set position to avoid
            //mutiple terrain collision that affect player position
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        //Find Portal
        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                //find other portal that is not the current one
                if (portal == this) continue;
                //Only return the portal if it has the right destination
                //and its not the current standing one
                if (portal.destination != destination) continue;
                return portal;
                
            }
            return null;

        }
    }
}

