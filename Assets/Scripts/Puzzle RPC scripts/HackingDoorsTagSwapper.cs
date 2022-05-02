using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingDoorsTagSwapper : MonoBehaviour
{
    public bool HackingToolInteraction;
    public bool FixingToolInteraction;
    
    public string readableTag;
    public string puzzleTag;

    // Update is called once per frame
    void Update()
    {
        var characterController = GameManager.instance.player.GetComponent<MyCharacterController>();
        if (HackingToolInteraction == true)
        {
            if (characterController.hasHackingTool)
            {
                this.gameObject.tag = puzzleTag;
            }
            else if (!characterController.hasHackingTool)
            {
                this.gameObject.tag = readableTag;
            }
        }
        else if (FixingToolInteraction == true)
        {
            if (characterController.hasFixingTool)
            {
                this.gameObject.tag = puzzleTag;
            }
            else if (!characterController.hasFixingTool)
            {
                this.gameObject.tag = readableTag;
            }
        }
        else
        {
            this.gameObject.tag = "Untagged";
        }
    }
}
