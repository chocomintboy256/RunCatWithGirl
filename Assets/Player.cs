using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
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
    public List<GameObject> targets = new List<GameObject>();
    public Animator animator;
    public Dictionary<string, int> _AnimStateHash = new Dictionary<string, int>() {
        {"Idol", Animator.StringToHash("Base Layer.Armature|Tpose")},
        {"Run", Animator.StringToHash("Base Layer.Armature|Run")},
        {"Up", Animator.StringToHash("Base Layer.Armature|Up")},
        {"Rot", Animator.StringToHash("Base Layer.Armature|Run")}
    };
    public Dictionary<string, int> AnimStateHash {
        get { return _AnimStateHash; }
        set { _AnimStateHash = value; }
    }
    private bool _AnimFlag = false;
    public float SPEED_MAX = 1.5f;
    public float a_speed = 0.0f;
    public float _speed = 0.6f;
    public float ROTATION_BRAKE = 15.0f;
    public float speed {
        get {return _speed;}
        set {_speed = value;
        }
    }
    float nextAngle = 0.0f;
    Quaternion toRot;
    public float rotSpeed = 3.0f;
    public float AccRotSpeed = 0.0f;
    public float ROTSPEED_MAX = 9.0f;
    public bool StandFlag = false;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
         switch (Game.GameMode) {
            case Game.GAMEMODE.TITLE: break;
            case Game.GAMEMODE.PLAY: Input(); Action(); break;
        }       
    }
    void OnTriggerEnter(Collider other){ 
        if (!other.gameObject.TryGetComponent(out Animal comp)) return;
        targets.Add(other.gameObject);
    }
    void OnTriggerExit(Collider other) {
        if (!other.gameObject.TryGetComponent(out Animal comp)) return;
        targets.Remove(other.gameObject);
    }
    void AnimComp() {
        if (_AnimFlag) return;
        CompleteHandler?.Invoke("AnimComp");
        _AnimFlag = true;
    }
    void Input() {
        switch (ActionMode) {
            case ACTIONMODE.Idol: break;
            case ACTIONMODE.Run: InputRun(); break;
            case ACTIONMODE.Up:  break;
        }
    }
    void Action() {
        ActionTime += Time.deltaTime;
        switch (ActionMode) {
            case ACTIONMODE.Idol: break;
            case ACTIONMODE.Run: ActionRun(); break;
            case ACTIONMODE.Up:  break;
        }
    }
    void NextActionInit(ACTIONMODE nextAction) {
        ActionMode = nextAction;
        ActionTime = 0.0f;
        switch (ActionMode) {
            case ACTIONMODE.Idol: break;
            case ACTIONMODE.Run: break;
            case ACTIONMODE.Up:  break;
        } 
    }
    public void NextAction(ACTIONMODE nextAction) {
        NextActionInit(nextAction);
        switch (ActionMode) {
            case ACTIONMODE.Idol: NextAnimation("Idol"); break;
            case ACTIONMODE.Run: NextAnimation("Run"); break;
            case ACTIONMODE.Up: NextAnimation("Up"); break;
        } 
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
    void ActionRun()
    {
        if (!StandFlag) a_speed = Math.Min(a_speed + speed, SPEED_MAX);
        if (a_speed <= speed) AccRotSpeed = Math.Min(AccRotSpeed + rotSpeed, ROTSPEED_MAX);
        transform.position += a_speed * transform.forward * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRot, AccRotSpeed * Time.deltaTime);
    }
    void InputRun()
    {
       if (Gamepad.current == null) return;
        var inp = Gamepad.current.leftStick.ReadValue();
        var tan = Math.Atan2(-inp.y, inp.x);
        float oldAngle = nextAngle;
        nextAngle = (float)(tan * Mathf.Rad2Deg + 90.0f);
        float dist = Math.Abs(Math.Abs(nextAngle) - Math.Abs(oldAngle));
        toRot = tan == 0.0f ? transform.rotation : Quaternion.Euler(0, nextAngle, 0);
        if (dist >= ROTATION_BRAKE) a_speed = 0.0f;

        if (Gamepad.current.leftStickButton.isPressed) {
            NextAnimation("Idol");
            a_speed = 0.0f;
            StandFlag = true;
        } else if(tan != 0.0f && StandFlag) {
            StandFlag = false;
            NextAnimation("Run");
        }
    }
}
