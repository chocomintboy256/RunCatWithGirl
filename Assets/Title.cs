using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class Title : MonoBehaviour
{
    public Camera GameCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick(GameObject obj)
    {
        switch (obj.name) {
            case "ButtonStart": 
                Game.NextGameMode(Game.GAMEMODE.PLAY);
                gameObject.SetActive(false);
                break;
            case "ButtonRule": break;
            case "ButtonCollection": break;
            case "ButtonOption": break;
        }
    }
}
