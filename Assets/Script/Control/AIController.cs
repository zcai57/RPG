using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;
using RPG.Attributes;

namespace RPG.Control {
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float maxChaseDistance = 5f;
        [SerializeField] float suspictionDuration = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wayPointTolerance = 1f;
        [SerializeField] float dwellTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.5f;
        [SerializeField] Boolean isArcher = false;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeAtWayPoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Start() {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.isDead()) return;

            if (InChaseDistance(player) && fighter.CanAttack(player))
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspictionDuration)
            {
                SuspicionBehavior();
            }
            else if (OutChaseDistance(player))
            {   
                PatrolBehavior();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeAtWayPoint += Time.deltaTime;
        }

        private void PatrolBehavior()
        {   
            Vector3 nextPosition = guardPosition;
            if(patrolPath != null) {
                if(AtWayPoint()) {
                    timeAtWayPoint = 0;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }

            if(timeAtWayPoint > dwellTime) {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPoint(currentWaypointIndex);
        }

        private void CycleWayPoint()
        {
            currentWaypointIndex = patrolPath.getNextIndex(currentWaypointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWaypoint < wayPointTolerance;
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool InChaseDistance(GameObject player)
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }

        private bool OutChaseDistance(GameObject player) 
        {
            return Vector3.Distance(transform.position, player.transform.position) > maxChaseDistance;
        }
        
        //Called by unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}