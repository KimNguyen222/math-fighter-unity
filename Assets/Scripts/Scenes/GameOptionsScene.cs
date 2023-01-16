using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MathFighter.Scenes
{
    public class GameOptionsScene : MonoBehaviour
    {
        [SerializeField]
        private GameObject P1Selector;
        [SerializeField]
        private GameObject P2Selector;
        [SerializeField]
        private GameObject P1P2Selector;

        private int playerNum1;             // Number of Character who is selected by Player1
        private int playerNum2;             // Number of Character who is selected by Player1

        private float[] z_angles = { 0f, 73.0f, 142.0f, 218.0f, 288.0f };   // Z angles of Selection bars
                                                                            //private string[] PlayerNames = { "Yurl", "", "TheEthernalBlaDc", "", "" };
                                                                            // Start is called before the first frame update
        void Start()
        {
            playerNum1 = 0;
            playerNum2 = 0;
        }

        // Update is called once per frame
        void Update()
        {
            // Rotate Selection bar by keycode
            // A,S,D,W for Player1; LeftArrow, RightArrow, UpArrow, DownArrow for Player2

            // Increase the playerNum1. KeyCode: A or S
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
            {
                playerNum1++;
                if (playerNum1 > 4)
                    playerNum1 = 0;
                RotateSelection();
            }

            // Decrease the playerNum1. KeyCode: D or W
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))
            {
                playerNum1--;
                if (playerNum1 < 0)
                    playerNum1 = 4;
                RotateSelection();
            }

            // Increase the playerNum2. KeyCode: LeftArrow or RightArrow
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                playerNum2++;
                if (playerNum2 > 4)
                    playerNum2 = 0;
                RotateSelection();
            }

            // Decrease the playerNum2. KeyCode: DownArrow or UpArrow
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerNum2--;
                if (playerNum2 < 0)
                    playerNum2 = 4;
                RotateSelection();
            }


        }

        private void RotateSelection()
        {
            if (playerNum1 == playerNum2)
            {
                P1P2Selector.SetActive(true);
                P1Selector.SetActive(false);
                P2Selector.SetActive(false);

                P1P2Selector.transform.rotation = Quaternion.Euler(0f, 0f, z_angles[playerNum1]);
                P1P2Selector.transform.Find("Lobby").transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                P1P2Selector.SetActive(false);
                P1Selector.SetActive(true);
                P2Selector.SetActive(true);

                P1Selector.transform.rotation = Quaternion.Euler(0f, 0f, z_angles[playerNum1]);
                P1Selector.transform.Find("Lobby").transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                P2Selector.transform.rotation = Quaternion.Euler(0f, 0f, z_angles[playerNum2]);
                P2Selector.transform.Find("Lobby").transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        public void OnContinueButtonClicked()
        {
            ;
        }
    }
}