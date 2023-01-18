using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace MathFighter.Scenes
{
    public class GameOptionsScene : MonoBehaviour
    {


        //[SerializeField]
        //public static Player player1;
        //public static Player player2;

        //********* Game Option  ************//
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

        private int optionIndex = 0;
        private int challengersIndex = 0;
        private int difficultyIndex = 0;
        private int locationIndex = 0;
        private int energyIndex = 0;
        private int gradesIndex = 0;
        private string[] challengers = { "Yes", "No" };
        private string[] difficulty = { "Easy", "Medium", "Hard" };
        private string location = "Location";
        private int[] energy = { 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200 };
        private string[] grades = { "All", "Custom", "Easy", "Medium", "Hard" };

        //**********************************//

        //****** Player Selection  *******//
        [SerializeField]
        private GameObject P1Selector;
        [SerializeField]
        private GameObject P2Selector;
        [SerializeField]
        private GameObject P1P2Selector;
        [SerializeField]
        private Canvas canvas;
        //[SerializeField]
        //private GameObject player1;
        //[SerializeField]
        //private GameObject player2;

        public List<GameObject> PlayerPrefabs;

        private int playerNum1;             // Number of Character who is selected by Player1
        private int playerNum2;             // Number of Character who is selected by Player1

        private float[] z_angles = {73.0f, 142.0f, 218.0f, 288.0f, 0f};   // Z angles of Selection bars
                                                                          //private string[] PlayerNames = { "Yurl", "", "TheEthernalBlaDc", "", "" };
                                                                          // Start is called before the first frame update
        private bool isGameOption = false;
        private bool isSelectPlayer = false;

        Animator animator1;
        Animator animator2;
        //**********************************//
        void Start()
        {
            playerNum1 = 4;
            playerNum2 = 1;
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

            if (!isGameOption) // GameOption
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (optionIndex == 0)        // Challengers
                    {
                        challengersIndex = 1 - challengersIndex;
                    }
                    else if (optionIndex == 1)   // Difficulty
                    {
                        difficultyIndex--;
                        if (difficultyIndex == -1) difficultyIndex = 2;
                    }
                    //else if (optionIndex == 2)   // Location
                    //{

                    //}
                    else if (optionIndex == 3)   // Energy
                    {
                        energyIndex--;
                        if (energyIndex == -1) energyIndex = 15;
                    }
                    else if (optionIndex == 4)   // Grades
                    {
                        gradesIndex--;
                        if (gradesIndex == -1) gradesIndex = 4;
                    }

                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
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
                        gradesIndex++;
                        if (gradesIndex == 5) gradesIndex = 0;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    optionIndex--;
                    if (optionIndex == -1) optionIndex = 4;
                    else if (optionIndex == 2) optionIndex = 1;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    optionIndex++;
                    if (optionIndex == 5) optionIndex = 0;
                    else if (optionIndex == 2) optionIndex = 3;
                }
                ShowOptions();
            }
            else               // Player Selection after Game Option
            {
                // Increase the playerNum1. KeyCode: A or S
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    playerNum1++;
                    if (playerNum1 > 4)
                        playerNum1 = 0;
                    RotateSelection();
                }

                // Decrease the playerNum1. KeyCode: D or W
                else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    playerNum1--;
                    if (playerNum1 < 0)
                        playerNum1 = 4;
                    RotateSelection();
                }
            }
        }
        private void ShowOptions()
        {
            _challengers.text = "Challengers: " + challengers[challengersIndex];
            _difficulty.text = "Difficulty: " + difficulty[difficultyIndex];
            _location.text = "Location: ???";
            _energy.text = "Energy: " + energy[energyIndex];
            _grades.text = "Grades: " + grades[gradesIndex];

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
            StartCoroutine(SelectPlayer());
            //SceneManager.LoadScene("VSScene");

        }

        private IEnumerator SelectPlayer()
        {
            if (playerNum1 != playerNum2 && !isSelectPlayer)
            {
                //SpriteRenderer spriteRenderer1 = PlayerPrefabs[playerNum1].GetComponent<SpriteRenderer>();
                //SpriteRenderer spriteRenderer2 = PlayerPrefabs[playerNum2].GetComponent<SpriteRenderer>();

                ////player1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(AssetDatabase.
                ////    GetAssetPath(spriteRenderer1.sprite).Replace("Assets/Resources", ""));
                ////player2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(AssetDatabase.
                ////    GetAssetPath(spriteRenderer2.sprite).Replace("Assets/Resources", "")); 
                ////Debug.Log("Sprite1: " + AssetDatabase.GetAssetPath(spriteRenderer1.sprite));


                ////player2.GetComponent<SpriteRenderer>().sprite = LoadSprite(AssetDatabase.GetAssetPath(spriteRenderer2.sprite));
                //Debug.Log("Sprite1: " + LoadSprite(AssetDatabase.GetAssetPath(spriteRenderer1.sprite)));
                //player1.GetComponent<Animator>().runtimeAnimatorController = PlayerPrefabs[playerNum1].GetComponent<Animator>().runtimeAnimatorController;
                //player2.GetComponent<Animator>().runtimeAnimatorController = PlayerPrefabs[playerNum2].GetComponent<Animator>().runtimeAnimatorController;
                

                Color tempcolor = PlayerPrefabs[playerNum1].GetComponent<SpriteRenderer>().color;
                tempcolor.a = 0f;
                PlayerPrefabs[playerNum1].GetComponent<SpriteRenderer>().color = tempcolor;
                PlayerPrefabs[playerNum2].GetComponent<SpriteRenderer>().color = tempcolor;

                GameObject player1 = Instantiate(PlayerPrefabs[playerNum1], new Vector3(0, 0, 0), Quaternion.Euler(0f, 180f, 0f));
                GameObject player2 = Instantiate(PlayerPrefabs[playerNum2], new Vector3(0, 0, 0), Quaternion.identity);

                player1.name = "Player1";
                player2.name = "Player2";
                tempcolor.a = 1f;
                PlayerPrefabs[playerNum1].GetComponent<SpriteRenderer>().color = tempcolor;
                PlayerPrefabs[playerNum2].GetComponent<SpriteRenderer>().color = tempcolor;



                //player1.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
                //player2.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);


                //player1.transform.SetParent(canvas.transform);
                player1.transform.position = new Vector3(-5.2f, -0.75f, 0f);
                //player2.transform.SetParent(canvas.transform);
                player2.transform.position = new Vector3(5.33f, -0.75f, 0f);


                animator1 = player1.GetComponent<Animator>();
                animator2 = player2.GetComponent<Animator>();

                animator1.SetTrigger("appearance");
                yield return new WaitForSeconds(1);
                animator2.SetTrigger("appearance");
                yield return new WaitForSeconds(2);
              
                isSelectPlayer = true;

                DontDestroyOnLoad(player1.transform);
                DontDestroyOnLoad(player2.transform);
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