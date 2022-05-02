using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterflowManager : MonoBehaviour
{

    public GameObject pipesHolder;
    public List<Pipe> pipes = new List<Pipe>();

    public PuzzleTicket myPuzzleTicket;

    public int numberOfTries;
    public float timeToSolve;
    public int difficulty;

    private bool didInit = false;
    public bool puzzleIsRunning;
    public bool puzzleIsComplete;
    public bool puzzleFailed;

    int totalPipes;

    public StartCollier startCollider;

    // Start is called before the first frame update

    private void Start()
    {
        startCollider = GameObject.Find("StartPipe").transform.GetChild(1).GetComponent<StartCollier>();
    }

    private void Init()
    {
        myPuzzleTicket.PuzzleCompletedFailed = false;
        myPuzzleTicket.PuzzleCompletedSucesfull = false;
        if (difficulty == 1)
        { timeToSolve = 20; }        
        totalPipes = pipesHolder.transform.childCount;
        for (int i = 0; i < pipesHolder.transform.childCount; i++)
        {
            pipes.Add(pipesHolder.transform.GetChild(i).GetComponent<Pipe>());
        }
        didInit = true;
        UpdateAllPipes();
    }

    private void Update()
    {
        
        if(puzzleIsComplete)
        {
            myPuzzleTicket.PuzzleCompletedFailed = false;
            myPuzzleTicket.PuzzleCompletedSucesfull = true;
            puzzleIsRunning = false;
            puzzleFailed = false;
            timeToSolve = 20;
        }        
        if(puzzleIsRunning)
        {
            if(!didInit)
            {
                Init();
            }
            timeToSolve -= Time.deltaTime;
            if (timeToSolve <0)
            {
                puzzleIsRunning = false;
                if (!puzzleIsComplete && timeToSolve < 0)
                {
                    myPuzzleTicket.PuzzleCompletedFailed = true;
                    myPuzzleTicket.PuzzleCompletedSucesfull = false;
                    puzzleFailed = true;
                    timeToSolve = 20;
                    PuzzleDispose();
                }
            }
        }
        if(startCollider.isConnectedToEnd)
        {
            puzzleIsComplete = true;
            PuzzleDispose();
        }
    }


    public void UpdateAllPipes()
    {
        for (int i = 0; i < pipesHolder.transform.childCount; i++)
        {
            pipes[i].UpdateColliders();
        }
    }

    private void StartPuzzle()
    {

    }

    public void PuzzleDispose()
    {
        didInit = false;
        this.gameObject.SetActive(false);
    }
}
