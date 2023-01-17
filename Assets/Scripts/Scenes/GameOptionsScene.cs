using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private GameObject player1;
        [SerializeField]
        private GameObject player2;

        public List<GameObject> PlayerPrefabs;

        //[SerializeField]
        //public static Player player1;
        //public static Player player2;

        private int playerNum1;             // Number of Character who is selected by Player1
        private int playerNum2;             // Number of Character who is selected by Player1

        private float[] z_angles = {73.0f, 142.0f, 218.0f, 288.0f, 0f};   // Z angles of Selection bars
                                                                          //private string[] PlayerNames = { "Yurl", "", "TheEthernalBlaDc", "", "" };
                                                                          // Start is called before the first frame update
        private bool isSelectPlayer = false;

        Animator animator1;
        Animator animator2;

        void Start()
        {
            playerNum1 = 4;
            playerNum2 = 1;
            RotateSelection();
        }

        // Update is called once per frame
        void Update()
        {
            if (isSelectPlayer)
                return;
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