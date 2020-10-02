﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;


namespace RPG.Cinematics
{
    
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        bool alreadyTriggered = false;
        //if the trigger is not trigged before it will trigger 
        //and only object has Player tag can trigger
        private void OnTriggerEnter(Collider other) 
        {
            if (!alreadyTriggered && other.gameObject.tag == "Player")
            {
                alreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
        


           //These 2 methods will save and Load Character HP 
        //Save Character HP
        public object CaptureState()
        {
            return alreadyTriggered;
        }

        //Will be call after the level loaded just after awake
        //Load Character HP from save file
        public void RestoreState(object state)
        {
           alreadyTriggered = (bool)state;
        }
    }
}