using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCollier : MonoBehaviour
{
    public bool isConnectedToEnd = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<WaterFlowCollider>().isConnectedToEnd)
        {
            isConnectedToEnd = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.GetComponent<WaterFlowCollider>().isConnectedToEnd)
        {
            isConnectedToEnd = true;
        }
    }
}
