using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneCondition: MonoBehaviour
{
    public EventSystem _eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        // イベント機能が未使用なら自身のイベント機能をオンにする(シーンをエディタで編集中に実行した時など)
        if(!EventSystem.current) { 
            _eventSystem.gameObject.SetActive(true);
            _eventSystem.enabled = true;
            EventSystem.current = _eventSystem;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick(GameObject btn)
    {
        string pattern = @"\d+$";
        string input = btn.name;
        Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
        if (m.Success)
        {
            GameManager.StageNum = Int32.Parse(m.Value);
            GameManager.NextGameMode(GameManager.GAMEMODE.PLAY);
        }
        else
        {
            switch (btn.name) {
                case "ButtonBack":
                    GameManager.BackPage("ConditionScene");
                    break;
            }
        }
   }
}
