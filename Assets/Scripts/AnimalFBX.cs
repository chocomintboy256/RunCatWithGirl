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

    // FBXƒCƒxƒ“ƒgˆê——
    void AnimComp()
    {
        if (_AnimFlag) return;
        AnimCompHandler.Invoke();
        _AnimFlag = true;
    }

    // void Start() { } 
    // void Update() { }
}
