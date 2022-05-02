using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    [System.Serializable]
    public class BaseSecuritySystemManager : MonoBehaviour
    {
        public SecuritySystem.State currentSystemState;
        public SecuritySystem.State targetSystemState;
        public SecuritySystem.State lastSystemState;


        public void SetTargetState(SecuritySystem.State aState)
        {
            targetSystemState = aState;

            if (targetSystemState != currentSystemState)
                UpdateTargetState();
        }

        public SecuritySystem.State GetCurrentState()
        {
            return currentSystemState;
        }

        public UnityEvent OnIdle;
        public UnityEvent OnLookingForPlayer;
        public UnityEvent OnEngaged;

        public virtual void Idle() { OnIdle.Invoke(); }
        public virtual void LookingForPlayer() { OnLookingForPlayer.Invoke(); }
        public virtual void Enagaged() { OnEngaged.Invoke(); }

        public virtual void UpdateTargetState()
        {
            if (targetSystemState == currentSystemState)
                return;
            switch (targetSystemState)
            {
                case SecuritySystem.State.idle:
                    break;

                case SecuritySystem.State.lookingForPlayer:
                    break;

                case SecuritySystem.State.engaged:
                    break;
            }
            currentSystemState = targetSystemState;
        }

        public virtual void UpdateCurrentState()
        {
            switch (currentSystemState)
            {
                case SecuritySystem.State.idle:
                    break;

                case SecuritySystem.State.lookingForPlayer:
                    break;

                case SecuritySystem.State.engaged:
                    break;
            }
        }
    }
}
