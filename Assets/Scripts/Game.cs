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
        player.NextAction(Player.ACTIONMODE.Run);
        foreach (var animal in ins.animals) 
            animal.GetComponent<Animal>().NextAction(Animal.ACTIONMODE.Run);

        CameraDistance = GameCam.gameObject.transform.position;
        if (player == null) player = FindObjectOfType<Player>();
        if (animals.Count == 0) {
            var tmp = FindObjectsOfType<Animal>();
            foreach(var animal in tmp) animals.Add(animal.gameObject);
        }
        DispTime(GameNowTime);
        DispScore(Score);
    }
    void CreateFance()
    {
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

    // Update is called once per frame
    void Update()
    {
        GameCameraMove();
        CatchUpAnimal();
        if (br) {br=false; CreateFance();}
        TimerCountDown();
        if (IsGameClear()) GameClear();
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
        if (player.targets.Count > 0 && GameManager.ins.IsInputAction) {
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
        timer += Time.deltaTime;
        GameNowTime = Mathf.Max(GameStartTime - Mathf.FloorToInt(timer), 0);
        DispTime(GameNowTime);
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
}
