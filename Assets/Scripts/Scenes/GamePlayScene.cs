using MathFighter.GamePlay;
using MathFighter.Math;
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
        /********** Game UI ***********/
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
        private GameObject Spotlight1;
        [SerializeField] 
        private GameObject Spotlight2;
        [SerializeField]
        private GameObject QuestionUI;
        [SerializeField]
        private TMP_Text QuestionTitle;
        [SerializeField]
        private GameObject QuestionBar;
        [SerializeField]
        private TMP_Text QuestionCat;
        [SerializeField]
        private TMP_Text QuestionNum;
        [SerializeField]
        private TMP_Text Grade;
        [SerializeField]
        private TMP_Text Timer;
        [SerializeField]
        private Image TimerBar;
        [SerializeField]
        private Image HealthBar1;
        [SerializeField]
        private Image HealthBar2;
        [SerializeField]
        private GameObject HintPanel;
        [SerializeField]
        private TMP_Text HintText;
        [SerializeField]
        private SpriteRenderer SpotlightGreen;
        [SerializeField]
        private SpriteRenderer SpotlightRed;

        /******** Additional Buttons  **********/
        [SerializeField]
        private SpriteRenderer SuperAttackLeftButton;
        [SerializeField]
        private SpriteRenderer SuperAttackRightButton;

        /********* AnswerStatus   **************/
        [SerializeField]
        private SpriteRenderer Correct1;
        [SerializeField]
        private SpriteRenderer Correct2;
        [SerializeField]
        private SpriteRenderer Wrong1;
        [SerializeField]
        private SpriteRenderer Wrong2;
        [SerializeField]
        private TMP_Text XNumber;

        /******** Public List Objects **********/
        public List<Sprite> _backgrounds;
        public List<Sprite> _normalSprites;
        public List<Sprite> _correctSprites;
        public List<Sprite> _wrongSprites;

        public List<Sprite> _avatars;
        public List<GameObject> PlayerPrefabs;
        public List<GameObject> AttackDamagePrefabs;
        public List<GameObject> SuperAttackDamagePrefabs;

        public List<TMP_Text> ContentTexts;
        public List<Image> AnswerButtons;
        public List<RawImage> AnswerRawImages;

        /******** Private List Objects **********/
        private List<GameObject> spotlights;
        private GamePlaySettings settings;
        private List<Texture2D> mathTextures;

        /********* Game Play *********/
        private GameObject player1;
        private GameObject player2;
        private GameObject attackDamage1;
        private GameObject attackDamage2;
        private GameObject superAttackDamage1;
        private GameObject superAttackDamage2;
        private Animator animGetReady;

        Animator animator1;
        Animator animator2;
        Animator attackAnim1;
        Animator attackAnim2;
        Animator superAttackAnim1;
        Animator superAttackAnim2;
        private int questionNum = 1;
        private bool isReady = false;
        private bool isSpotlightGreeRed = false;
        private bool gameplay = false;
        private bool isQuestionShown = false;
        private int energy1;
        private int energy2;
        private int maxEnergy1;
        private int maxEnergy2;
        private int damage = 30;
        private int xNumber = 1;
        private bool gameover = false;
        private bool onHint = true;


        private float initHealthBarXPosition1;
        private float initHealthBarXPosition2;


        /********* Timer ************/
        private float maxTime = 15f;
        private float currentTime;
        private bool isTimer = false;

        private QuestionContent question;

        // Start is called before the first frame update
        void Start()
        {
            settings = GameObject.Find("GamePlaySettings").GetComponent<GamePlaySettings>();
            spotlights = new List<GameObject>();
            player1 = Instantiate(PlayerPrefabs[settings.playerNum1], new Vector3(-5.5f, -2.2f, 0), Quaternion.Euler(0f, 180f, 0f));
            player2 = Instantiate(PlayerPrefabs[settings.playerNum2], new Vector3(5.5f, -2.2f, 0), Quaternion.identity);
            attackDamage1 = Instantiate(AttackDamagePrefabs[settings.playerNum1], new Vector3(5.5f, 0, 0), Quaternion.Euler(0f, 180f, 0f));
            attackDamage2 = Instantiate(AttackDamagePrefabs[settings.playerNum2], new Vector3(-5.5f, 0, 0), Quaternion.identity);
            superAttackDamage1 = Instantiate(SuperAttackDamagePrefabs[settings.playerNum1], new Vector3(5.5f, 0, 0), Quaternion.Euler(0f, 180f, 0f));
            superAttackDamage2 = Instantiate(SuperAttackDamagePrefabs[settings.playerNum2], new Vector3(-5.5f, 0, 0), Quaternion.identity);


            animator1 = player1.GetComponent<Animator>();
            animator2 = player2.GetComponent<Animator>();
            attackAnim1 = attackDamage1.GetComponent<Animator>();
            attackAnim2 = attackDamage2.GetComponent<Animator>();
            superAttackAnim1 = superAttackDamage1.GetComponent<Animator>();
            superAttackAnim2 = superAttackDamage2.GetComponent<Animator>();


            energy1 = settings.EnergyBar;
            energy2 = settings.EnergyBar;
            maxEnergy1 = energy1;
            maxEnergy2 = energy2;

            initHealthBarXPosition1 = HealthBar1.transform.localPosition.x;
            initHealthBarXPosition2 = HealthBar2.transform.localPosition.x;

            attackDamage1.SetActive(false);
            attackDamage2.SetActive(false);
            superAttackDamage1.SetActive(false);
            superAttackDamage2.SetActive(false);

            SetEnvironment();
            GetReady.gameObject.SetActive(true);
            animGetReady = GetReady.GetComponent<Animator>();

            MathExpression.RegisterDefaultOperators();
            MathQuestion.LoadData();
            settings.CreateNewDealer();
            question = settings.Dealer.GetQuestion();

            currentTime = maxTime;
            UpdateTimer();
            mathTextures = new List<Texture2D>();
            for (int i = 0; i < 5; i ++)
            {
                mathTextures.Add(new Texture2D(100, 100));
            }
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

            //if (!isSpotlightGreeRed)
            //    DrawSpotlightGreenRed(1);
            //else
            //    RemoveSpotlightGreenRed();
        }

        public void OnClickAnswers(int anserIndex)
        {
            if (isQuestionShown)
            {
                if (anserIndex == question.RightAnswer)       // Correct Answer
                {
                    settings.CreateNewDealer();
                    question = settings.Dealer.GetQuestion();
                    SetNormalSpritesAll();
                    AnswerButtons[anserIndex].sprite = _correctSprites[anserIndex];
                    //StartCoroutine(DrawSpotlightGreen());
                    //StartCoroutine(RemoveSpotlightGreenRed());
                    isTimer = false;
                    StartCoroutine(Attack());
                    //Spotlight1.SetActive(true);
                }
                else                                          // Wrong Answer
                {
                    SetNormalSpritesAll();
                    //StartCoroutine(RemoveSpotlightGreenRed());
                    Spotlight1.SetActive(false);
                    StartCoroutine(DrawSpotlightRed());
                    AnswerButtons[anserIndex].sprite = _wrongSprites[anserIndex];
                }
            }
        }

        // Click Taunt Button
        public void OnTauntButtonClicked()
        {
            if (isQuestionShown)
                animator1.SetTrigger("taunt");
        }

        // Click Hint Button
        public void OnHintButtonClicked()
        {
            onHint = !onHint;
            if (isQuestionShown)
            {
                if (!onHint)
                {
                    animator1.SetTrigger("freeze");
                    HintPanel.SetActive(true);
                    HintText.text = question.Hint;
                }
                else
                {
                    animator1.SetTrigger("unFreeze");
                    HintPanel.SetActive(false);
                }
            }
        }

        // Set all sprites of Answer Buttons to Normal
        private void SetNormalSpritesAll()
        {
            for (int i = 0; i < AnswerButtons.Count; i ++)
            {
                AnswerButtons[i].sprite = _normalSprites[i];
            }
        }
        private void RemoveAllAnswersUI()
        {
            QuestionTitle.text = "";
            Grade.text = "";
            for (int i = 0; i < AnswerButtons.Count; i++)
            {
                ContentTexts[i].text = "";
            }
        }
        private void GamePlay()
        {
            UpdateTimer();
            if (!isQuestionShown)
            {
                isTimer = false;
                if (question.LevelName == null) return;
                StartCoroutine(ShowQuestionTitle());
            }

            if (currentTime > 0 && isTimer)
                currentTime -= Time.deltaTime;

        }

        private void UpdateTimer()
        {
            Timer.text = currentTime.ToString("0");
            TimerBar.fillAmount = currentTime / maxTime;            
        }

        private IEnumerator AppearGetReady()
        {
            yield return new WaitForSeconds(1);
            animGetReady.SetTrigger("Appear");
            yield return new WaitForSeconds(2);
            animGetReady.SetTrigger("Disappear");
            yield return new WaitForSeconds(1);
            isReady = true;
            gameplay = true;
        }

        private IEnumerator Attack()
        {
           
            Animator spotAnim = SpotlightGreen.GetComponent<Animator>();
            spotAnim.SetTrigger("appear");
            Spotlight1.SetActive(false);
            yield return new WaitForSeconds(2);
            animator1.SetTrigger("attack");
            yield return new WaitForSeconds(1f);
            animator2.SetTrigger("takingDamage");
            attackDamage1.SetActive(true);
            attackAnim1.SetTrigger("attackDamage");

            StartCoroutine(UpdateHealthBars2());
            spotAnim.SetTrigger("disappear");
            yield return new WaitForSeconds(1);
            Spotlight1.SetActive(true);
            yield return new WaitForSeconds(2);
            attackDamage1.SetActive(false);
            questionNum++;
            RemoveAllAnswersUI();
            SetNormalSpritesAll();
            isQuestionShown = false;
        }
        private IEnumerator UpdateHealthBars2()
        {
            for (int h= energy2; h >= energy2 - damage; h -= 2)
            {
                HealthBar2.fillAmount = (float)h / (float)maxEnergy2;
                HealthBar2.transform.localPosition = new Vector3(initHealthBarXPosition2 - 
                    (1 - HealthBar2.fillAmount) * HealthBar2.rectTransform.rect.width, HealthBar2.transform.localPosition.y, 
                    HealthBar2.transform.localPosition.z);
                HealthBar2.color = new Color(1, HealthBar2.fillAmount, 0, 1);
                yield return new WaitForSeconds(0.1f);
            }
            energy2 -= damage;
            if (energy2 <= 0)                          // Win the game
            {
                gameplay = false;
                animator2.SetTrigger("lose");
                yield return new WaitForSeconds(1);
                //player2.SetActive(false);
                animator1.SetTrigger("win");
            }
        }
        private IEnumerator ShowQuestionTitle()
        {
            QuestionUI.SetActive(true);
            QuestionNum.text = "Question " + questionNum;
            QuestionCat.text = question.LevelName;
            Grade.text = question.CatName;
            yield return new WaitForSeconds(1);
            QuestionUI.SetActive(false);
            currentTime = maxTime;
            isTimer = true;
            QuestionTitle.text = question.Question;
            ContentTexts[0].text = question.Answers[0];
            ContentTexts[1].text = question.Answers[1];
            ContentTexts[2].text = question.Answers[2];
            ContentTexts[3].text = question.Answers[3];

            MathTextRenderer mathTextRenderer = new MathTextRenderer();

            QuestionBar.GetComponent<RawImage>().texture = mathTextRenderer.RenderText(question.Question);
            AnswerRawImages[0].GetComponent<RawImage>().texture = mathTextRenderer.RenderText(question.Answers[0]);
            AnswerRawImages[1].GetComponent<RawImage>().texture = mathTextRenderer.RenderText(question.Answers[1]);
            AnswerRawImages[2].GetComponent<RawImage>().texture = mathTextRenderer.RenderText(question.Answers[2]);
            AnswerRawImages[3].GetComponent<RawImage>().texture = mathTextRenderer.RenderText(question.Answers[3]);



            isQuestionShown = true;
        }


        //private void OnGUI()
        //{
        //    Debug.Log("MathTextures Length: " + mathTextures.Count);
        //    GUI.DrawTexture(ContentTexts[0].rectTransform.rect, mathTextures[0]);
        //    GUI.DrawTexture(ContentTexts[1].rectTransform.rect, mathTextures[1]);
        //    GUI.DrawTexture(ContentTexts[2].rectTransform.rect, mathTextures[2]);
        //    GUI.DrawTexture(ContentTexts[3].rectTransform.rect, mathTextures[3]);
        //    GUI.DrawTexture(ContentTexts[4].rectTransform.rect, mathTextures[4]);
        //}
        private IEnumerator DrawSpotlightGreen()
        {
            //SpotlightGreen.gameObject.SetActive(true);
            Animator spotAnim = SpotlightGreen.GetComponent<Animator>();
            spotAnim.SetTrigger("appear");
            yield return new WaitForSeconds(2);
            spotAnim.SetTrigger("disappear");
            Spotlight1.SetActive(true);
            //SpotlightGreen.gameObject.SetActive(false);
        }

        private IEnumerator DrawSpotlightRed()
        {
            //SpotlightRed.gameObject.SetActive(true);
            Animator spotAnim = SpotlightRed.GetComponent<Animator>();
            spotAnim.SetTrigger("appear");
            yield return new WaitForSeconds(2);
            spotAnim.SetTrigger("disappear");
            Spotlight1.SetActive(true);
            //SpotlightRed.gameObject.SetActive(false);
        }

        private IEnumerator RemoveSpotlightGreenRed()
        {
            for (int i = spotlights.Count - 1; i >= 0; i--)
            {
                GameObject spotlight = spotlights[i];
                spotlights.Remove(spotlight);
                Destroy(spotlight);
                yield return new WaitForSeconds(0.02f);
            }
            Spotlight1.SetActive(true);
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