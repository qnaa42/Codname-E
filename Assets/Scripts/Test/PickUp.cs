using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PickUp : MonoBehaviour
    {
        public float pickUprange = 20f;
        private GameObject heldObj;
        public Transform holdParent;
        private float moveForce = 250;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (heldObj == null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)), out hit, pickUprange))
                    {
                        if (hit.transform.CompareTag("Grabable"))
                        {
                            PickUpObject(hit.transform.gameObject);
                        }
                        if(hit.transform.CompareTag("Readable"))
                        {
                            hit.transform.GetComponent<ReadableObject>().ReadObject();
                        }
                        if (hit.transform.CompareTag("Pickable"))
                        {
                            if (hit.transform.gameObject.GetComponent<ToolTag>())
                            {
                                hit.transform.gameObject.GetComponent<ToolTag>().pickUpTool();
                            }
                            GameManager.instance.inventory.AddToInventory(hit.transform.gameObject);
                        }
                        if(hit.transform.CompareTag("PuzzleConsole"))
                        {
                            PuzzleTicket puzzleTicket = hit.transform.GetComponent<PuzzleTicket>();
                            GameManager.instance.StartAPuzzle(puzzleTicket.typeOfPuzzle, puzzleTicket);
                        }
                    }
                    return;
                }
                else
                {
                    if (heldObj.gameObject.CompareTag("Pickable"))
                    {
                        GameManager.instance.inventory.AddToInventory(heldObj);
                        heldObj = null;
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            Rigidbody rb = heldObj.GetComponent<Rigidbody>();
                            heldObj.transform.parent = null;
                            rb.AddForce(holdParent.forward * 1000f);
                        }
                        DropObject();
                    }    
                }
            }

            if(Input.GetKey(KeyCode.Q) && heldObj != null)
            {
                DropObject();
            }

            if (heldObj != null)
            {
                MoveObject();

            }
        }



        public void MoveObject()
        {
            var rb = heldObj.GetComponent<Rigidbody>();
            //if we want to avoid rotation, but it clips the object
            //rb.constraints = RigidbodyConstraints.FreezePosition;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (Input.GetAxis("RotationHorizontal")!=0)
            {
                var horInput = Input.GetAxis("RotationHorizontal");
                
                heldObj.transform.Rotate(Vector3.up * horInput* Time.deltaTime * 25f);
            }
            if (Input.GetAxis("RotationVertical") != 0)
            {
                var horInput = Input.GetAxis("RotationVertical");

                heldObj.transform.Rotate(Vector3.right * horInput * Time.deltaTime * 25f);
            }
            if (Vector3.Distance(heldObj.transform.position, holdParent.position) > 0.1f)
            {
                Vector3 moveDirection = (holdParent.position - heldObj.transform.position);
                heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce); 
            }
        }

        public void PickUpObject(GameObject pickUp)
        {
            if (pickUp.CompareTag("Pickable"))
            {
                GameManager.instance.uiManager.inventoryInfo.SetActive(true);
            }
            if (pickUp.GetComponent<Rigidbody>())
            {
                var rb = pickUp.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.drag = 10;

                rb.transform.parent = holdParent;
                heldObj = pickUp;

                rb.velocity = Vector3.zero;
            }
        }

        public void DropObject()
        {
            if (heldObj.CompareTag("Pickable"))
            {
                GameManager.instance.uiManager.inventoryInfo.SetActive(false);
            }
            var rb = heldObj.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.drag = 1f;

            heldObj.transform.parent = null;
            heldObj = null;
        }
    }
}

