using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject PersistentObjectPrefab;

        static bool hasSpawned = false;
        private void Awake() 
        {
            if (hasSpawned) return;
            SpawnPersistentObject();

            hasSpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject PersistentObject = Instantiate(PersistentObjectPrefab);
            DontDestroyOnLoad(PersistentObject);
        }
    }

}