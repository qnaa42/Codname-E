using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

using UnityEngine;
using Random = UnityEngine.Random;

public class ToolTag : MonoBehaviour
{
    public string whatTool;

    public void pickUpTool()
    {
        if (whatTool == "Fixing")
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().hasFixingTool = true;
            GameManager.instance.player.GetComponent<MyCharacterController>().fixingToolAudio.Play();
        }
        else if (whatTool == "Hacking")
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().hasHackingTool = true;
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().wakeUpAudio.Play();
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().defendClonesAudio.Play();
        }

        if (Input.GetKeyUp((KeyCode.Alpha3)))
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().goodJobAudio.Play();
        }
        
        if (Input.GetKeyUp((KeyCode.Alpha4)))
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().breakOfcontactAudio.Play();
        }
        
        if (Input.GetKeyUp((KeyCode.Alpha5)))
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().yourShipIsBroken.Play();
        }
        
        //AI
        if (Input.GetKeyUp((KeyCode.Alpha6)))
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().afterBeatingCloneBay.Play();
        }
        if (Input.GetKeyUp((KeyCode.Alpha7)))
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().afterTaskDone.Play();
        }
        if (Input.GetKeyUp((KeyCode.Alpha8)))
        {
            int x = Random.Range(1, 10);
            GameManager.instance.player.GetComponent<MyCharacterController>().AiIdle[x].Play();
        }
        if (Input.GetKeyUp((KeyCode.Alpha9)))
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().droneBayMonologue.Play();
        }
        if (Input.GetKeyUp((KeyCode.Alpha0)))
        {
            GameManager.instance.player.GetComponent<MyCharacterController>().firstCamera.Play();
        }
    }
}
