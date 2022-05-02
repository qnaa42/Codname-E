using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum ObjectType
    {
        Pickable,
        EnviormentInteractable,
        Readable,
    }


    public class Object : MonoBehaviour
    {
        public int objectId;
        public string objectName;
        public ObjectType objecttype;



        public float pickUprange = 5f;
        // Start is called before the first frame update

        private void Update()
        {
        }




    }
}

