using MathFighter.GamePlay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        public List<Sprite> _backgrounds;
        public List<Sprite> _avatars;
        public List<GameObject> PlayerPrefabs;


        private GamePlaySettings settings;

        private GameObject player1;
        private GameObject player2;
        private Animator animGetReady;
        private bool isReady = false;

        // Start is called before the first frame update
        void Start()
        {
            settings = GameObject.Find("GamePlaySettings").GetComponent<GamePlaySettings>();
            player1 = Instantiate(PlayerPrefabs[settings.playerNum1], new Vector3(-5.5f, 0, 0), Quaternion.Euler(0f, 180f, 0f));
            player2 = Instantiate(PlayerPrefabs[settings.playerNum2], new Vector3(5.5f, 0, 0), Quaternion.identity);
            SetEnvironment();
            GetReady.gameObject.SetActive(true);
            animGetReady = GetReady.GetComponent<Animator>();
            //StartCoroutine(AppearGetReady());
            //StartCoroutine(DisappearGetReady());
        }

        private IEnumerator AppearGetReady()
        {
            yield return new WaitForSeconds(1);
            animGetReady.SetTrigger("Appear");
            yield return new WaitForSeconds(3);
            animGetReady.SetTrigger("Disappear");
            isReady = true;
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
        // Update is called once per frame
        void Update()
        {
            //Debug.Log(isReady);
            if (!isReady)
                StartCoroutine(AppearGetReady());
        }
    }
}