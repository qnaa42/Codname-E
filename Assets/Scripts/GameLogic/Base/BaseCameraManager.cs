using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseCameraManager : MonoBehaviour
    {
        public static List<CameraData> global_cameraDatas;
        public bool didInit { get; set; }

        public void Init()
        {
            if (global_cameraDatas == null)
                global_cameraDatas = new List<CameraData>();

            didInit = true;
        }

        public void ResetSystem()
        {
            if (!didInit)
                Init();

            global_cameraDatas = new List<CameraData>();
        }

        public List<CameraData> GetCameraList()
        {
            if (global_cameraDatas == null)
                Init();

            return global_cameraDatas;
        }

        public int AddNewCamera()
        {
            if (!didInit)
                Init();

            CameraData newCamera = new CameraData();

            newCamera.id = global_cameraDatas.Count;

            newCamera.radius = 25;
            newCamera.angle = 45;

            newCamera.targetMask = LayerMask.GetMask("TargetLayer");
            newCamera.obstructionMask = LayerMask.GetMask("ObstructionLayer");

            newCamera.canSeePlayer = false;

            global_cameraDatas.Add(newCamera);

            return newCamera.id;       
        }

        public float GetRadius(int id)
        {
            if (!didInit)
                Init();

            return global_cameraDatas[id].radius;
        }

        public void AddRadius(int id, float anAmount)
        {
            if (!didInit)
                Init();

            global_cameraDatas[id].radius += anAmount;
        }

        public void ReduceRadius(int id, float anAmount)
        {
            if (!didInit)
                Init();

            global_cameraDatas[id].radius -= anAmount;
        }

        public void SetRadius(int id, float anAmount)
        {
            if (!didInit)
                Init();

            global_cameraDatas[id].radius = anAmount;
        }

        public float GetAngle(int id)
        {
            if (!didInit)
                Init();

            return global_cameraDatas[id].angle;
        }

        public void AddAngle(int id, float anAmount)
        {
            if (!didInit)
                Init();

            global_cameraDatas[id].angle += anAmount;
        }

        public void ReduceAngle(int id, float anAmount)
        {
            if (!didInit)
                Init();

            global_cameraDatas[id].angle -= anAmount;
        }

        public void SetAngle(int id, float anAmount)
        {
            if (!didInit)
                Init();

            global_cameraDatas[id].angle = anAmount;
        }

        public LayerMask GetTargetMask(int id)
        {
            if (!didInit)
                Init();

            return global_cameraDatas[id].targetMask;
        }

        public void SetTargetMask(int id, LayerMask targetLayerMask)
        {
            if (!didInit)
                Init();

            global_cameraDatas[id].targetMask = targetLayerMask;
        }

        public LayerMask GetObstructionMask(int id)
        {
            if (!didInit)
                Init();

            return global_cameraDatas[id].obstructionMask;
        }

        public void SetObstructionMask(int id, LayerMask obstructionLayerMask)
        {
            if (!didInit)
                Init();

            global_cameraDatas[id].obstructionMask = obstructionLayerMask;
        }

        public bool GetCanSeePlayer(int id)
        {
            if (!didInit)
                Init();

            return global_cameraDatas[id].canSeePlayer;
        }

        public void SetCanSeePlayer(int id, bool value)
        {
            if (!didInit)
                Init();

            global_cameraDatas[id].canSeePlayer = value;
        }

    }
}

