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
        Rot,
        Clear
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
        {"Rot", Animator.StringToHash("Base Layer.Armature|Run")},
        {"Clear", Animator.StringToHash("Base Layer.Clear")}
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
    void Update()
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
            case ACTIONMODE.Idol: break;
            case ACTIONMODE.Run: InputRun(); break;
            case ACTIONMODE.Up: break; 
            case ACTIONMODE.Clear: break; 
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
            case ACTIONMODE.Idol: break;
            case ACTIONMODE.Run: ActionRun(); break;
            case ACTIONMODE.Up:  ActionUp(); break;
            case ACTIONMODE.Clear:  ActionClear(); break;
        }
    }
    void NextActionInit(ACTIONMODE nextAction) {
        ActionMode = nextAction;
        ActionTime = 0.0f;
        switch (ActionMode) {
            case ACTIONMODE.Idol: break;
            case ACTIONMODE.Run: break; 
            case ACTIONMODE.Up: break;
            case ACTIONMODE.Clear: break;
        }
    }
   public void NextAction(ACTIONMODE nextAction) {
        NextActionInit(nextAction);
        switch (ActionMode) {
            case ACTIONMODE.Idol: NextAnimation("Idol"); break;
            case ACTIONMODE.Run: NextAnimation("Run"); break;
            case ACTIONMODE.Up: NextAnimation("Up"); break;
            case ACTIONMODE.Clear: NextAnimation("Clear"); break;
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
    void ActionClear()
    {
    }
 
    void SpeedRun(float speed)
    {
        if (!StandFlag) a_speed = Math.Min(a_speed + speed, SPEED_MAX);
        a_speed = Math.Max(a_speed - friction, 0.0f);
        if (a_speed <= speed) AccRotSpeed = Math.Min(AccRotSpeed + rotSpeed, ROTSPEED_MAX);
        transform.position += a_speed * transform.forward * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRot, AccRotSpeed * Time.deltaTime);
    }
    Vector2 GetInputVector()
    {
        INPUT_TYPE type = GameManager._ins.IsInputGamePad() ? INPUT_TYPE.PAD : INPUT_TYPE.MOUSE;
        Vector2 rangeVec = Vector2.zero;
        switch (type) {
          case INPUT_TYPE.MOUSE: 
            Vector2 nowValue = GameManager.ins.mouseInputValue;
            Vector2 startValue = GameManager.ins.startMouseInputValue;
            Vector2 nowValueFromCenter = new Vector2(nowValue.x - Screen.width/2, nowValue.y - Screen.height/2);
            Vector2 startValueFromCenter = new Vector2(startValue.x - Screen.width/2, startValue.y - Screen.height/2);

            rangeVec = startValueFromCenter - nowValueFromCenter;
            rangeVec = new Vector2((rangeVec.x == 0.0f ? 0.0f : -rangeVec.x), rangeVec.y);
            break;

          case INPUT_TYPE.PAD:
            rangeVec = GameManager.ins.stickInputValue;
            rangeVec = new Vector2(rangeVec.x, (rangeVec.y == 0.0f ? 0.0f : -rangeVec.y));
            break;
        }
        return rangeVec;
    }
    void InputRun()
    {
        Vector2 rangeVec = GetInputVector();

        bool SpdFlag = Math.Abs(rangeVec.x) > 1.0f || Math.Abs(rangeVec.y) > 1.0f;  // 離した距離が1.0f以上なら移動フラグを立てるゆるい判定
        double tan = Math.Atan2(rangeVec.y, rangeVec.x);                 // Vector2から角度取得
        float oldAngle = nextAngle;                                      // 今の角度を旧角度に記録
        nextAngle = (float)(tan * Mathf.Rad2Deg + 90.0f);                // 進行方向取得
        toRot = tan == 0.0f ? transform.rotation : Quaternion.Euler(0, nextAngle, 0); //  EulerをQuaternionへ
        float dist = Math.Abs(Math.Abs(nextAngle) - Math.Abs(oldAngle)); // 角度距離取得
        if (dist >= ROTATION_BRAKE) a_speed = 0.0f;                      // 角度距離が15度超えてたら加速をなしにする

        // 今のプログラムの仕様
        //   まず移動では速度を元に位置と角度を設定してる
        //   入力では角度を更新して、速度で状態変化を見てる
        //     ゲームパッドのレフトスティック中ボタンが押された or 加速と角度が0だったら停止
        // ★: タップしたら停止にしたい
        //Debug.Log($"StandFlag: {StandFlag} / SpdFlag: {SpdFlag} / x:{Math.Abs(rangeVec.x)} / y:{Math.Abs(rangeVec.y)}");

        if(StandFlag && SpdFlag) {
            Debug.Log($"Run");
            StandFlag = false;
            NextAnimation("Run");
        } else if (
          Gamepad.current != null && Gamepad.current.leftStickButton.isPressed || 
          GameManager._ins.IsInputFire ||
          a_speed == 0.0) {
            NextAnimation("Idol");
            a_speed = 0.0f;
            StandFlag = true;
        }
    }
}

