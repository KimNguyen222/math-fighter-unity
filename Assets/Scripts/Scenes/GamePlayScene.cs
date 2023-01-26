using MathFighter.GamePlay;
using MathFighter.Math.Categories;
using MathFighter.Math.Questions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
namespace MathFighter.Scenes
{
    public class GamePlayScene : MonoBehaviour
    {
        [SerializeField]
        private Image Background;
        [SerializeField]
        private SpriteRenderer Avatar1;
        [SerializeField]
        private SpriteRenderer Avatar2;
        [SerializeField]
        private TMP_Text PlayerName1;
        [SerializeField]
        private TMP_Text PlayerName2;
        [SerializeField]
        private SpriteRenderer GetReady;
        [SerializeField]
        private GameObject SpotlightUI;
        [SerializeField]
        private GameObject QuestionUI;
        [SerializeField]
        private TMP_Text QuestionTitle;
        [SerializeField]
        private TMP_Text QuestionNum;



        public List<GameObject> SpotlightGreenRedPrefab;
        public List<Sprite> _backgrounds;
        public List<Sprite> _avatars;
        public List<GameObject> PlayerPrefabs;

        private List<GameObject> spotlights;
        private GamePlaySettings settings;

        private GameObject player1;
        private GameObject player2;
        private Animator animGetReady;
        private bool isReady = false;
        private bool isSpotlightGreeRed = false;
        private bool gameplay = false;
        private bool isQuestionShown = false;

        // Start is called before the first frame update
        void Start()
        {
            settings = GameObject.Find("GamePlaySettings").GetComponent<GamePlaySettings>();
            spotlights = new List<GameObject>();
            player1 = Instantiate(PlayerPrefabs[settings.playerNum1], new Vector3(-5.5f, 0, 0), Quaternion.Euler(0f, 180f, 0f));
            player2 = Instantiate(PlayerPrefabs[settings.playerNum2], new Vector3(5.5f, 0, 0), Quaternion.identity);
            SetEnvironment();
            GetReady.gameObject.SetActive(true);
            animGetReady = GetReady.GetComponent<Animator>();
            MathQuestion.LoadData();
            //StartCoroutine(AppearGetReady());
            //StartCoroutine(DisappearGetReady());
        }
        // Update is called once per frame
        void Update()
        {
            //Debug.Log(isReady);
            if (!isReady)
                StartCoroutine(AppearGetReady());
            if (gameplay)
            {
                GamePlay();
            }

            if (!isSpotlightGreeRed)
                DrawSpotlightGreenRed(1);
            else
                RemoveSpotlightGreenRed();
        }

        private void GamePlay()
        {
            if (!isQuestionShown)
                StartCoroutine(ShowQuestionTitle());

            if (isQuestionShown)
            {
                settings.CreateNewDealer();
                QuestionContent question = settings.Dealer.GetQuestion();
                Debug.Log(question.LevelName);
                isQuestionShown = false;
            }
        }

        private IEnumerator AppearGetReady()
        {
            yield return new WaitForSeconds(1);
            animGetReady.SetTrigger("Appear");
            yield return new WaitForSeconds(3);
            animGetReady.SetTrigger("Disappear");
            yield return new WaitForSeconds(3);
            isReady = true;
            gameplay = true;
        }
        private IEnumerator ShowQuestionTitle()
        {
            QuestionUI.SetActive(true);
            yield return new WaitForSeconds(2);
            QuestionUI.SetActive(false);
            isQuestionShown = true;
        }
        private void DrawSpotlightGreenRed(int mode)
        {
            if (spotlights.Count > 7)
            {
                return;
            }
            GameObject spotlight = Instantiate(SpotlightGreenRedPrefab[mode], new Vector3(0, 0, 0), Quaternion.Euler(0f, 180f, 0f));
            spotlight.transform.SetParent(SpotlightUI.transform);
            spotlight.transform.localPosition = new Vector3(0, -550, 0);
            spotlight.transform.localScale = new Vector3(1, 1, 0);
            spotlight.transform.localRotation = Quaternion.identity;
            spotlight.transform.Rotate(0, 0, 10.8f * spotlights.Count, Space.Self);
            spotlights.Add(spotlight);


            //SpotlightGreenRed.AddComponent<MeshFilter>();
            //SpotlightGreenRed.AddComponent<MeshRenderer>();
            //Mesh mesh = SpotlightGreenRed.GetComponent<MeshFilter>().mesh;
            //mesh.Clear();
            //mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1000, 0), new Vector3(1000, 1000, 0) };
            //mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1000), new Vector2(1000, 1000) };
            //mesh.triangles = new int[] { 0, 1, 2 };

            //Color[] colors = Enumerable.Repeat(Color.green, mesh.vertices.Length).ToArray();
            //mesh.colors = colors;
            //Debug.Log("Draw");

        }

        private void RemoveSpotlightGreenRed()
        {
            if (spotlights.Count == 0) 
            {
                return;
            }
            GameObject spotlight = spotlights[spotlights.Count - 1];
            spotlights.Remove(spotlight);
            Destroy(spotlight);
        }
        //private IEnumerator DisappearGetReady()
        //{
        //}
        private void SetEnvironment()
        {
            Background.GetComponent<Image>().sprite = _backgrounds[settings.playerNum2];
            Avatar1.sprite = _avatars[settings.playerNum1];
            Avatar2.sprite = _avatars[settings.playerNum2];
            PlayerName1.text = settings.playerName1;
            PlayerName2.text = settings.playerName2;
            player1.GetComponent<Player>().playerName = settings.playerName1;
            player2.GetComponent<Player>().playerName = settings.playerName2;
        }

    }
}