using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Linq;

public class Game : MonoBehaviour
{
    public static float fps;
    public static Game _ins;
    public static Game ins {
        get { return _ins; }
        set {
            if (!_ins) _ins = value;
        }
    }
    public GameInputs _gameInputs;
    public GameInputs gameInputs {
        get { return _gameInputs; }
        set { if (_gameInputs == null) _gameInputs = value; }
    }
    public Minimap minimap;
    public GameObject PrefabFance;
    float FanceTileWidth = 1.2f;
    int FanceWidth = 15;
    int FanceHeight = 10;
    bool br = true;

    public GameObject GameObjectAnimal; //生成するAnimaのゲームオブジェクトl
    public bool StartFlag = false;
    public bool CameraMoveFlag = false;
    public int Score = 0;
    public int GameStartTime = 60;
    public int GameNowTime = 60;
    public float timer = 0.0f;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimerText;
    public Camera GameCam;
    public Camera GameUICam;
    public Camera MinimapCam;
    public Vector3 CameraDistance;
    public Player player;
    public List<GameObject> animals;
    //public List<GameObject> allAnimals;
    //public List<int> animalIDs;
    public List<int> stageClearAnimalIDs;
    public List<List<StageAnimalData>> stageAnimalDataSets = new List<List<StageAnimalData>>() { 
        new List<StageAnimalData>() { // 1-1
            new StageAnimalData(active: true, x: 0.09f, z: 4.04f, kind: Animal.KIND.Panda),
            new StageAnimalData(active: true, x: -0.9292631f, z: 3.2077991f, kind: Animal.KIND.Bear),
        }, new List<StageAnimalData>() { // 1-2
            new StageAnimalData(active: true, x: 0.09f, z: 4.04f, kind: Animal.KIND.Panda),
            new StageAnimalData(active: true, x: -0.9292631f, z: 3.2077991f, kind : Animal.KIND.Bear),
            new StageAnimalData(active: true, x: 0.4f, z: 3.34f, kind : Animal.KIND.Cat),
        }, new List<StageAnimalData>() { // 1-3
            new StageAnimalData(active: true, x: 0.09f*2.0f, z: 4.04f*2.0f, kind: Animal.KIND.Panda),
            new StageAnimalData(active: true, x: -0.9292631f*2.0f, z: 3.2077991f*2.0f, kind : Animal.KIND.Bear),
            new StageAnimalData(active: true, x: 0.4f*2.0f, z: 3.34f*2.0f, kind : Animal.KIND.Cat),
            new StageAnimalData(active: true, x: 0.09f*4.0f, z: 4.04f*4.0f, kind : Animal.KIND.Panda),
        }, new List<StageAnimalData>() { // 1-4
            new StageAnimalData(active: true, x: 0.09f*2.0f, z: 4.04f*2.0f, kind : Animal.KIND.Panda),
            new StageAnimalData(active: true, x: -0.9292631f*2.0f, z: 3.2077991f*2.0f, kind : Animal.KIND.Bear),
            new StageAnimalData(active: true, x: 0.4f*2.0f, z: 3.34f*2.0f, kind : Animal.KIND.Cat),
            new StageAnimalData(active: true, x: 0.09f*4.0f, z: 4.04f*4.0f  , kind : Animal.KIND.Panda),
            new StageAnimalData(active: true, x: -0.9292631f*4.0f, z: 3.2077991f*4.0f, kind : Animal.KIND.Bear),
         }, new List<StageAnimalData>() { // 1-5
            new StageAnimalData(active: true, x: 0.09f*2.0f, z: 4.04f*2.0f, kind : Animal.KIND.Panda),
            new StageAnimalData(active: true, x: -0.9292631f*2.0f, z: 3.2077991f*2.0f, kind : Animal.KIND.Bear),
            new StageAnimalData(active: true, x: 0.4f*2.0f, z: 3.34f*2.0f, kind : Animal.KIND.Cat),
            new StageAnimalData(active: true, x: 0.09f*4.0f, z: 4.04f*4.0f, kind : Animal.KIND.Panda),
            new StageAnimalData(active: true, x: -0.9292631f*4.0f, z: 3.2077991f*4.0f, kind : Animal.KIND.Bear),
            new StageAnimalData(active: true, x: 0.4f*4.0f, z: 3.34f*4.0f, kind : Animal.KIND.Cat),
         }, new List<StageAnimalData>() { // 1-6
            new StageAnimalData(active: true, x: 0.09f*2.0f, z: 4.04f*2.0f, kind : Animal.KIND.Panda    ),
            new StageAnimalData(active: true, x: -0.9292631f*2.0f, z: 3.2077991f*2.0f, kind : Animal.KIND.Bear),
            new StageAnimalData(active: true, x: 0.4f*2.0f, z: 3.34f*2.0f, kind : Animal.KIND.Cat),
            new StageAnimalData(active: true, x: 0.09f*4.0f, z: 4.04f*4.0f, kind : Animal.KIND.Panda),
            new StageAnimalData(active: true, x: 0.09f*-8.0f, z: 4.04f*-8.0f, kind : Animal.KIND.Bear),
            new StageAnimalData(active: true, x: -0.9292631f*4.0f, z: 3.2077991f*4.0f, kind : Animal.KIND.Cat),
            new StageAnimalData(active: true, x: -0.9292631f*-8.0f, z: 3.2077991f*-8.0f, kind : Animal.KIND.Panda),
            new StageAnimalData(active: true, x: 0.4f*4.0f, z: 3.34f*4.0f, kind : Animal.KIND.Bear),
            new StageAnimalData(active: true, x: 0.4f*-8.0f, z: 3.34f*-8.0f, kind : Animal.KIND.Cat),
         }
    };

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
        if (player == null) player = FindObjectOfType<Player>();
        CameraDistance = GameCam.gameObject.transform.position;

        // ステージ開始演出完了したらゲームをスタートする
        GameInit();
        TextDoTween.EffectStageStartCountDown(() => { GameStart(); });
    }
    void GameInit()
    {
        DispTime(GameNowTime);
        DispScore(Score);
        GameCameraMove();
        SetupAnimals();
    }
    void SetupAnimals()
    {
        // ステージデータから動物たちの表示と位置を設定して、アクティブな動物一覧を取得する
        StageAnimalData[] sdAnimals = ins.stageAnimalDataSets[GameManager.StageNum-1].ToArray();
        animals = new List<GameObject>();
        for (int i = 0; i < sdAnimals.Length; i++) {
            //GameObject animal = allAnimals[i];
            StageAnimalData dAnimal = sdAnimals[i];
            GameObject animal = Animal.InstanceWithInit(
                kind: dAnimal.kind, 
                vec3: new Vector3(dAnimal.x, 0.0f, dAnimal.z), 
                actionMode: Animal.ACTIONMODE.Run, 
                minimapMarkerFg: true
            );
            animals.Add(animal);
            minimap.CreateMarker(animal);
        }
    }
    void GameStart()
    {
        StartFlag = true;
        player.NextAction(Player.ACTIONMODE.Run);
        foreach (var animal in ins.animals)
            animal.GetComponentInChildren<Animal>().NextAction(Animal.ACTIONMODE.Run);

        if (animals.Count == 0)
        {
            var tmp = FindObjectsOfType<Animal>();
            foreach (var animal in tmp) animals.Add(animal.gameObject);
        }
        // カメラ位置をプレイヤーの位置に近づける
        /*
        DOVirtual.Float(from: 0.0f, to: 1.0f, duration: 1.0f, onVirtualUpdate: (tweenValue) =>
        {
            Vector3 pos = player.gameObject.transform.position;
            Vector3 GoalPos = new Vector3(pos.x, CameraDistance.y, pos.z);
            Vector3 StartPos = CameraDistance;
            float x = StartPos.x + ((GoalPos.x - StartPos.x) * tweenValue);
            float z = StartPos.z + ((GoalPos.z - StartPos.z) * tweenValue);
            GameCam.transform.position = new Vector3( x, CameraDistance.y, z);
        }).OnComplete(() => { CameraMoveFlag = true; });
        */ CameraMoveFlag = true;
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
        if (br) {br=false; CreateFance();}
        if (!StartFlag) return; 

        GameCameraMove();
        CatchUpAnimal();
        TimerCountDown();
        if (IsGameClear()) GameClear();
    }
    bool IsGameClear() {
        return GameNowTime == 0 || animals.Count == 0;
    }
    void GameClear() {
        GameManager.ins.Score = Score;
        GameManager.ins.TotalScore += Score;
        GameManager.ins.stageClearAnimalIDs = stageClearAnimalIDs;
        SceneManager.LoadScene("ClearScene");
    }
    void GameCameraMove() 
    {
        Vector3 pos = player.gameObject.transform.position;
        // if (CameraMoveFlag) GameCam.transform.position = new Vector3(pos.x, CameraDistance.y, pos.z);
        if (CameraMoveFlag) GameCam.transform.position = new Vector3(pos.x, CameraDistance.y, pos.z - 1.6f);
        MinimapCam.transform.position = new Vector3(pos.x, MinimapCam.transform.position.y, pos.z);
    }
    void CatchUpAnimal()
    {
        if (player.targets.Count > 0 && GameManager.ins.IsInputFire) {
            player.NextAction(Player.ACTIONMODE.Up,                 // プレイヤーが拾うアクション
                (unityActionComplete) => {
                    player.NextAction(Player.ACTIONMODE.Run);
                });
            GameObject target = GetNearestTarget(player.gameObject, player.targets);   // 一番近いターゲット取得
            if (target == null) return;
            Animal animal = target.GetComponentInChildren<Animal>();          
            if (animal.ActionMode == Animal.ACTIONMODE.Up) return;  // すでに持ち上がっていたらやめ
            target.GetComponentInChildren<SphereCollider>().enabled = false;  // 持ち上げ処理に移行 ターゲットの当たり判定消す
            animal.NextAction(Animal.ACTIONMODE.Up,                 // アニメーション設定
                (unityActionComplete) => {
                    //int ind  = allAnimals.FindIndex(x => x == target);
                    //int id  = ind + 1;
                    //animalIDs.Remove(ind);
                    // GameObject animalContainer = target.transform.parent.gameObject;
                    GameObject animalContainer = target;
                    stageClearAnimalIDs.Add(animal.animalDataId);
                    animals.Remove(animalContainer);
                    player.targets.Remove(target);
                    minimap.DestroyMarker(animalContainer);
                    Destroy(animalContainer);
                    ScoreUp();
                });
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
 
    GameObject GetNearestTarget(GameObject src, List<GameObject> list)
    {
        if (list.Count == 0) return null;
        list.Sort((a,b) => {
            return Vector3.Distance(src.transform.position, a.transform.position) <= 
                    Vector3.Distance(src.transform.position, b.transform.position) ? -1 : 1;
        });
        return list[0];
    }
}
