using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Game: MonoBehaviour
{
    public static float fps;
    public enum GAMEMODE {
        TITLE,
        PLAY,
        COLLECTION,
        OPTION
    }
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
 
    public GameObject PrefabFance;
    float FanceTileWidth = 1.2f;
    int FanceWidth = 15;
    int FanceHeight = 10;
    bool br = true;
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
    void Start()
    {
        CameraDistance = GameCam.gameObject.transform.position;
        player = FindObjectOfType<Player>();
        var tmp = FindObjectsOfType<Animal>();
        Game.NextGameMode(Game.GAMEMODE.TITLE);
        foreach(var animal in tmp) animals.Add(animal.gameObject);
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

    // Update is called once per frame
    void Update()
    {
        if (Game._GameMode == 0) return;
        GameCameraMove();
        CatchUpAnimal();
        if (br) {br=false; CreateFance();}
    }
    void GameCameraMove() 
    {
        GameCam.transform.position = CameraDistance + player.gameObject.transform.position;
    }
    void CatchUpAnimal()
    {
        var keyboard = Keyboard.current;
        if (player.targets.Count > 0 &&
            keyboard.spaceKey.wasPressedThisFrame ||
            Gamepad.current.buttonSouth.wasReleasedThisFrame
        ) {
            player.NextAnimation("Up", ((str) => { player.NextAnimation("Run"); }));
            GameObject target = GetNearestTarget(player.targets);
            Animal animal = target.GetComponent<Animal>();
            animal.NextAnimation("Up", ((str) => {
                animals.Remove(target);
                player.targets.Remove(target);
                Destroy(target);
            }));
            target.transform.position = player.transform.position + (Vector3.forward / 1.5f);
        }
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
        } 
    }
}
