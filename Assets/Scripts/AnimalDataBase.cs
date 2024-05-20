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
    [Header("�X�N���v�g�ŃX�e�[�W�ɐ�������fbx�̎Q�� �����̑��̓����f�[�^�̓\�[�X���Œ��ڕύX���Ă�������")]
    public GameObject _AnimalContainer;
    public static GameObject AnimalContainer { get { return ins._AnimalContainer; } set { } }
    public GameObject[] AnimalFbxContainers;

    public static AnimalData[] AnimalMaster { get { return ins._AnimalMaster; } set { } }
    private AnimalData[] _AnimalMaster = {
        new AnimalData(id: 0, fbx: null, cname: "�p���_", kind: Animal.KIND.Panda, catchword: "�p���_�͂��₵", detail: "�ǂ��ɂł�����p���_�B����H�ׂ�炵�����ǂ��̕ӂ�ɂ͐����ĂȂ��B���������ǂ��ŐH�������Ă���񂾂낤��"),
        new AnimalData(id: 0, fbx: null, cname: "����", kind: Animal.KIND.Bear, catchword: "���\����Ȃ�����", detail: "��肩�������Ȃ��܁B���ǂ����Ăق������ǐl�Ԃ��|���̂ł����ɓ�����B�߂��̃p���_�̂��Ƃ��Z�킾�Ǝv���Ă���"),
        new AnimalData(id: 0, fbx: null, cname: "�˂�", kind: Animal.KIND.Cat, catchword: "�������ł��l�R", detail: "���܂ɃG�T���˂���ɂ���d�������킢���l�R�B���̕ӂ�̃l�R�͖ѕ��݂������̂ł����ƋC���������炵��")
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
