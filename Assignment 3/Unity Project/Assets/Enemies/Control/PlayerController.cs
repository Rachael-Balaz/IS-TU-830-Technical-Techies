using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour {

        Health health;
    private void Start() 
        {
            health = GetComponent<Health>();
        }
    private void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
               CombatTarget target = hit.transform.GetComponent<CombatTarget>();
               //if no Combat Target Player cant click on it
               //to avoid on clicking ourself or npc been attacing
               if (target == null ) continue;
                //Checking if this is GameObject is null
                //if CanAttack return false then go on into next thing
                //This method will check whether this target has health component or not
                //if its not then its going to fail
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {   
                    continue;
                }
                // if mouse button pointed to or down player will try to attack it
                if (Input.GetMouseButton(1))
               {
                   GetComponent<Fighter>().Attack(target.gameObject);
               }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(1))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}