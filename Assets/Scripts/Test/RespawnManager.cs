using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public List<ClonePod> listOfClones = new List<ClonePod>();

    private void Start()
    {        
        for (int i = 0; i < this.transform.childCount; i++)
        {            
            listOfClones.Add(transform.GetChild(i).GetComponent<ClonePod>());
            listOfClones[i].cloneAlive = true;
            listOfClones[i].cloneDead = false;
            listOfClones[i].cloneUsed = false;
        }       
    }

    public void UpdatePods()
    {
        listOfClones[GameManager.instance.userManager.GetClones() -1].cloneAlive = false;
        listOfClones[GameManager.instance.userManager.GetClones()-1].cloneDead = false;
        listOfClones[GameManager.instance.userManager.GetClones()-1].cloneUsed = true;        
    }

    public void KillAllClones()
    {
        GameManager.instance.userManager.SetClones(0);
        for (int i = 0; i < this.transform.childCount; i++)
        {
            listOfClones[i].cloneAlive = false;
            listOfClones[i].cloneDead = true;
            listOfClones[i].cloneUsed = false;
        }
    }
}
