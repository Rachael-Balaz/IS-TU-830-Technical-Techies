using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] GameObject weaponPrefab = null; 
        [SerializeField] Transform handTransform = null;
        [SerializeField] AnimatorOverrideController weaponOverride = null;

        Health target;
        //Using Mathf.Infinity to make timeSinceLastAttack always true
        //(lager thantimeBetweenAttacks) when first start.
        float timeSinceLastAttack = Mathf.Infinity;
        
        private void Start() {
            SpawnWeapon();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            //Check if player is in range of target or not
            if (!GetIsInRange())
            {
                //Move to if its in range
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);

            }
            else
            {
                //if not cancel movement and start AttackBehaviour()
                GetComponent<Mover>().Cancel();

                AttackBehaviour();
            }
        }
        private void SpawnWeapon()
        {
            Instantiate(weaponPrefab, handTransform);
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = weaponOverride;
        }

        private void AttackBehaviour()
        {
            //turn to target
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //Trigger animation attack and Hit()t event
                TriggerAttack();
                timeSinceLastAttack = 0;

            }
        }

        //Reset trigger to reset stopAttack anim before attack anim run
        //if not a glicth that appear that make character stop attack
        //before start a new attack
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event Lec34
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        //This method will check whether this target has health component or not
        public bool CanAttack(GameObject combatTarget)
        {
            //return fasle if player not click on target
            if (combatTarget == null) 
            {
                return false;
            }
            //Get Health component
            Health targetToTest = combatTarget.GetComponent<Health>();
            //if what player clicled on is a character and
            //if target dead return fasle in PlayerController
            //then continue the for each loop and not go into other actions (stop the attack)
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)        
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();

        }

        //Reset trigger to reset attack anim before StopAttack anim run
        //if not a glicth that appear that make character attack once more
        //before stop attack
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}
