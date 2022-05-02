using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmPuzzleEndGate : MonoBehaviour
{
    public int good;
    public int bad;

    RhythmPuzzleManager puzzleManager;
    private void Start()
    {
        puzzleManager = this.GetComponentInParent<RhythmPuzzleManager>();
    }
    private void Update()
    {
        if (bad >3)
        {
            puzzleManager.StopAllCoroutines();
            puzzleManager.myTicket.PuzzleCompletedFailed = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        puzzleManager.howManyReachEnd++;
        RhythmPuzzleElement rhythmPuzzleElement = other.GetComponent<RhythmPuzzleElement>();
        if(rhythmPuzzleElement.Correct)
        {
            good++;
        }
        if (rhythmPuzzleElement.Bad)
        {
            bad++;
        }
        Destroy(other.gameObject);
    }
}
