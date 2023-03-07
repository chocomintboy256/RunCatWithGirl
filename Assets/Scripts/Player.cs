using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum INPUT_TYPE {
        MOUSE,
        PAD
    }
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
        {"Idol", Animator.StringToHash("Base Layer.Armature|Idol")},
        {"Run", Animator.StringToHash("Base Layer.Armature|Run")},
        {"Up", Animator.StringToHash("Base Layer.Armature|Up")},
        {"Rot", Animator.StringToHash("Base Layer.Armature|Run")}
    };
    public Dictionary<string, int> AnimStateHash {
        get { return _AnimStateHash; }
        set { _AnimStateHash = value; }
    }
    public float friction = 0.05f;
    public float SPEED_MAX = 1.5f;
    public float a_speed = 0.0f;
    public float _speed = 0.2f; 
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
    public Camera GameCamera;
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
         switch (GameManager.GameMode) {
            case GameManager.GAMEMODE.TITLE:
                break;
            case GameManager.GAMEMODE.PLAY: 
                Input();
                Action();
                break;
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
    //-- from FBX import setting Event
    void AnimComp() {
        CompleteHandler?.Invoke("AnimComp");
        CompleteHandler -= CompleteHandler;
    }
    void Input() {
        switch (ActionMode) {
            case ACTIONMODE.Idol: 
                break;
            case ACTIONMODE.Run: 
                InputRun();
                break;
            case ACTIONMODE.Up:
                break;
        }
    }
    public void PubAction() {
        int aa = 1;
        aa = aa + 1;
        Action();
    }
    private void Action() {
        ActionTime += Time.deltaTime;
        switch (ActionMode) {
            case ACTIONMODE.Idol:
                break;
            case ACTIONMODE.Run: 
                ActionRun();
                break;
            case ACTIONMODE.Up:  
                ActionUp();
                break;
        }
    }
    void NextActionInit(ACTIONMODE nextAction) {
        ActionMode = nextAction;
        ActionTime = 0.0f;
        switch (ActionMode) {
            case ACTIONMODE.Idol:
                break;
            case ACTIONMODE.Run:
                break;
            case ACTIONMODE.Up:
                break;
        }
    }
   public void NextAction(ACTIONMODE nextAction) {
        NextActionInit(nextAction);
        switch (ActionMode) {
            case ACTIONMODE.Idol: 
                NextAnimation("Idol"); 
                break;
            case ACTIONMODE.Run: 
                NextAnimation("Run");
                break;
            case ACTIONMODE.Up:
                NextAnimation("Up");
                break;
        } 
    }
    public void NextAction(ACTIONMODE nextAction, Action<string> comp) {
        NextAction(nextAction);
        CompleteHandler += comp;
    }
    private void NextAnimation(string label)
    {
        animator.Play(AnimStateHash[label], 0, 0.0f); 
    }
    private void NextAnimation(string label, Action<string> comp)
    {
        animator.Play(AnimStateHash[label], 0, 0.0f); 
        CompleteHandler += comp;
    }
    void ActionUp()
    {
        SpeedRun(speed/2.5f);
    }
    void ActionRun()
    {
       SpeedRun(speed); 
    }
    void SpeedRun(float speed)
    {
        if (!StandFlag) a_speed = Math.Min(a_speed + speed, SPEED_MAX);
        a_speed = Math.Max(a_speed - friction, 0.0f);
        if (a_speed <= speed) AccRotSpeed = Math.Min(AccRotSpeed + rotSpeed, ROTSPEED_MAX);
        transform.position += a_speed * transform.forward * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRot, AccRotSpeed * Time.deltaTime);
    }
    double InputMobile()
    {
        Vector2 mouseInputValue = GameManager.ins.mouseInputValue;
        Vector3 mouseFromCenter = new Vector3(mouseInputValue.x - Screen.width/2, mouseInputValue.y - Screen.height/2, 0.0f);
        Vector3 playerPosition = gameObject.transform.position;
        Vector3 screenPlayerPosition = Camera.main.WorldToScreenPoint(playerPosition);
        Vector3 screenCenterPosition = new Vector3(Screen.width/2, Screen.height/2, 0.0f);
        Vector3 playerFromCenter = screenPlayerPosition - screenCenterPosition;
        // Vector3 v3 = new Vector3(mouseInputValue.x + centerToPlayer.x, mouseInputValue.y + centerToPlayer.y, 1.0f);
        //Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint();
        //Vector3 rangeVec = worldMousePosition - playerPosition - new Vector3(0.0f, 0.0f, 1.5f);
        Vector3 rangeVec = mouseFromCenter - playerFromCenter;
        var tan = Math.Atan2(-rangeVec.y, rangeVec.x);
        var angle = tan * (180/Math.PI);
        // Debug.Log($"spp:{screenPlayerPosition} m:{mouseInputValue} / p:{playerPosition} r:{rangeVec} t:{tan} a:{angle} sw:{Screen.width} sh:{Screen.height}");
        Debug.Log($"m:{mouseFromCenter} p:{playerFromCenter} r:{rangeVec} / t:{tan} a:{angle}");
        return tan;
    }
    double InputUnity()
    {
        Vector2 mouseInputValue = GameManager.ins.mouseInputValue;
        Vector3 playerPosition = gameObject.transform.position;
        Vector3 CameraPosition = Camera.main.transform.position;
        Vector3 screenCameraPosition = Camera.main.WorldToScreenPoint(CameraPosition);
        Vector3 screenPlayerPosition = Camera.main.WorldToScreenPoint(playerPosition);
        Vector3 screenPosition = screenPlayerPosition - new Vector3(Screen.width/2, Screen.height/2, 0.0f);
        Vector3 v3 = new Vector3(mouseInputValue.x + screenPosition.x, mouseInputValue.y + screenPosition.y, 1.0f);
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(v3);
        Vector3 rangeVec = worldMousePosition - playerPosition - new Vector3(0.0f, 0.0f, 1.5f);
        rangeVec.y = 0.0f;
        var tan = Math.Atan2(-rangeVec.z, rangeVec.x);
        var angle = tan * (180/Math.PI);
        // Debug.Log($"cp:{CameraPosition} scp:{screenCameraPosition} spp:{screenPlayerPosition} sp:{screenPosition} m:{_mouseInputValue} v3: {v3} \nw: {worldMousePosition} p:{playerPosition} icp:{_initCameraPos} r:{rangeVec} t:{tan} a:{angle}");
        return tan;
    }
    double GetInputTangent(INPUT_TYPE type)
    {
        double tan = 0.0;
        switch (type) {
            case INPUT_TYPE.MOUSE:
                tan = PlatformInfo.IsMobile() ?
                    InputMobile() :
                    InputUnity();
                break;
            case INPUT_TYPE.PAD:
                Vector2 stickInputValue = GameManager.ins.stickInputValue;
                tan = Math.Atan2(-stickInputValue.y, stickInputValue.x);                   // 入力からタンジェント取得
                Debug.Log($"x: {stickInputValue.x} y: {stickInputValue.y} tan: {tan}");
                break;
       }
        return tan;
    }
    void InputRun()
    {
        double tan = GetInputTangent(GameManager._ins.IsInputGamePad() ? INPUT_TYPE.PAD : INPUT_TYPE.MOUSE);
        float oldAngle = nextAngle;                                      // 今の角度を旧角度に記録
        nextAngle = (float)(tan * Mathf.Rad2Deg + 90.0f);                // 進行方向取得
        toRot = tan == 0.0f ? transform.rotation : Quaternion.Euler(0, nextAngle, 0); //  EulerをQuaternionへ
        float dist = Math.Abs(Math.Abs(nextAngle) - Math.Abs(oldAngle)); // 角度距離取得
        if (dist >= ROTATION_BRAKE) a_speed = 0.0f;                      // 角度距離が15度超えてたら加速をなしにする

        if (Gamepad.current != null && Gamepad.current.leftStickButton.isPressed || a_speed == 0.0 && tan == 0.0) {
            NextAnimation("Idol");
            //a_speed = 0.0f;
            StandFlag = true;
        } else if(tan != 0.0f && StandFlag) {
            StandFlag = false;
            NextAnimation("Run");
        }
    }
}

