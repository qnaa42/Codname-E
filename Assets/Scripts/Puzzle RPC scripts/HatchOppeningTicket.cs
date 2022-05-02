using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchOppeningTicket : MonoBehaviour
{
    public PuzzleTicket ticket;
    public GameObject hatch;


    private void Update()
    {
        if (ticket.PuzzleCompletedSucesfull == true)
        {
            hatch.gameObject.SetActive(false);
        }
    }
}
