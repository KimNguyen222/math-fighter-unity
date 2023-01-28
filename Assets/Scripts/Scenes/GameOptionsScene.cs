using MathFighter.Math.Questions;
using MathFighter.GamePlay;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

namespace MathFighter.Scenes
{
    public class GameOptionsScene : MonoBehaviour
    {


        //[SerializeField]
        //public static Player player1;
        //public static Player player2;

        //********* Game Option  ************//
        [SerializeField]
        private GameObject GameOption;
        [SerializeField]
        private TMP_Text _challengers;
        [SerializeField]
        private TMP_Text _difficulty;
        [SerializeField]
        private TMP_Text _location;
        [SerializeField]
        private TMP_Text _energy;
        [SerializeField]
        private TMP_Text _grades;
        [SerializeField]
        private Sprite checkboxChecked;
        [SerializeField]
        private Sprite checkboxEmpty;

        public List<Image> CheckBoxes = new List<Image>();

        private int optionIndex = 0;
        private int challengersIndex = 0;
        private int difficultyIndex = 0;
        private int locationIndex = 0;
        private int energyIndex = 0;
        private int gradesOptionIndex = 0;

        private string[] challengersOptions = { "Yes", "No" };
        private string[] difficultyOptions = { "Easy", "Medium", "Hard" };
        private string location = "Location";
        private int[] energyOptions = { 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200 };
        private string[] gradesOptions = { "All", "Custom", "Easy", "Medium", "Hard" };
        private bool[] grades = { true, true, true, true, true, true, true, true, true, true, true, true, true };
        private bool[] customGrades = { true, true, true, true, true, true, true, true, true, true, true, true, true };



        private const int GRADE_COUNT = 13;
        //**********************************//

        //****** Player Selection  *******//
        [SerializeField]
        private GameObject PlayerSelection;
        [SerializeField]
        private GameObject P1Selector;
        [SerializeField]
        private GameObject P2Selector;
        [SerializeField]
        private GameObject P1P2Selector;
        [SerializeField]
        private Canvas canvas;
       
        private GameObject player1;
        private GameObject player2;


        public List<GameObject> PlayerPrefabs;

        private int playerNum1;             // Number of Character who is selected by Player1
        private int playerNum2;             // Number of Character who is selected by Player1

        private float[] z_angles = {0f, 288.0f, 218.0f, 142.0f, 73.0f};   // Z angles of Selection bars
                                                                          //private string[] PlayerNames = { "Yurl", "", "TheEthernalBlaDc", "", "" };
                                                                          // Start is called before the first frame update
        private bool isGameOption = false;
        private bool isSelectPlayer = false;

        public GameObject GamePlaySettings;

        Animator animator1;
        Animator animator2;
        //**********************************//
        void Start()
        {
            playerNum1 = 0;
            playerNum2 = 0;
            //RotateSelection();
            ShowOptions();
        }

        // Update is called once per frame
        void Update()
        {
            if (isSelectPlayer)
                return;
            // Rotate Selection bar by keycode
            // A,S,D,W for Player1; LeftArrow, RightArrow, UpArrow, DownArrow for Player2

            //if (!isGameOption) // GameOption
            //{
            //    if (Input.GetKeyDown(KeyCode.LeftArrow))
            //    {
            //        if (optionIndex == 0)        // Challengers
            //        {
            //            challengersIndex = 1 - challengersIndex;
            //        }
            //        else if (optionIndex == 1)   // Difficulty
            //        {
            //            difficultyIndex--;
            //            if (difficultyIndex == -1) difficultyIndex = 2;
            //        }
            //        //else if (optionIndex == 2)   // Location
            //        //{

            //        //}
            //        else if (optionIndex == 3)   // Energy
            //        {
            //            energyIndex--;
            //            if (energyIndex == -1) energyIndex = 15;
            //        }
            //        else if (optionIndex == 4)   // Grades
            //        {
            //            gradesOptionIndex--;
            //            if (gradesOptionIndex == -1) gradesOptionIndex = 4;

            //            switch (gradesOptionIndex)
            //            {
            //                case 0:
            //                    grades = new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, true };
            //                    break;
            //                case 1:
            //                    for (int i = 0; i < GRADE_COUNT; i++)
            //                        grades[i] = customGrades[i];
            //                    break;
            //                case 2:
            //                    grades = new bool[] { true, true, true, true, false, false, false, false, false, false, false, false, false };
            //                    break;
            //                case 3:
            //                    grades = new bool[] { false, false, false, false, true, true, true, true, false, false, false, false, false };
            //                    break;
            //                case 4:
            //                    grades = new bool[] { false, false, false, false, false, false, false, false, false, true, true, true, false };
            //                    break;
            //            }
            //            UpdateCheckBoxes();
            //        }

            //    }
            //    else if (Input.GetKeyDown(KeyCode.RightArrow))
            //    {
            //        if (optionIndex == 0)        // Challengers
            //        {
            //            challengersIndex = 1 - challengersIndex;
            //        }
            //        else if (optionIndex == 1)   // Difficulty
            //        {
            //            difficultyIndex++;
            //            if (difficultyIndex == 3) difficultyIndex = 0;
            //        }
            //        //else if (optionIndex == 2)   // Location
            //        //{

            //        //}
            //        else if (optionIndex == 3)   // Energy
            //        {
            //            energyIndex++;
            //            if (energyIndex == 16) energyIndex = 0;
            //        }
            //        else if (optionIndex == 4)   // Grades
            //        {
            //            gradesOptionIndex++;
            //            if (gradesOptionIndex == 5) gradesOptionIndex = 0;

            //            switch(gradesOptionIndex)
            //            {
            //                case 0:
            //                    grades = new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, true};
            //                    break;
            //                case 1:
            //                    for (int i = 0; i < GRADE_COUNT; i ++)
            //                        grades[i] = customGrades[i];
            //                    break;
            //                case 2:
            //                    grades = new bool[] { true, true, true, true, false, false, false, false, false, false, false, false, false };
            //                    break;
            //                case 3:
            //                    grades = new bool[] { false, false, false, false, true, true, true, true, false, false, false, false, false };
            //                    break;
            //                case 4:
            //                    grades = new bool[] { false, false, false, false, false, false, false, false, false, true, true, true, false };
            //                    break;
            //            }
            //            UpdateCheckBoxes();
            //        }
            //    }
            //    else if (Input.GetKeyDown(KeyCode.UpArrow))
            //    {
            //        optionIndex--;
            //        if (optionIndex == -1) optionIndex = 4;
            //        else if (optionIndex == 2) optionIndex = 1;
            //    }
            //    else if (Input.GetKeyDown(KeyCode.DownArrow))
            //    {
            //        optionIndex++;
            //        if (optionIndex == 5) optionIndex = 0;
            //        else if (optionIndex == 2) optionIndex = 3;
            //    }
            //    ShowOptions();
            //}
            //else               // Player Selection after Game Option
            //{
            //    // Increase the playerNum1. KeyCode: A or S
            //    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            //    {
            //        playerNum1++;
            //        if (playerNum1 > 4)
            //            playerNum1 = 0;
            //        RotateSelection();
            //    }

            //    // Decrease the playerNum1. KeyCode: D or W
            //    else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            //    {
            //        playerNum1--;
            //        if (playerNum1 < 0)
            //            playerNum1 = 4;
            //        RotateSelection();
            //    }
            //}
        }

        public void OnClickOptions(int index)
        {
            if (!isGameOption)
            {
                optionIndex = index;
                if (optionIndex == 0)        // Challengers
                {
                    challengersIndex = 1 - challengersIndex;
                }
                else if (optionIndex == 1)   // Difficulty
                {
                    difficultyIndex++;
                    if (difficultyIndex == 3) difficultyIndex = 0;
                }
                //else if (optionIndex == 2)   // Location
                //{

                //}
                else if (optionIndex == 3)   // Energy
                {
                    energyIndex++;
                    if (energyIndex == 16) energyIndex = 0;
                }
                else if (optionIndex == 4)   // Grades
                {
                    gradesOptionIndex++;
                    if (gradesOptionIndex == 5) gradesOptionIndex = 0;

                    switch (gradesOptionIndex)
                    {
                        case 0:
                            grades = new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, true };
                            break;
                        case 1:
                            for (int i = 0; i < GRADE_COUNT; i++)
                                grades[i] = customGrades[i];
                            break;
                        case 2:
                            grades = new bool[] { true, true, true, true, false, false, false, false, false, false, false, false, false };
                            break;
                        case 3:
                            grades = new bool[] { false, false, false, false, true, true, true, true, false, false, false, false, false };
                            break;
                        case 4:
                            grades = new bool[] { false, false, false, false, false, false, false, false, false, true, true, true, false };
                            break;
                    }
                    UpdateCheckBoxes();
                }
                ShowOptions();
            }
        }
        private void ShowOptions()
        {
            _challengers.text = "Challengers: " + challengersOptions[challengersIndex];
            _difficulty.text = "Difficulty: " + difficultyOptions[difficultyIndex];
            _location.text = "Location: ???";
            _energy.text = "Energy: " + energyOptions[energyIndex];
            _grades.text = "Grades: " + gradesOptions[gradesOptionIndex];

            _challengers.transform.localScale = new Vector3(1f, 1f, 1f);
            _difficulty.transform.localScale = new Vector3(1f, 1f, 1f);
            _location.transform.localScale = new Vector3(1f, 1f, 1f);
            _energy.transform.localScale = new Vector3(1f, 1f, 1f);
            _grades.transform.localScale = new Vector3(1f, 1f, 1f);

            if (optionIndex == 0)
            {
                _challengers.transform.localScale = new Vector3(1.5f, 1.3f, 1f);
            }
            else if (optionIndex == 1)
            {
                _difficulty.transform.localScale = new Vector3(1.5f, 1.3f, 1f);
            }
            else if (optionIndex == 3)
            {
                _energy.transform.localScale = new Vector3(1.5f, 1.3f, 1f);
            }
            else if (optionIndex == 4)
            {
                _grades.transform.localScale = new Vector3(1.5f, 1.3f, 1f);
            }
            UpdateCheckBoxes();

        }
        private void UpdateCheckBoxes()
        {
            for (int i = 0; i < GRADE_COUNT; i++)
            {
                if (grades[i])
                    CheckBoxes[i].GetComponent<Image>().sprite = checkboxChecked;
                else
                    CheckBoxes[i].GetComponent<Image>().sprite = checkboxEmpty;
            }
        }
        public void ToggleGrade(int gradeNum)
        {
            grades[gradeNum] = !grades[gradeNum];
            for (int i = 0; i < GRADE_COUNT; i++)
                customGrades[i] = grades[i];
            gradesOptionIndex = 1; 
            _grades.text = "Grades: " + gradesOptions[gradesOptionIndex];
            UpdateCheckBoxes();
        }

        public void OnClickPlayer(int playerIndex)
        {
            playerNum1 = playerIndex;
            if (isGameOption)
                RotateSelection();
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
            if (!isGameOption)
            {
                isGameOption = true;
                bool[] grades = { true, true };
                GameOption.SetActive(false);
                PlayerSelection.SetActive(true);
                RotateSelection();
            }
            else
            {
                StartCoroutine(SelectPlayer());
                GamePlaySettings settings = GamePlaySettings.GetComponent<GamePlaySettings>();

                if (challengersOptions[challengersIndex] == "Yes")
                    settings.AllowChallengers = true;
                else
                    settings.AllowChallengers = false;

                switch (difficultyOptions[difficultyIndex])
                {
                    case "Easy":
                        settings.DifficultyLevel = 0;
                        break;
                    case "Medium":
                        settings.DifficultyLevel = 1;
                        break;
                    case "Hard":
                        settings.DifficultyLevel = 2;
                        break;
                }

                settings.EnergyBar = energyOptions[energyIndex];

                switch (gradesOptions[gradesOptionIndex])
                {
                    case "All":
                        settings.MathGradesSetting = GamePlay.GamePlaySettings.EnumMathGradesSetting.All;
                        break;
                    case "Custom":
                        settings.MathGradesSetting = GamePlay.GamePlaySettings.EnumMathGradesSetting.Custom;
                        break;
                    case "Easy":
                        settings.MathGradesSetting = GamePlay.GamePlaySettings.EnumMathGradesSetting.Easy;
                        break;
                    case "Medium":
                        settings.MathGradesSetting = GamePlay.GamePlaySettings.EnumMathGradesSetting.Medium;
                        break;
                    case "Hard":
                        settings.MathGradesSetting = GamePlay.GamePlaySettings.EnumMathGradesSetting.Hard;
                        break;
                }

                settings.EnableMathGrades(grades);
                //settings.AddPlayer(player1.GetComponent<Player>());
                //settings.AddPlayer(player2.GetComponent<Player>());
                settings.playerNum1 = playerNum1;
                settings.playerNum2 = playerNum2;
                settings.playerName1 = "TheEternalBlaDe";
                settings.playerName2 = player2.GetComponent<Player>().playerName;
                DontDestroyOnLoad(GamePlaySettings.GetComponent<GamePlaySettings>());
                //DontDestroyOnLoad(settings.Players);
                //SceneManager.LoadScene("VSScene");
            }
        }

        private IEnumerator SelectPlayer()
        {
            if (!isSelectPlayer)
            {
                Color tempcolor = PlayerPrefabs[playerNum1].GetComponent<SpriteRenderer>().color;
                tempcolor.a = 0f;
                PlayerPrefabs[playerNum1].GetComponent<SpriteRenderer>().color = tempcolor;
                PlayerPrefabs[playerNum2].GetComponent<SpriteRenderer>().color = tempcolor;

                player1 = Instantiate(PlayerPrefabs[playerNum1], new Vector3(0, 0, 0), Quaternion.Euler(0f, 180f, 0f));
                player2 = Instantiate(PlayerPrefabs[playerNum2], new Vector3(0, 0, 0), Quaternion.identity);

                //player1.name = "Player1";
                //player2.name = "Player2";

                tempcolor.a = 1f;
                PlayerPrefabs[playerNum1].GetComponent<SpriteRenderer>().color = tempcolor;
                PlayerPrefabs[playerNum2].GetComponent<SpriteRenderer>().color = tempcolor;



                //player1.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
                //player2.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);


                //player1.transform.SetParent(canvas.transform);
                player1.transform.position = new Vector3(-5.2f, -3.25f, 0f);
                //player2.transform.SetParent(canvas.transform);
                player2.transform.position = new Vector3(5.33f, -3.25f, 0f);


                animator1 = player1.GetComponent<Animator>();
                animator2 = player2.GetComponent<Animator>();

                animator1.SetTrigger("appearance");
                yield return new WaitForSeconds(1);
                animator2.SetTrigger("appearance");
                yield return new WaitForSeconds(2);
              
                isSelectPlayer = true;

                //yield return new WaitForSeconds(2);
                SceneManager.LoadScene("VSScene");
            }
        }

        private Sprite LoadSprite(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (System.IO.File.Exists(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(900, 900, TextureFormat.RGB24, false);
                texture.filterMode = FilterMode.Trilinear;
                texture.LoadImage(bytes);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.0f), 1.0f);

                // You should return the sprite here!
                return sprite;
            }
            return null;
        }

        private IEnumerator Apperance1()
        {
            // Play the animation for getting O in
            animator1.SetTrigger("appearance");

            yield return new WaitUntil(() => animator1.GetCurrentAnimatorStateInfo(0).normalizedTime <= 3.0f);


            // Move this object somewhere off the screen

        }
        private IEnumerator Apperance2()
        {
            // Play the animation for getting O in
            animator2.SetTrigger("appearance");


            yield return new WaitUntil(() => animator2.GetCurrentAnimatorStateInfo(0).normalizedTime <= 3.0f);

            // Move this object somewhere off the screen

        }
    }
}