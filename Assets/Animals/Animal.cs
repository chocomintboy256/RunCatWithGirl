using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal: MonoBehaviour
{
    public enum ACTIONMODE {
        Idol,
        Run,
        Up,
        Rot
    }
    public ACTIONMODE ActionMode = ACTIONMODE.Idol;
    public float ActionTime = 0.0f;
    public event Action<string> CompleteHandler;
    public float _speed = 0.8f;
    public float speed {
        get {return _speed;}
        set {_speed = value;
        }
    }
    public const float ROT_NUM = 30.0f;
    public float rotSpeed = 3.0f;
    public Quaternion toRot;
    public Animator animator;
    public Dictionary<string, int> _AnimStateHash = new Dictionary<string, int>() {
        {"Idol", Animator.StringToHash("Base Layer.アーマチュア|Idol")},
        {"Run", Animator.StringToHash("Base Layer.アーマチュア|Run")},
        {"Up", Animator.StringToHash("Base Layer.アーマチュア|Up")},
        {"Rot", Animator.StringToHash("Base Layer.アーマチュア|Run")}
    };
    public Dictionary<string, int> AnimStateHash {
        get { return _AnimStateHash; }
        set { _AnimStateHash = value; }
    }

    private bool _AnimFlag = false;
    // Start is called before the first frame update
    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public virtual void Start()
    {
        // rotSpeed = 3.0f / Game.fps;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        switch (Game.GameMode) {
            case Game.GAMEMODE.TITLE: break;
            case Game.GAMEMODE.PLAY: Action(); break;
        }
    }
    void Action() {
        ActionTime += Time.deltaTime;
        switch (ActionMode) {
            case ACTIONMODE.Idol: break;
            case ACTIONMODE.Run: ActionRun(); break;
            case ACTIONMODE.Up:  break;
            case ACTIONMODE.Rot: ActionRotation(); break;
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
        } 
    }
    public void NextAction(ACTIONMODE nextAction) {
        NextActionInit(nextAction);
        switch (ActionMode) {
            case ACTIONMODE.Idol: NextAnimation("Idol"); break;
            case ACTIONMODE.Run: NextAnimation("Run"); break;
            case ACTIONMODE.Up: NextAnimation("Up"); break;
            case ACTIONMODE.Rot: NextAnimation("Rot"); break;
        }
    }
    void ActionRotationInit() {
        float rotDir = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        float nextAngle = transform.eulerAngles.y + ROT_NUM * rotDir;
        toRot = Quaternion.Euler(0, nextAngle, 0);
    }
    void ActionRotation()
    {
        Quaternion nowRot = transform.rotation;
		Quaternion nextRot = Quaternion.Slerp(nowRot, toRot, rotSpeed * Time.deltaTime);
        transform.rotation = nextRot;
        float dis = Math.Abs(Math.Abs(nextRot.y) - Math.Abs(toRot.y));
        if (dis <= 0.1) {
            NextAction(ACTIONMODE.Run);
        }
    }
   void ActionRun()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if (ActionTime >= 3.0f) NextAction(ACTIONMODE.Rot);
    }
    private void OnDestroy()
    {
        CancelInvoke();
    }

    void AnimComp() {
        if (_AnimFlag) return;
        CompleteHandler?.Invoke("AnimComp");
        _AnimFlag = true;
    }
    public void NextAnimation(string label)
    {
        animator.Play(AnimStateHash[label], 0, 0.0f); 
    }
    public void NextAnimation(string label, Action<string> comp)
    {
        animator.Play(AnimStateHash[label], 0, 0.0f); 
        CompleteHandler += comp;
    }
}
