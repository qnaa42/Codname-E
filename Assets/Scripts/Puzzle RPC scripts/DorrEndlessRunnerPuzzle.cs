using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DorrEndlessRunnerPuzzle : MonoBehaviour
{
    public PuzzleTicket ticket;

    public GameObject leftDoorClosed;
    public GameObject rightDoorClosed;

    public GameObject leftDoorOpen;
    public GameObject rightDoorOpen;

    private void Update()
    {
        if(ticket.PuzzleCompletedSucesfull == true)
        {
            OpenTheDoors();
        }
    }

    public void OpenTheDoors()
    {        
            leftDoorClosed.SetActive(false);
            rightDoorClosed.SetActive(false);

            leftDoorOpen.SetActive(true);
            leftDoorOpen.SetActive(true);        
    }


}
