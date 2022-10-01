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
 
    public Camera GameCam;
    public Camera GameUICam;
    public Camera TitleCam;
    public Camera TitleUICam;
    public Vector3 CameraDistance;
    public Player player;
    public List<Animal> animals;
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
        foreach(var animal in tmp) animals.Add(animal);
        Game.NextGameMode(Game.GAMEMODE.TITLE);
    }

    // Update is called once per frame
    void Update()
    {
        if (Game._GameMode == 0) return;
        GameCameraMove();
        CatchUpAnimal();
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
            foreach (var animal in ins.animals) animal.NextAction(Animal.ACTIONMODE.Idol);
            break;
        case GAMEMODE.PLAY:
            ins.TitleCam.gameObject.SetActive(false);
            ins.TitleUICam.gameObject.SetActive(false);
            ins.GameCam.gameObject.SetActive(true);
            ins.GameUICam.gameObject.SetActive(true);

            ins.player.NextAction(Player.ACTIONMODE.Run);
            foreach (var animal in ins.animals) animal.NextAction(Animal.ACTIONMODE.Run);
            break;
        case GAMEMODE.COLLECTION:break;
        case GAMEMODE.OPTION: break;
        } 
    }
}
