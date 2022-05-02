using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmPuzzleElement : MonoBehaviour
{
    public bool Correct = false;
    public bool Bad = false;
    public float moveSpeed;
    public int type;    
    
    // Start is called before the first frame update
    void Start()
    {
        if(type ==1)
        {
            Transform arrow = this.transform.GetChild(0);
            arrow.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(type ==2)
        {
            Transform arrow = this.transform.GetChild(0);
            arrow.transform.eulerAngles = new Vector3(0, 0, 270);
        }
        else if (type == 3)
        {
            Transform arrow = this.transform.GetChild(0);
            arrow.transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (type == 4)
        {
            Transform arrow = this.transform.GetChild(0);
            arrow.transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
    }
}
