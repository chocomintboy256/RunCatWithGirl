using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

// カメラコントローラ 
public class CameraController : MonoBehaviour
{
    // マウスの開始位置とカメラの開始角度 
    private Vector2 mouseStartPos;
    private Vector3 camStartRot;

    // フレーム毎に呼ばれる
    public void Update()
    {
        Transform camTrans = this.gameObject.transform;

        // カメラの位置の操作 (WSADQE)
        var keyboard = Keyboard.current;
        if (keyboard != null) {
            // カメラ位置の取得
            Vector3 camPos = transform.position;

            // キー押下中の処理
            if (keyboard.wKey.isPressed) {camPos += camTrans.forward * Time.deltaTime * 5.0f;}
            if (keyboard.sKey.isPressed) {camPos -= camTrans.forward * Time.deltaTime * 5.0f;}
            if (keyboard.aKey.isPressed) {camPos -= camTrans.right   * Time.deltaTime * 5.0f;}
            if (keyboard.dKey.isPressed) {camPos += camTrans.right   * Time.deltaTime * 5.0f;}
            if (keyboard.qKey.isPressed) {camPos -= camTrans.up      * Time.deltaTime * 5.0f;}
            if (keyboard.eKey.isPressed) {camPos += camTrans.up      * Time.deltaTime * 5.0f;}

            // カメラ位置の更新
            this.gameObject.transform.position = camPos;
        }

        // カメラの角度の操作 (マウス)
        var mouse = Mouse.current;
        if (mouse != null) {
            // マウス位置の取得
            Vector2 mousePos = mouse.position.ReadValue();

            // マウス左ボタンの押下時の処理
            if (mouse.leftButton.wasPressedThisFrame) {
                // マウスの開始位置とカメラの開始角度の取得
                this.mouseStartPos = mousePos;
                this.camStartRot.x = camTrans.transform.eulerAngles.x;
                this.camStartRot.y = camTrans.transform.eulerAngles.y;
            }

            // マウス左ボタンの押下中の処理
            if (mouse.leftButton.isPressed) {
                // カメラの角度の更新
                float x = (this.mouseStartPos.x - mousePos.x) / Screen.width;
                float y = (this.mouseStartPos.y - mousePos.y) / Screen.height;
                camTrans.rotation = Quaternion.Euler(
                    this.camStartRot.x + y * 90f, this.camStartRot.y + x * 90f, 0);
            }
        }
    }
}
