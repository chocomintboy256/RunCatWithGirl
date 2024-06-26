using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimalFBX : MonoBehaviour
{
    private Animal animal;
    private bool _AnimFlag = false;
    public UnityAction AnimCompHandler;

    // FBXイベント一覧
    void AnimComp()
    {
        if (_AnimFlag) return;
        AnimCompHandler.Invoke();
        _AnimFlag = true;
    }

    // void Start() { } 
    // void Update() { }
}
