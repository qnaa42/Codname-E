using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmPuzzleManager : MonoBehaviour
{
    public bool didInit;
    public PuzzleTicket myTicket;

    public GameObject arrow;
    public Transform spawnTransform;
    public RhythmPuzzleEndGate endGate;

    public float timeBeetwennSpawn;
    public float tempTimeBeetwenSpawn;
    public float arrowSpeed;
    public int howManySpawn;
    public int howManyReachEnd;

    public bool isRunning;
    public bool isSpawning;

    public int difficulty;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            if(!didInit)
            {
                Init();
                didInit = false;
            }
            isRunning = false;
            StartCoroutine(Spawn());
        }
        if (howManyReachEnd == howManySpawn)
        {
            if (endGate.good >= (howManySpawn - 3))
            {
                myTicket.PuzzleCompletedSucesfull = true;
                PuzzleCleanUp();
            }
            else
            {
                myTicket.PuzzleCompletedFailed = true;
                PuzzleCleanUp();
            }
        }
    }
    public IEnumerator Spawn()
    {
        for (int i = 0; i < howManySpawn; i++)
        {        
            GameObject arrowSpawnee = Instantiate(arrow, spawnTransform);
            RhythmPuzzleElement rhythmPuzzleElement = arrowSpawnee.GetComponent<RhythmPuzzleElement>();
            rhythmPuzzleElement.type = Random.Range(1, 5);
            rhythmPuzzleElement.moveSpeed = arrowSpeed;
            yield return new WaitForSeconds(timeBeetwennSpawn);
        }
    }

    private void Init()
    {
        if(difficulty == 1)
        {
            myTicket.PuzzleCompletedFailed = false;
            myTicket.PuzzleCompletedSucesfull = false;
            howManyReachEnd = 0;
            timeBeetwennSpawn = 5;
            arrowSpeed = 75;
            howManySpawn = 5;
        }
    }

    public void PuzzleCleanUp()
    {
        didInit = false;
        this.gameObject.SetActive(false);
    }
}
