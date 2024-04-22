using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataAnimal : MonoBehaviour
{
    private bool _active;
    private float _x;
    private float _z;

    public bool active { get { return _active; } set { _active = value; } }
    public float x { get { return _x; } set { _x = value; } }
    public float z { get { return _z; } set { _z = value; } }
    public StageDataAnimal(bool active, float x, float z)
    {
        _active = active;
        _x = x;
        _z = z;
    }
    //void Start() { }
    //void Update() { }
}
