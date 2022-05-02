using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.Scripts
{
    public class UiManager : MonoBehaviour
    {
        

        public Text idText;
        public Text nameText;
        public Text scoreText;
        public Text healthText;
        public Text hungerText;


        public Text stateText;

        public GameObject popUpMenu;

        public GameObject inventoryInfo;


        public GameObject popUpText;
        public Text popUpMenuText;

        public GameObject inventory;
        public bool inventoryOn;


        // Start is called before the first frame update
        void Start()
        {
            popUpMenu.SetActive(false);
            popUpText.SetActive(false);
            inventoryInfo.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            idText.text = "Id : 0";
            nameText.text = "Name :" + GameManager.instance.userManager.GetName();
            scoreText.text = "Score :" + GameManager.instance.userManager.GetScore();
            healthText.text = "Health :" + GameManager.instance.userManager.GetHealth();
            hungerText.text = "Clones Remain :" + GameManager.instance.userManager.GetClones();

            stateText.text = "State is :" + GameManager.instance.currentGameState.ToString();



            Pause();
            MenuInput();
        }

        private void MenuInput()
        {
            if(Input.GetKeyDown(KeyCode.Tab) && !inventoryOn)
            {
                inventory.SetActive(true);
                inventoryOn = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            if (Input.GetKeyDown(KeyCode.Tab) && inventoryOn)
            {
                inventory.SetActive(false);
                inventoryOn = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }
        }

        public void ExitGame()
        {
            GameManager.instance.SetTargetState(Game.State.gameEnded);
            UnPausing();
        }
        public void RestartGame()
        {

            GameManager.instance.SetTargetState(Game.State.restarting);
            UnPausing();
        }
        public void Pause()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.currentGameState != Game.State.gamePausing)
            {
                GameManager.instance.SetTargetState(Game.State.gamePausing);
                popUpMenu.SetActive(true);
            }
        }

        internal void DisplayTextFromObject(string Text)
        {
            popUpText.SetActive(true);
            popUpMenuText.text = Text;
            GameManager.instance.SetTargetState(Game.State.gamePausing);
        }

        public void UnPausing()
        {
                GameManager.instance.SetTargetState(Game.State.gameUnPausing);
                popUpMenu.SetActive(false);
                popUpText.SetActive(false);
        }
    }
}
