using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCatBack : Animal
{
    public override void Awake()
    {
        speed = 0.4f;
        AnimStateHash = new Dictionary<string, int>() {
            {"Run", Animator.StringToHash("Base Layer.cat_run01")},
            {"Goast", Animator.StringToHash("Base Layer.cat_run01")},
            {"Up", Animator.StringToHash("Base Layer.cat_up01")}
        };
    }

    public override void Start()
    {
       base.Start(); 
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
       base.FixedUpdate(); 
    }
}
