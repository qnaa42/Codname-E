using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SecuritySystemManager : BaseSecuritySystemManager
    {
        public BaseCameraManager baseCameraManager;
        public CameraStatsManager cameraManager;

        public GameObject camerasParent;

        private void Awake()
        {
            if (baseCameraManager == null)
                baseCameraManager = GetComponent<BaseCameraManager>();

            if (baseCameraManager.GetCameraList().Count< camerasParent.transform.childCount+1)
            {
                baseCameraManager.ResetSystem();
                for (int i = 0; i < camerasParent.transform.childCount; i++)
                {
                    baseCameraManager.AddNewCamera();
                    CameraClass thisCamera = camerasParent.transform.GetChild(i).GetComponent<CameraClass>();
                    thisCamera.Init(i);
                }                
            }
        }

        public void Start()
        {
            SetTargetState(SecuritySystem.State.lookingForPlayer);
            
        }

        public override void UpdateTargetState()
        {
            Debug.Log("TargetSecuritySustemState = " + targetSystemState);
            if (targetSystemState == currentSystemState)
                return;

            switch (targetSystemState)
            {
                case SecuritySystem.State.idle:
                    SetTargetState(SecuritySystem.State.lookingForPlayer);
                    break;

                case SecuritySystem.State.lookingForPlayer:
                    break;

                case SecuritySystem.State.engaged:
                    break;
            }
            currentSystemState = targetSystemState;
        }
    }
}
