using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSScene : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;

    [SerializeField]
    private GameObject _playerObject1;
    [SerializeField] 
    private GameObject _playerObject2;
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        player1.transform.position = _playerObject1.transform.position;
        player2.transform.position = _playerObject2.transform.position;
        player1.transform.SetParent(_playerObject1.transform);
        player2.transform.SetParent(_playerObject2.transform);
        player1.transform.localScale = new Vector3(150f, 150f, 150f);
        player2.transform.localScale = new Vector3(150f, 150f, 150f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
