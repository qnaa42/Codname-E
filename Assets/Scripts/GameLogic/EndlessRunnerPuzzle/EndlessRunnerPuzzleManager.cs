using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRunnerPuzzleManager : MonoBehaviour
{
    public GameObject[] Obstruction;
    public List<GameObject> spawnedObstruction = new List<GameObject>();
    public Transform spawningPoint;

    private bool didInit;

    public bool isRunning;
    public int difficulty;
    public PuzzleTicket myTicket;
    
    public float timeBetweenSpawn;
    public float obstructionSpeed;
    public int howManySpawn;
    public int howManySucesses;

    public bool PuzzleCompletedSucesfull;
    public bool PuzzleCompletedFailed;


    public GameObject playerToken;
    public float playerTokenSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        {
            if(!didInit)
            {
                Init();
                didInit = true;                
            }
            isRunning = false;
            StartCoroutine(Spawn());
        }
        if (howManySucesses == (2* howManySpawn))
        {
            myTicket.PuzzleCompletedSucesfull = true;
            PuzzleCleanUp();
        }
    }
    public IEnumerator Spawn()
    {
        for (int i = 0; i < howManySpawn; i++)
        {
            int whichObstruction = Random.Range(0, 5);
            GameObject obstructionSpawnee = Instantiate(Obstruction[whichObstruction], spawningPoint);
            spawnedObstruction.Add(obstructionSpawnee);
            EndlessRunnerPuzzleObstruction endlessRunnerPuzzleObstruction = obstructionSpawnee.GetComponent<EndlessRunnerPuzzleObstruction>();
            endlessRunnerPuzzleObstruction.moveSpeed = obstructionSpeed;                       
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }
    public void Init()
    {
        if(difficulty ==1)
        {
            myTicket.PuzzleCompletedFailed = false;
            myTicket.PuzzleCompletedSucesfull = false;
            howManySucesses = 0;
            timeBetweenSpawn = 3;
            obstructionSpeed = 5f;
            howManySpawn = 3;
        }
    }

    public void PuzzleCleanUp()
    {
        didInit = false;
        this.gameObject.SetActive(false);
    }
}
