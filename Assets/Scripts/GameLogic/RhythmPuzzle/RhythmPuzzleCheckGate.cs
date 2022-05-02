using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmPuzzleCheckGate : MonoBehaviour
{
    public bool elementInRangeOfInteraction = false;

    public GameObject puzzleElement = null;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (elementInRangeOfInteraction && Input.GetKeyDown(KeyCode.UpArrow))
        {
            RhythmPuzzleElement rhythmPuzzleElement = puzzleElement.GetComponent<RhythmPuzzleElement>();
            if (rhythmPuzzleElement.type == 1 && !rhythmPuzzleElement.Bad)
            {
                rhythmPuzzleElement.Correct = true;
            }
            else if (rhythmPuzzleElement.type != 1 && !rhythmPuzzleElement.Correct)
            {
                rhythmPuzzleElement.Bad = true;
            }
        }
        if (elementInRangeOfInteraction && Input.GetKeyDown(KeyCode.RightArrow))
        {
            RhythmPuzzleElement rhythmPuzzleElement = puzzleElement.GetComponent<RhythmPuzzleElement>();
            if (rhythmPuzzleElement.type == 2 && !rhythmPuzzleElement.Bad)
            {
                rhythmPuzzleElement.Correct = true;
            }
            else if (rhythmPuzzleElement.type != 2 && !rhythmPuzzleElement.Correct)
            {
                rhythmPuzzleElement.Bad = true;
            }
        }
        if (elementInRangeOfInteraction && Input.GetKeyDown(KeyCode.DownArrow))
        {
            RhythmPuzzleElement rhythmPuzzleElement = puzzleElement.GetComponent<RhythmPuzzleElement>();
            if (rhythmPuzzleElement.type == 3 && !rhythmPuzzleElement.Bad)
            {
                rhythmPuzzleElement.Correct = true;
            }
            else if (rhythmPuzzleElement.type != 3 && !rhythmPuzzleElement.Correct)
            {
                rhythmPuzzleElement.Bad = true;
            }
        }
        if (elementInRangeOfInteraction && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RhythmPuzzleElement rhythmPuzzleElement = puzzleElement.GetComponent<RhythmPuzzleElement>();
            if (rhythmPuzzleElement.type == 4 &&!rhythmPuzzleElement.Bad)
            {
                rhythmPuzzleElement.Correct = true;
            }
            else if (rhythmPuzzleElement.type != 4 && !rhythmPuzzleElement.Correct)
            {
                rhythmPuzzleElement.Bad = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        elementInRangeOfInteraction = true;
        puzzleElement = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        RhythmPuzzleElement rhythmPuzzleElement = puzzleElement.GetComponent<RhythmPuzzleElement>();
        if (!rhythmPuzzleElement.Correct)
        {
            rhythmPuzzleElement.Bad = true;
        }
        elementInRangeOfInteraction = false;
        puzzleElement = null;
    }

}
