using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraStatsManager : MonoBehaviour
    {
        public BaseCameraManager _myDataManager;
        public int myID { get; set; }

        public bool didInit { get; set; }

        private void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            Debug.Log("Init Security Cameras");
            SetupDataManager();
            didInit = true;
        }

        public virtual void SetCameraDetails(int anID)
        {
            //Allow handling multiple cameras
            myID = anID;
        }

        public virtual void SetupDataManager()
        {
            if (_myDataManager == null)
                _myDataManager = GetComponent<BaseCameraManager>();
            if (_myDataManager == null)
                _myDataManager = gameObject.AddComponent<BaseCameraManager>();
            if (_myDataManager == null)
                _myDataManager = GetComponent<BaseCameraManager>();
        }

        public float GetRadius()
        {
            if (!didInit)
                Init();

            return _myDataManager.GetRadius(myID);
        }

        public virtual void AddRadius(float anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.AddRadius(myID, anAmount);
        }

        public virtual void ReduceRadius(float anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.ReduceRadius(myID, anAmount);
        }
        
        public virtual void SetRadius(float anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.SetRadius(myID, anAmount);
        }

        public float GetAngle()
        {
            if (!didInit)
                Init();

            return _myDataManager.GetAngle(myID);
        }

        public virtual void AddAngle(float anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.AddAngle(myID, anAmount);
        }

        public virtual void ReduceAngle(float anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.ReduceAngle(myID, anAmount);
        }

        public virtual void SetAngle(float anAmount)
        {
            if (!didInit)
                Init();

            _myDataManager.SetAngle(myID, anAmount);
        }

        public LayerMask GetTargetMask()
        {
            if (!didInit)
                Init();

            return _myDataManager.GetTargetMask(myID);
        }

        public virtual void SetTargetMask(LayerMask targetLayerMask)
        {
            if (!didInit)
                Init();

            _myDataManager.SetTargetMask(myID, targetLayerMask);
        }

        public LayerMask GetObstructionMask()
        {
            if (!didInit)
                Init();

            return _myDataManager.GetObstructionMask(myID);
        }

        public virtual void SetObstructionMask(LayerMask obstructionLayerMask)
        {
            if (!didInit)
                Init();

            _myDataManager.SetObstructionMask(myID, obstructionLayerMask);
        }

        public bool GetCanSeePlayer()
        {
            if (!didInit)
                Init();

            return _myDataManager.GetCanSeePlayer(myID);
        }

        public virtual void SetCanSeePlayer(bool value)
        {
            if (!didInit)
                Init();

            _myDataManager.SetCanSeePlayer(myID, value);
        }
    }
}
