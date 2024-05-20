using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAnimalData
{
    private bool _active;
    private float _x;
    private float _z;
    private Animal.KIND _kind;

    public bool active { get { return _active; } set { _active = value; } }
    public float x { get { return _x; } set { _x = value; } }
    public float z { get { return _z; } set { _z = value; } }
    public Animal.KIND kind { get { return _kind; } set { _kind= value; } }
    public StageAnimalData(bool active, float x, float z, Animal.KIND kind)
    {
        _active = active;
        _x = x;
        _z = z;
        _kind = kind;
    }
}
