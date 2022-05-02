using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    [System.Serializable]
    public class BaseCameraStateMachine : MonoBehaviour
    {
        public SecCamera.State currentCameraState;
        public SecCamera.State targetCameraState;
        public SecCamera.State lastCameraState;

        public void SetTargetState(SecCamera.State aState)
        {
            targetCameraState = aState;

            if (targetCameraState != currentCameraState)
                UpdateTargetState();
        }

        public SecCamera.State GetCurrentState()
        {
            return currentCameraState;
        }

        public UnityEvent OnLookingForPlayer;
        public UnityEvent OnPlayerFound;

        public virtual void LookingForPlayer() { OnLookingForPlayer.Invoke(); }
        public virtual void PlayerFound() { OnPlayerFound.Invoke(); }

        public virtual void UpdateTargetState()
        {
            if (targetCameraState == currentCameraState)
                return;
            switch(targetCameraState)
            {
                case SecCamera.State.lookingForPlayer:
                    break;

                case SecCamera.State.foundPlayer:
                    break;
            }
            currentCameraState = targetCameraState;
        }

        public virtual void UpdateCurrentState()
        {
            switch(currentCameraState)
            {
                case SecCamera.State.lookingForPlayer:
                    break;

                case SecCamera.State.foundPlayer:
                    break;
            }
        }
    }

    
}
