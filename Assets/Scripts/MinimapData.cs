using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapData : MonoBehaviour
{
    public GameObject Go;
    public GameObject IconTri;
    public GameObject IconCube;
    public MinimapData(GameObject go, GameObject iconTri, GameObject iconCube)
    {
        Go = go;
        IconTri = iconTri;
        IconCube = iconCube;
    }

    //void Start() {}
}
