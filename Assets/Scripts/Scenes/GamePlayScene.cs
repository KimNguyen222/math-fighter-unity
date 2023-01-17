using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayScene : MonoBehaviour
{
    public List<Sprite> _backgrounds;
    [SerializeField]
    private Image Background;
    // Start is called before the first frame update
    void Start()
    {
        Background.GetComponent<Image>().sprite = _backgrounds[3];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
