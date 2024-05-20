using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDataBase : MonoBehaviour
{
    private static AnimalDataBase _ins;
    public static AnimalDataBase ins {
        get { return _ins; }
        set { 
            if (_ins) return; 
            _ins = value; 
        }
    }
    [Header("スクリプトでステージに生成するfbxの参照 ※その他の動物データはソース内で直接変更してください")]
    public GameObject _AnimalContainer;
    public static GameObject AnimalContainer { get { return ins._AnimalContainer; } set { } }
    public GameObject[] AnimalFbxContainers;

    public static AnimalData[] AnimalMaster { get { return ins._AnimalMaster; } set { } }
    private AnimalData[] _AnimalMaster = {
        new AnimalData(id: 0, fbx: null, cname: "パンダ", kind: Animal.KIND.Panda, catchword: "パンダはいやし", detail: "どこにでもいるパンダ。笹を食べるらしいけどこの辺りには生えてない。いったいどこで食事をしているんだろうか"),
        new AnimalData(id: 0, fbx: null, cname: "くま", kind: Animal.KIND.Bear, catchword: "狂暴じゃないくま", detail: "わりかし温厚なくま。仲良くしてほしいけど人間が怖いのですくに逃げる。近くのパンダのことを兄弟だと思っている"),
        new AnimalData(id: 0, fbx: null, cname: "ねこ", kind: Animal.KIND.Cat, catchword: "動きが固いネコ", detail: "たまにエサをねだりにくる仕草がかわいいネコ。この辺りのネコは毛並みがいいのでさわると気持ちいいらしい")
    };

    private void Awake()
    {
        ins = this;
        for (var i = 0; i < AnimalFbxContainers.Length; i++) {
            AnimalMaster[i].id = i+1;
            AnimalMaster[i].fbx = AnimalFbxContainers[i];
        }
    }
}
