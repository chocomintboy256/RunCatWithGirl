using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using System;
using System.Text;

public class JoystickInfo : MonoBehaviour
{
  /// <summary>情報を表示させるテキストオブジェクト。</summary>
  [SerializeField] private Text TextObject;

  StringBuilder Builder = new StringBuilder();

  // 更新はフレームごとに1回呼び出されます
  void Update()
  {
    if (TextObject == null)
    {
      Debug.Log($"{nameof(TextObject)} が null です。");
      return;
    }

    // １つ目のジョイスティックの情報を取得
    var joystick = Joystick.current;
    if (joystick == null)
    {
      Debug.Log("ジョイスティックがありません。");
      TextObject.text = "";
      return;
    }

    Builder.Clear();

    // ジョイスティックの情報取得
    Builder.AppendLine($"deviceId:{joystick.deviceId}");
    Builder.AppendLine($"name:{joystick.name}");
    Builder.AppendLine($"displayName:{joystick.displayName}");

    // ジョイスティックにはボタンの大部分が定義されていないので
    // 基本的には allControls で列挙して入力の種類と値を確認していく
    foreach (var key in joystick.allControls)
    {
      if (key is StickControl stick)
      {
        if (stick.up.isPressed) Builder.AppendLine($"{key.name} Up");
        if (stick.down.isPressed) Builder.AppendLine($"{key.name} Down");
        if (stick.left.isPressed) Builder.AppendLine($"{key.name} Left");
        if (stick.right.isPressed) Builder.AppendLine($"{key.name} Right");

        var value = stick.ReadValue();
        if (value.magnitude > 0f)
        {
          Builder.AppendLine($"{key.name}:{value.normalized * value.magnitude}");
        }
      }
      else if (key is Vector2Control vec2)
      {
        var value = vec2.ReadValue();
        if (value.magnitude > 0f)
        {
          Builder.AppendLine($"{key.name}:{value.normalized * value.magnitude}");
        }
        if (vec2.x.ReadValue() != 0f)
        {
          Builder.AppendLine($"{key.name}.x:{vec2.x.ReadValue()}");
        }
        if (vec2.y.ReadValue() != 0f)
        {
          Builder.AppendLine($"{key.name}.x:{vec2.y.ReadValue()}");
        }
      }
      else if (key is ButtonControl button)
      {
        if (button.isPressed) Builder.AppendLine($"{key.name} isPress");
        var value = button.ReadValue();
        if (value != 0f)
        {
          Builder.AppendLine($"{key.name}:{value}");
        }
      }
      else if (key is AxisControl axis)
      {
        if (axis.IsPressed()) Builder.AppendLine($"{key.name} isPress");

        var value = axis.ReadValue();
        if (value != 0f)
        {
          Builder.AppendLine($"{key.name}:{value}");
        }
      }
      else
      {
        Builder.AppendLine($"Type={key.GetType()}");
      }
    }

    // 押しているボタン一覧をテキストで表示
    TextObject.text = Builder.ToString();
  }
}