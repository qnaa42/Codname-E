using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRunnerPuzzlePlayerToken : MonoBehaviour
{
    public float horizontalInput;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("RotationVertical");
        transform.Translate(Vector3.up * horizontalInput * moveSpeed);
        if (transform.localPosition.y >= 280)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 280, transform.localPosition.z);
        }
        if (transform.localPosition.y <= -280)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -280, transform.localPosition.z);
        }
    }
}
