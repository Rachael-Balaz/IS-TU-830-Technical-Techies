using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour 
    {
        GameObject player;
        private void Start() 
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player= GameObject.FindWithTag("Player");

        }
        //This method will stop player action and disable player controller
        void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        //this method will enable player controller
        void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }

        
    }
}
