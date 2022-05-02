using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Pipe : MonoBehaviour
    {
        public List<WaterFlowCollider> colliders = new List<WaterFlowCollider>();

        public bool isConectedToPipe;
        public bool isConnectedToEnd;

        float[] rotations = { 0, 90, 180, 270 };

        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                colliders.Add(transform.GetChild(i).GetComponent<WaterFlowCollider>());
            }

            int rand = Random.Range(0, rotations.Length);
            this.transform.eulerAngles = new Vector3(0, 0, rotations[rand]);
            //UpdateColliders();
        }

        private void Update()
        {

        }

        public void OnMouseDown()
        {
            this.transform.Rotate(new Vector3(0, 0, 90));
            
            UpdateColliders();
        }

        public void UpdateColliders()
        {
            isConectedToPipe = false;
            isConnectedToEnd = false;
            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].isConnectedToPipe == true)
                {
                    this.isConectedToPipe = true;
                }
                if (colliders[i].isConnectedToEnd == true)
                {
                    this.isConnectedToEnd = true;
                }
            }
        }
    }
}
