using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public class CameraData
    {
        public int id;
        public float radius;
        public float angle;

        public LayerMask targetMask;
        public LayerMask obstructionMask;

        public bool canSeePlayer;
    }
}
