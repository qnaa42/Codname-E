using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{

    public class CameraClass : BaseCameraStateMachine
    {
        public int ID;

        public float radius;
        [Range(0, 360)]
        public float angle;
        [Range(0,30)]

        public float rotationAngle;

        [Range(0, 360)] public float rotationOffset;
        [Range(-90, 90)]public float rotationYaw;
        public float rotationSpeed;

        public GameObject playerRef;

        public LayerMask targetMask;
        public LayerMask obstructionMask;

        public bool canSeePlayer;

        public bool didInit;

        public float timerToEngage = 3f;
        private Light myLight;



        private void Start()
        {
            StartCoroutine(FOVRoutine(0.2f));
            SetTargetState(SecCamera.State.lookingForPlayer);
        }

        private void Update()
        {
            if (GameManager.instance.securitySystemManager.currentSystemState == SecuritySystem.State.lookingForPlayer)
            {
                if(currentCameraState == SecCamera.State.lookingForPlayer)
                {
                    transform.rotation = Quaternion.Euler(rotationYaw, rotationOffset + rotationAngle * Mathf.Sin(Time.time * rotationSpeed), 0f);
                    myLight.color = Color.green;
                }
                else
                {
                    myLight.color = Color.yellow;
                    FollowPlayer();                    
                }
            }
            else if (GameManager.instance.securitySystemManager.currentSystemState == SecuritySystem.State.engaged)
            {
                myLight.color = Color.red;
                if(currentCameraState == SecCamera.State.foundPlayer)
                {
                    FollowPlayer();                    
                }
                else
                {

                }
                
            }
            if (canSeePlayer && GameManager.instance.securitySystemManager.currentSystemState == SecuritySystem.State.lookingForPlayer)
            {
                SetTargetState(SecCamera.State.foundPlayer);
                timerToEngage -= Time.deltaTime;
                if (timerToEngage < 0)
                {
                    timerToEngage = 3f;
                    GameManager.instance.securitySystemManager.SetTargetState(SecuritySystem.State.engaged);
                }
            }
            else if (canSeePlayer && GameManager.instance.securitySystemManager.currentSystemState == SecuritySystem.State.engaged)
            {
                SetTargetState(SecCamera.State.foundPlayer);
            }
            else
            {
                SetTargetState(SecCamera.State.lookingForPlayer);
                if (timerToEngage < 3f)
                {
                    timerToEngage += Time.deltaTime;
                }
                else if (timerToEngage >3f)
                {
                    timerToEngage = 3f;
                }
            }
        }
        public void Init(int id)
        {
            myLight = GetComponent<Light>();
            playerRef = GameObject.Find("Ellise");
            UpdateData(id);
        }

        private void UpdateData(int id)
        {
            ID = id;
            GameManager.instance.securitySystemManager.cameraManager.SetCameraDetails(ID);
            radius = GameManager.instance.securitySystemManager.cameraManager.GetRadius();
            angle = GameManager.instance.securitySystemManager.cameraManager.GetAngle();
            targetMask = GameManager.instance.securitySystemManager.cameraManager.GetTargetMask();
            obstructionMask = GameManager.instance.securitySystemManager.cameraManager.GetObstructionMask();
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
        public override void UpdateTargetState()
        {
            Debug.Log("TargetSecurityCameraState = " + targetCameraState);
            switch (targetCameraState)
            {
                case SecCamera.State.lookingForPlayer:
                    break;

                case SecCamera.State.foundPlayer:
                    break;
            }
            currentCameraState = targetCameraState;
        }

        private void FollowPlayer()
        {
            Vector3 relativePos = playerRef.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            float tempEulerY = transform.rotation.eulerAngles.y;
            float tempEulerDelta = tempEulerY = rotation.eulerAngles.y;
            var finalEuler = transform.rotation.eulerAngles.y;
            if (tempEulerDelta > -rotationAngle && tempEulerDelta < rotationAngle)
            {
                finalEuler = rotation.eulerAngles.y;
            }
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, finalEuler, transform.rotation.eulerAngles.z);
        }
    }


}
