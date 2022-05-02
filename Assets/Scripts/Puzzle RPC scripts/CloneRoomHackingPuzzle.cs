using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneRoomHackingPuzzle : MonoBehaviour
{
    public PuzzleTicket ticket;

    private void Update()
    {
        if (ticket.PuzzleCompletedSucesfull)
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().goodJobAudio.Play();
        }
        if(ticket.PuzzleCompletedFailed)
        {
            GameManager.instance.respawnManager.KillAllClones();
        }
    }
}
