using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class Animal : MonoBehaviour
{
    public int animalDataId;
    public Transform targetTransform;
    public GameObject IconMinimap;
    public AnimalData animalData { get; set; }
    public enum KIND {
        Panda,
        Bear,
        Cat
    }
    public enum ACTIONMODE {
        Idol,
        Run,
        Up,
        Rot,
        Appeal
    }
    public ACTIONMODE ActionMode = ACTIONMODE.Idol;
    public float ActionTime = 0.0f;
    public event Action<string> CompleteHandler;
    public float _speed = 0.8f / 2.0f;
    public float speed {
        get { return _speed; }
        set { _speed = value;
        }
    }
    public const float ROT_NUM = 30.0f;
    public float rotSpeed = 3.0f;
    public Quaternion toRot;
    private Animator animator;
    public AnimationClip[] motions;
    public Dictionary<string, AnimationClip> MotionNameToAnimationClip = new Dictionary<string, AnimationClip>() { };
    private Regex nameReg = new Regex("panda|bear", RegexOptions.IgnoreCase);

    // Start is called before the first frame update
    public static GameObject InstanceWithInit(KIND kind, Vector3 vec3 = default(Vector3), ACTIONMODE actionMode = ACTIONMODE.Idol, bool minimapMarkerFg = false)
    {
        AnimalData ad = AnimalDataBase.AnimalMaster.First(x => x.kind == kind);
        //GameObject animalContainer = Instantiate(AnimalDataBase.AnimalContainer, vec3, Quaternion.identity);
        //GameObject animalFbxContainer = Instantiate(ad.fbx, Vector3.zero, Quaternion.identity, animalContainer.transform);
        GameObject animalFbxContainer = Instantiate(ad.fbx, vec3, Quaternion.identity);
        Animal animal = animalFbxContainer.GetComponent<Animal>();
        animal.AnimalInit(ad, actionMode);
        animal.IconMinimap.SetActive(minimapMarkerFg);
        //return animalContainer;
        return animalFbxContainer;
    }
    private void AnimalInit(AnimalData ad, ACTIONMODE actionMode)
    {
        //targetTransform = transform.parent;
        targetTransform = transform;
        animalDataId = ad.id;
        AnimalFBX animalFBX = gameObject.GetComponentInChildren<AnimalFBX>();
        animalFBX.AnimCompHandler += AnimationComplete;
        NextAction(actionMode);
    }
    public virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        //NextAction(ActionMode);
    }
    public virtual void Start()
    {
        speed = 0.4f;
        // rotSpeed = 3.0f / Game.fps;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
       switch (GameManager.GameMode) {
            case GameManager.GAMEMODE.TITLE: break;
            case GameManager.GAMEMODE.PLAY: Action(); break;
        }
    }
    void Action() {
        ActionTime += Time.deltaTime;
        switch (ActionMode) {
            case ACTIONMODE.Idol: break;
            case ACTIONMODE.Run: ActionRun(); break;
            case ACTIONMODE.Up:  break;
            case ACTIONMODE.Rot: ActionRotation(); break;
            case ACTIONMODE.Appeal:  break;
        }
    }
    void NextActionInit(ACTIONMODE nextAction) {
        ActionMode = nextAction;
        ActionTime = 0.0f;
        switch (ActionMode) {
            case ACTIONMODE.Idol: break;
            case ACTIONMODE.Run: break;
            case ACTIONMODE.Up:  break;
            case ACTIONMODE.Rot: ActionRotationInit();  break;
            case ACTIONMODE.Appeal:  break;
        } 
    }
    public void NextAction(ACTIONMODE nextAction) {
        NextActionInit(nextAction);
        switch (ActionMode) {
            case ACTIONMODE.Idol: NextAnimation("Idol"); break;
            case ACTIONMODE.Run: NextAnimation("Run"); break;
            case ACTIONMODE.Up: NextAnimation("Up"); break;
            case ACTIONMODE.Rot: NextAnimation("Rot"); break;
            case ACTIONMODE.Appeal: NextAnimation("Appeal"); break;
        }
    }
    public void NextAction(ACTIONMODE nextAction, Action<string> comp) {
        NextAction(nextAction);
        CompleteHandler += comp;
    }
    void ActionRotationInit() {
        float rotDir = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        float nextAngle = targetTransform.eulerAngles.y + ROT_NUM * rotDir;
        toRot = Quaternion.Euler(0, nextAngle, 0);
    }
    void ActionRotation()
    {
        Quaternion nowRot = targetTransform.rotation;
		Quaternion nextRot = Quaternion.Slerp(nowRot, toRot, rotSpeed * Time.deltaTime);
        transform.rotation = nextRot;
        float dis = Math.Abs(Math.Abs(nextRot.y) - Math.Abs(toRot.y));
        if (dis <= 0.1) {
            NextAction(ACTIONMODE.Run);
        }
    }
   void ActionRun()
    {
        targetTransform.position += targetTransform.forward * speed * Time.deltaTime;
        if (ActionTime >= 3.0f) NextAction(ACTIONMODE.Rot);
    }
    private void OnDestroy()
    {
        GetComponentInChildren<AnimalFBX>().AnimCompHandler -= AnimationComplete;
        CancelInvoke();
    }

    private void AnimationComplete() {
        CompleteHandler?.Invoke("AnimationComplete");
    }
    private void NextAnimation(string label)
    {
        if (animator)
        {
            animator.SetBool("RunBool", false); 
            animator.SetBool("RotBool", false); 
            animator.SetBool("AppealBool", false); 
            switch(label) { 
                case "Idol":
                    break;
                case "Up": 
                    animator.SetTrigger("UpTrigger"); 
                   break;
                case "Run": animator.SetBool("RunBool", true); break;
                case "Rot": animator.SetBool("RotBool", true); break;
                case "Appeal": animator.SetBool("AppealBool", true); break;
            }
        }
    }
    private void NextAnimation(string label, Action<string> comp)
    {
        NextAnimation(label);
        CompleteHandler += comp;
    }
}
