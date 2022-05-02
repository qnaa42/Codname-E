using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public class FOVcheck : MonoBehaviour
    {
        public float radius;
        [Range(0,360)]
        public float angle;

        public GameObject playerRef;

        public LayerMask targetMask;
        public LayerMask obstructionMask;

        public bool canSeePlayer;

        private void Start()
        {
            playerRef = GameObject.Find("NewCharacter");
            StartCoroutine(FOVRoutine(0.2f));
        }

        private IEnumerator FOVRoutine(float delay)
        {
            WaitForSeconds wait = new WaitForSeconds(delay);

            while (true)
            {
                yield return wait;
                FieldOdViewCheck();
            }
        }

        private void FieldOdViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        canSeePlayer = true;
                    }
                    else
                    {
                        canSeePlayer = false;
                    }
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else if (canSeePlayer)
            {
                canSeePlayer = false;
            }
        }
    }
}
