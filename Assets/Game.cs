using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using TMPro;

public class Game: MonoBehaviour
{
    public static float fps;
    public enum GAMEMODE {
        TITLE,
        PLAY,
        COLLECTION,
        OPTION,
        CLEAR 
    }
    public static GAMEMODE SeneInitGameMode = Game.GAMEMODE.TITLE;
    public static GAMEMODE _GameMode = 0;
    public static GAMEMODE GameMode {
        get {return _GameMode;}
        set {_GameMode = value;}
    }
    public static Game _ins;
    public static Game ins {
        get {return _ins;}
        set {
            if (!_ins) _ins = value;
        }
    }
    public GameInputs _gameInputs;
    public GameInputs gameInputs {
        get {return _gameInputs;}
        set {if (_gameInputs == null) _gameInputs = value;}
    }
    private bool _IsInputAction = false;
    public GameObject PrefabFance;
    float FanceTileWidth = 1.2f;
    int FanceWidth = 15;
    int FanceHeight = 10;
    bool br = true;

    public int Score = 0;
    public int GameStartTime = 60;
    public int GameNowTime = 60;
    public float timer = 0.0f;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimerText;
    public Camera GameCam;
    public Camera GameUICam;
    public Camera TitleCam;
    public Camera TitleUICam;
    public Vector3 CameraDistance;
    public Player player;
    public List<GameObject> animals;
    private List<Hashtable> GoData = new List<Hashtable>();
    // Start is called before the first frame update
    void Awake()
    {
        ins = this;
        fps = (float)(1.0f / Time.fixedDeltaTime);
    }

    [System.Obsolete]
    void Start()
    {
        InitInput();
        CameraDistance = GameCam.gameObject.transform.position;
        player = FindObjectOfType<Player>();
        var tmp = FindObjectsOfType<Animal>();
        foreach(var animal in tmp) animals.Add(animal.gameObject);
        Game.NextGameMode(Game.SeneInitGameMode);
        
        DispTime(GameNowTime);
        DispScore(Score);
    }
    void InitInput()
    {
        // InputAction設定
        _gameInputs = new GameInputs();
        _gameInputs.Player.Action.started += OnAction;
        _gameInputs.Player.Action.performed += OnAction;
        _gameInputs.Player.Action.canceled += OnAction;
        _gameInputs.Enable();
    }
    public void OnAction(InputAction.CallbackContext context) {
        _IsInputAction = true;
    }    
    void CreateFance()
    {
        Debug.Log(PrefabFance);
        for (var cnt = 0; cnt < FanceWidth; cnt++) {
            Vector3 PosTop = new Vector3(-3.0f + FanceTileWidth * cnt, 0.64f, 6.0f);
            Instantiate(PrefabFance, PosTop, Quaternion.identity);
            Vector3 PosBottom = new Vector3(-3.0f + FanceTileWidth * cnt, 0.64f, -1.0f);
            Instantiate(PrefabFance, PosBottom, Quaternion.Euler(0.0f,180.0f,0.0f));
        }
        for (var cnt = 0; cnt < FanceHeight; cnt++) {
            Vector3 PosLeft = new Vector3(-3.0f, 0.64f, 6.0f - FanceTileWidth * cnt);
            Instantiate(PrefabFance, PosLeft, Quaternion.Euler(0.0f,-90.0f,0.0f));
            Vector3 PosRight= new Vector3(-3.0f + FanceTileWidth * FanceWidth, 0.64f, 6.0f - FanceTileWidth * cnt);
            Instantiate(PrefabFance, PosRight, Quaternion.Euler(0.0f,90.0f,0.0f));
        }
    }
    void InputClear()
    {
        _IsInputAction = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Game._GameMode == GAMEMODE.TITLE) return;
        GameCameraMove();
        CatchUpAnimal();
        if (br) {br=false; CreateFance();}
        TimerCountDown();
        if (IsGameClear()) GameClear();
        InputClear();
    }
    bool IsGameClear() {
        return GameNowTime == 0 || animals.Count == 0;
    }
    void GameClear() {
        SceneManager.LoadScene("ClearScene");
    }
    void GameCameraMove() 
    {
        GameCam.transform.position = CameraDistance + player.gameObject.transform.position;
    }
    void CatchUpAnimal()
    {
        if (player.targets.Count > 0 && _IsInputAction) {
            player.NextAnimation("Up", ((str) => { player.NextAnimation("Run"); }));
            GameObject target = GetNearestTarget(player.targets);
            if (target == null) return;
            target.GetComponent<SphereCollider>().enabled = false;  // 当たり判定消す
            Animal animal = target.GetComponent<Animal>();          // アニメーション切り替え
            animal.NextAnimation("Up", ((str) => {
                animals.Remove(target);
                player.targets.Remove(target);
                Destroy(target);
                ScoreUp();
            }));
            target.transform.position = player.transform.position + (Vector3.forward / 1.5f);
        }
    }
    void TimerCountDown()
    {
        if (GameMode == Game.GAMEMODE.PLAY) {
            timer += Time.deltaTime;
            GameNowTime = Mathf.Max(GameStartTime - Mathf.FloorToInt(timer), 0);
            DispTime(GameNowTime);
        }
    }
    void DispTime(int now)
    {
        TimerText.text = $"タイム: {now}";
    }
    void ScoreUp()
    {
        Score += 1;
        DispScore(Score);
    }
    void DispScore(int score)
    {
        ScoreText.text = $"集めた数: {score}";
    }
 
    GameObject GetNearestTarget(List<GameObject> list)
    {
        if (list.Count == 0) return null;
        list.Sort((a,b) => {
            Vector3 p1 = player.gameObject.transform.position;
            return Vector3.Distance(p1, a.transform.position) <= 
                    Vector3.Distance(p1, b.transform.position) ? -1 : 1;
        });
        return list[0];
    }
    public static void NextGameMode(GAMEMODE nextGameMode) {
        GameMode = nextGameMode;
        switch (GameMode) {
        case GAMEMODE.TITLE: 
            ins.TitleCam.gameObject.SetActive(true);
            ins.TitleUICam.gameObject.SetActive(true);
            ins.GameCam.gameObject.SetActive(false);
            ins.GameUICam.gameObject.SetActive(false);

            ins.player.NextAction(Player.ACTIONMODE.Idol);
            foreach (var animal in ins.animals) animal.GetComponent<Animal>().NextAction(Animal.ACTIONMODE.Idol);
            break;
        case GAMEMODE.PLAY:
            ins.TitleCam.gameObject.SetActive(false);
            ins.TitleUICam.gameObject.SetActive(false);
            ins.GameCam.gameObject.SetActive(true);
            ins.GameUICam.gameObject.SetActive(true);

            ins.player.NextAction(Player.ACTIONMODE.Run);
            foreach (var animal in ins.animals) animal.GetComponent<Animal>().NextAction(Animal.ACTIONMODE.Run);
            break;
        case GAMEMODE.COLLECTION:break;
        case GAMEMODE.OPTION: break;
        case GAMEMODE.CLEAR: break;     // 別シーンで対応します
        } 
    }
    //--------
    // シーンをロードしてゲームモードを設定する
    //--------
    public static void SceneLoadWithSetGameMode(
                            GAMEMODE mode, 
                            string SceneName = "TitleGameScene")
    {
        Game.SeneInitGameMode = mode;
        SceneManager.LoadScene(SceneName); 
    }
}
