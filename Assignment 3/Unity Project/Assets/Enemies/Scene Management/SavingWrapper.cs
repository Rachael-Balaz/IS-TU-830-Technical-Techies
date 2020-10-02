﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float FadeInTime = 0.2f;
        IEnumerator Start() 
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(FadeInTime);        
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }   
        }
        public void Save()
        {
            //Call to saving system load
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
        
        public void Load()
        {
            //Call to saving system load
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }


    }

}