using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInfo : MonoBehaviour
{
    static readonly bool isAndroid = Application.platform == RuntimePlatform.Android;
    static readonly bool isIOS = Application.platform == RuntimePlatform.IPhonePlayer;

    public static bool IsMobile() {
        // AndroidかiOSか、あるいはUnity RemoteだったらMobile扱いとする
    #if UNITY_EDITOR
        bool ret = UnityEditor.EditorApplication.isRemoteConnected;
    #else
        bool ret = isAndroid || isIOS;
    #endif
        return ret;
    }

}