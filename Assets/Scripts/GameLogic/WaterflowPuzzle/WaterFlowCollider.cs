using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlowCollider : MonoBehaviour
{
    public bool isConnectedToPipe = false;
    public bool isConnectedToEnd = false;

    private void Update()
    {
        Pipe parentPipe = this.transform.GetComponentInParent<Pipe>();
        if (parentPipe.isConnectedToEnd)
        {
            isConnectedToEnd = true;
        }
           
    }

    private void OnCollisionEnter(Collision collision)
    {                
        isConnectedToPipe = true;
        if (!collision.transform.GetComponent<EndCollider>() && !collision.transform.GetComponent<StartCollier>())
        {
            if (collision.transform.GetComponent<WaterFlowCollider>().isConnectedToEnd)
            {
                isConnectedToEnd = true;
            }
        }
        if(collision.transform.GetComponent<EndCollider>())
        {
            isConnectedToEnd = true;
        }    

        Pipe parentPipe = this.transform.GetComponentInParent<Pipe>();        
        parentPipe.UpdateColliders();
        if (!collision.transform.GetComponent<EndCollider>() && !collision.transform.GetComponent<StartCollier>())
        {
            Pipe contactPipe = collision.transform.GetComponentInParent<Pipe>();
            contactPipe.UpdateColliders();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.transform.GetComponent<EndCollider>() && !collision.transform.GetComponent<StartCollier>())
        {
            if (collision.transform.GetComponent<WaterFlowCollider>().isConnectedToEnd)
            {
                isConnectedToEnd = true;
                Pipe parentPipe = this.transform.GetComponentInParent<Pipe>();
                parentPipe.UpdateColliders();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isConnectedToEnd = false;
        isConnectedToPipe = false;
        Pipe parentPipe = this.transform.GetComponentInParent<Pipe>();
        parentPipe.UpdateColliders();        
        if (!collision.transform.GetComponent<EndCollider>() && !collision.transform.GetComponent<StartCollier>())
        {
            Pipe contactPipe = collision.transform.GetComponentInParent<Pipe>();
            contactPipe.UpdateColliders();
        }
    }
}
