using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;

public class Title : MonoBehaviour
{
    public Player player;
    public List<GameObject> animals;
    public TextMeshProUGUI TotalScoreText;
    // Start is called before the first frame update
    void Start()
    {
        player.NextAction(Player.ACTIONMODE.Idol);
        foreach (var animal in animals) 
            animal.GetComponent<Animal>().NextAction(Animal.ACTIONMODE.Idol);
        TotalScoreText.text = GameManager.ins.TotalScore > 0 ?
            $"ç°Ç‹Ç≈èWÇﬂÇΩêî: {GameManager.ins.TotalScore}" :
            $"";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick(GameObject btn)
    {
        switch (btn.name) {
            case "ButtonStart":
                //GameManager.NextGameMode(GameManager.GAMEMODE.PLAY);
                GameManager.NextPage("StageSelectScene");
                break;
            case "ButtonExchange":
                GameManager.NextPage("ExchangeScene");
                break;
            case "ButtonCondition":
                GameManager.NextPage("ConditionScene");
                break;
            case "ButtonRule":
            case "ButtonCollection":
            case "ButtonOption":
                var ui = GameObject.Find($"/Title UI Canvas");
                var panelName = btn.name.Replace("Button","Panel");
                ui.transform.Find(panelName).gameObject.SetActive(true);
                Debug.Log(ui.transform.Find(panelName));
                break;
            case "ButtonClose":
                btn.transform.parent.gameObject.SetActive(false);
                break;
        }
    }
}
