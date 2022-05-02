using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePod : MonoBehaviour
{
    public bool cloneAlive;
    public bool cloneDead;
    public bool cloneUsed;

    public GameObject lightUp;
    public GameObject lightDown;
    public GameObject particleEmitter;
    public GameObject ellisePrefab;

    private void Update()
    {
        Light upLight = lightUp.GetComponent<Light>();
        Light downLight = lightDown.GetComponent<Light>();
        if (cloneAlive)
        {
            upLight.color = new Color32(30, 221, 43, 255);
            downLight.color = new Color32(30, 221, 43, 255);
            particleEmitter.SetActive(true);
            ellisePrefab.SetActive(true);
        }
        else if (cloneDead)
        {
            upLight.color = new Color32(221,30,41,255);
            downLight.color = new Color32(221, 30, 41, 255);
            particleEmitter.SetActive(false);
            ellisePrefab.SetActive(true);
        }
        else if (cloneUsed)
        {
            lightUp.SetActive(false);
            lightDown.SetActive(false);
            particleEmitter.SetActive(false);
            ellisePrefab.SetActive(false);
        }
    }
}
