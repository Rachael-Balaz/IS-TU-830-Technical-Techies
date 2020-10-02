using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        [SerializeField] float waypointDwellTime = 3f;
        [Range(0,1)]

        //Patrol Speed = 20% of MaxSpeed
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWayPoint = Mathf.Infinity;

        int currentWaypointIndex = 0;

        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;
            //if player in range of Player and can attack
            //Then Attack Player
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                //Reset timeSinceLastSawPlayer when attack player
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //Suspicion state
                SuspicionBehaviour();
            }
            //Go back to guarding position if player ran off
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWayPoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWayPoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWayPoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);


            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        //return distance to player
        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected() 
	{
            //Draw a red Sphere when object is selected
            //Automaticly updated when chaseDistance is changed
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}