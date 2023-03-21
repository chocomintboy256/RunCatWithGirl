using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TMPro;

public class GameClear: MonoBehaviour
{
    public static float fps;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimerText;
    public Camera GameCam;
    public Camera GameUICam;
    public Camera ClearCam;
    public Camera ClearUICam;
    public Vector3 CameraDistance;
    public Player player;
    public List<GameObject> animals;
    private List<Hashtable> GoData = new List<Hashtable>();
    // Start is called before the first frame update
    void Awake()
    {
        fps = (float)(1.0f / Time.fixedDeltaTime);
    }

    [System.Obsolete]
    void Start()
    {
        CameraDistance = GameCam.gameObject.transform.position;
        if (player == null) player = FindObjectOfType<Player>();
        if (animals.Count == 0)
        {
            var tmp = FindObjectsOfType<Animal>();
            foreach (var animal in tmp) animals.Add(animal.gameObject);
        }
        GameClearInit();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameMode == GameManager.GAMEMODE.CLEAR) return;
    }
    void GameClearInit()
    {
        ClearCam.gameObject.SetActive(true);
        ClearUICam.gameObject.SetActive(true);
        GameCam.gameObject.SetActive(false);
        GameUICam.gameObject.SetActive(false);

        player.NextAction(Player.ACTIONMODE.Clear);
        foreach (var animal in animals) animal.GetComponent<Animal>().NextAction(Animal.ACTIONMODE.Idol);
        //TimerText.text = Game._ins.GameNowTime.ToString();
        ScoreText.text = $"集めた数: {Game._ins.Score}";
    }
    public void OnClick(string str) {
        switch(str){
            case "Retry": 
                GameManager.SceneLoadWithSetGameMode(GameManager.GAMEMODE.PLAY);
                break;
            case "ToTitle":
                GameManager.SceneLoadWithSetGameMode(GameManager.GAMEMODE.TITLE);
               break;
        }
    }
}
