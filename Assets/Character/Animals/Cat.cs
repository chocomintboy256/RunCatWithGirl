using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Animal
{
    public override void Awake()
    {
        speed = 0.6f;
        base.Awake(); 
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
