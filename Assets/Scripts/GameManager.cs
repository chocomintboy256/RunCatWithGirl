using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager: MonoBehaviour
{
    public static float fps;
    public enum GAMEMODE {
        TITLE,
        PLAY,
        COLLECTION,
        OPTION,
        CLEAR 
    }
    public static GAMEMODE SeneInitGameMode = GameManager.GAMEMODE.TITLE;
    public static GAMEMODE _GameMode = 0;
    public static GAMEMODE GameMode {
        get {return _GameMode;}
        set {_GameMode = value;}
    }
    public static GameManager _ins;
    public static GameManager ins {
        get {return _ins;}
        set {
            if (!_ins) _ins = value;
        }
    }
    //---- input ----
    public GameInputs _gameInputs;
    public GameInputs gameInputs {
        get {return _gameInputs;}
        set {if (_gameInputs == null) _gameInputs = value;}
    }
    private bool _IsInputFire = false;
    public bool IsInputFire {
        get {return _IsInputFire;}
        set {_IsInputFire = value;}
    }
    private Vector2 _mouseInputValue;
    public Vector2 mouseInputValue {
        get {return _mouseInputValue;}
        set {if (_mouseInputValue == null) _mouseInputValue = value;}
    }
    private Vector2 _stickInputValue;
    public Vector2 stickInputValue {
        get {return _stickInputValue;}
        set {if (_stickInputValue == null) _stickInputValue = value;}
    }
 
    public int Score = 0;
    public int GameStartTime = 60;
    public int GameNowTime = 60;
    public float timer = 0.0f;
    public Vector3 CameraDistance;
    public Player player;
    public List<GameObject> animals;
    private List<Hashtable> GoData = new List<Hashtable>();

    // Start is called before the first frame update
    void Awake()
    {
        ins = this;
        fps = (float)(1.0f / Time.fixedDeltaTime);
        GameManager.SeneInitGameMode = getActiveSceneGameMode();
    }
    private GAMEMODE getActiveSceneGameMode() {
        switch(SceneManager.GetActiveScene().name) {
            case "TitleScene": return GameManager.GAMEMODE.TITLE;
            case "GameScene": return GameManager.GAMEMODE.PLAY;
            case "ClearScene": return GameManager.GAMEMODE.CLEAR;
        }
        return GameManager.GAMEMODE.TITLE;
    }
    [System.Obsolete]
    void Start()
    {
        InitInput();
        GameMode = GameManager.SeneInitGameMode;
        player = FindObjectOfType<Player>();
        var tmp = FindObjectsOfType<Animal>();
        foreach(var animal in tmp) animals.Add(animal.gameObject);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        InputFireClear();
    }

    void InitInput()
    {
        // InputFire設定
        _gameInputs = new GameInputs();
        _gameInputs.Player.Fire.started += OnFire;
        _gameInputs.Player.Fire.performed += OnFire;
        _gameInputs.Player.Fire.canceled += OnFire;

        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;
        _gameInputs.Player.MoveStick.started += OnMoveStick;
        _gameInputs.Player.MoveStick.performed += OnMoveStick;
        _gameInputs.Player.MoveStick.canceled += OnMoveStick;
        //_gameInputs.Player.Jump.performed += OnJump;
        _gameInputs.Enable();
    }
    //---- Input ----
    public void OnFire(InputAction.CallbackContext context) {
        _IsInputFire = true;
    }    
    public void InputFireClear()
    {
        _IsInputFire = false;
    }
    public bool IsInputGamePad()
    {
        return _stickInputValue.x != 0 || _stickInputValue.y != 0;
    }
    // マウス座標が更新された時に通知するコールバック関数
    public void OnMove(InputAction.CallbackContext context) {
        _mouseInputValue = context.ReadValue<Vector2>();
    }
    public void OnMoveStick(InputAction.CallbackContext context) {
        _stickInputValue = context.ReadValue<Vector2>();
    }
    //---- GameMode ---- 
    public static void NextGameMode(GAMEMODE nextGameMode) {
        GameMode = nextGameMode;
        switch (GameMode) {
        case GAMEMODE.TITLE: 
            break;
        case GAMEMODE.PLAY:
            SceneLoadWithSetGameMode(nextGameMode);
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
                            string SceneName = "GameScene")
    {
        GameManager.SeneInitGameMode = mode;
        SceneName = 
            mode == GameManager.GAMEMODE.TITLE ? "TitleScene" :
            mode == GameManager.GAMEMODE.PLAY ? "GameScene" :
            mode == GameManager.GAMEMODE.CLEAR ? "ClearScene" : "GameScene";
        SceneManager.LoadScene(SceneName); 
    }
}
