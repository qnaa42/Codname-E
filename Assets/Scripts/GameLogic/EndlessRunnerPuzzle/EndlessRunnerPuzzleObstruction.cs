using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRunnerPuzzleObstruction : MonoBehaviour
{
    public float moveSpeed;
    
    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RunnerPuzzlePlayerToken"))
        {
            EndlessRunnerPuzzleManager puzzleManager = this.GetComponentInParent<EndlessRunnerPuzzleManager>();
            moveSpeed = 0;
            puzzleManager.StopAllCoroutines();
            for (int i = 0; i < puzzleManager.spawnedObstruction.Count; i++)
            {
                if (puzzleManager.spawnedObstruction[i] !=null)
                {                    
                    EndlessRunnerPuzzleObstruction obstructions = puzzleManager.spawnedObstruction[i].GetComponent<EndlessRunnerPuzzleObstruction>();
                    obstructions.moveSpeed = 0;
                    EndlessRunnerPuzzlePlayerToken playerToken = other.GetComponent<EndlessRunnerPuzzlePlayerToken>();
                    playerToken.moveSpeed = 0;
                }
            }
            puzzleManager.PuzzleCleanUp();
            puzzleManager.myTicket.PuzzleCompletedFailed = true;
        }
        if (other.CompareTag("RunnerPuzzleEndBarrier"))
        {
            GameObject parent = this.transform.parent.gameObject;
            EndlessRunnerPuzzleManager puzzleManager = parent.GetComponentInParent<EndlessRunnerPuzzleManager>();
            puzzleManager.spawnedObstruction.Remove(other.gameObject);
            puzzleManager.howManySucesses++;
            Destroy(this.gameObject);
        }
    }
}
