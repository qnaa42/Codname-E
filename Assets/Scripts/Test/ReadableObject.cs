using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ReadableObject : Object
    {
        public string objectTextBody;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ReadObject()
        {
            GameManager.instance.uiManager.DisplayTextFromObject(objectTextBody);
        }
    }
}
