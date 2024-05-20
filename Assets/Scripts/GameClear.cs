using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TMPro;
using System.Linq;

public class GameClear: MonoBehaviour
{
    const int CLEAR_STAGE_ANIMAL_MAX = 9;
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
    public List<GameObject> allAnimals;
    private List<Hashtable> GoData = new List<Hashtable>();
    private List<Vector3> AnimalPosData = new List<Vector3>() {
        new Vector3(-0.230000004f,-1.78813934e-07f,3.92000008f),
        new Vector3(-1.10000002f,5.12599945e-06f,3.08779931f),
        new Vector3(0.0799999982f,-1.78813934e-07f,3.22000003f),
        new Vector3(0.279999971f,0f,4.6500001f),
        new Vector3(-0.590000033f,0f,3.81779933f),
        new Vector3(0.589999974f,0f,3.95000005f),
        new Vector3(-0.99000001f,0f,3.78999996f),
        new Vector3(1.12f,0f,2.95779943f),
        new Vector3(-0.680000007f,0f,3.09000015f)
    };
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
        AnimalSetUp();

        player.NextAction(Player.ACTIONMODE.Clear);
        //foreach (var animal in animals) animal.GetComponent<Animal>().NextAction(Animal.ACTIONMODE.Idol);

        //TimerText.text = Game._ins.GameNowTime.ToString();
        int score = GameManager.ins.Score;
        ScoreText.text = $"集めた数: {score}";
    }
    void AnimalSetUp()
    {
        if (GameManager.ins.stageClearAnimalIDs == null) return;

        int clearStageAnimalMax = Mathf.Min(CLEAR_STAGE_ANIMAL_MAX, GameManager.ins.stageClearAnimalIDs.Count);

        for (int i = 0; i < clearStageAnimalMax; i++) {
            int id = GameManager.ins.stageClearAnimalIDs[i];
            AnimalData ad = AnimalDataBase.AnimalMaster.First(x => x.id == id);
            GameObject animal = Animal.InstanceWithInit(kind: ad.kind, vec3: AnimalPosData[i]);
            animals.Add(animal);
        }
        
/*
        animals = new List<GameObject>();
        for (int i = 0; i < allAnimals.Count; i++) {
            allAnimals[i].SetActive(false);
            int targetIndex = GameManager.ins.stageClearAnimalIDs.Find(x => x == i + 1);
            if (targetIndex > 0) { 
                allAnimals[i].SetActive(true);
                animals.Add(allAnimals[i]);
            }
        }
*/
    }
    public void OnClick(string str) {
        switch(str){
            case "Retry": 
                GameManager.SceneLoadWithSetGameMode(GameManager.GAMEMODE.PLAY);
                break;
            case "StageSelect": 
                GameManager.NextPage("StageSelectScene");
                break;
            case "ToTitle":
                GameManager.SceneLoadWithSetGameMode(GameManager.GAMEMODE.TITLE);
               break;
        }
    }
}
