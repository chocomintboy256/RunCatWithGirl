using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float _speed = 0.6f;
    public float speed {
        get {return _speed;}
        set {_speed = value;
        }
    }
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
            case Game.GAMEMODE.PLAY: Action(); break;
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
        transform.position += speed * Vector3.forward * Time.deltaTime;
    }
}
