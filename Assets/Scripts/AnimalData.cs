using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalData
{
    public int id { get; set; }
    public GameObject fbx { get; set; }
    public string cname { get; set; }
    public Animal.KIND kind { get; set; }
    public string catchword { get; set; }
    public string detail { get; set; }
    public AnimalData(int id, GameObject fbx, string cname, Animal.KIND kind, string catchword, string detail)
    {
        this.id = id;
        this.fbx = fbx;
        this.cname = cname;
        this.kind = kind;
        this.catchword = catchword;
        this.detail = detail;
    }
}
