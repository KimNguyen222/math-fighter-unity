using MathFighter.GamePlay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VSScene : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;

    [SerializeField]
    private SpriteRenderer _character1;
    [SerializeField] 
    private SpriteRenderer _character2;

    [SerializeField]
    private TMP_Text _playerName1;
    [SerializeField]
    private TMP_Text _playerName2;

    private GamePlaySettings settings;

    public List<Sprite> characters;
    // Start is called before the first frame update
    void Start()
    {
        //player1 = GameObject.Find("Player1");
        //player2 = GameObject.Find("Player2");
        //player1.transform.position = _playerObject1.transform.position;
        //player2.transform.position = _playerObject2.transform.position;
        //player1.transform.SetParent(_playerObject1.transform);
        //player2.transform.SetParent(_playerObject2.transform);
        //player1.transform.localScale = new Vector3(150f, 150f, 150f);
        //player2.transform.localScale = new Vector3(150f, 150f, 150f);

        settings = GameObject.Find("GamePlaySettings").GetComponent<GamePlaySettings>();
        _character1.sprite = characters[settings.playerNum1];
        _character2.sprite = characters[settings.playerNum2];
        _playerName1.text = settings.playerName1;
        _playerName2.text = settings.playerName2;
        StartCoroutine(LoadGamePlayScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator LoadGamePlayScene()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("GamePlayScene");
    }
}
