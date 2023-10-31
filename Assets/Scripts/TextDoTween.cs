using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDoTween : MonoBehaviour
{
    TextMeshProUGUI tmp1;
    Coroutine _someCorutione;
    // Start is called before the first frame update
    void Start()
    {
        tmp1 = gameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(EffectStageStartCountDown());
        Debug.Log("start end");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator EffectStageStartCountDown()
    {
        _someCorutione = StartCoroutine(EffectScaleText());
        yield return _someCorutione;
        Debug.Log("effect end");
    }
    public IEnumerator EffectScaleText()
    {
        transform.DOScale(4.0f, 1.0f)
    ﻿﻿﻿﻿﻿    .SetEase(Ease.OutQuint)
        .SetLoops(3)
        .OnStepComplete(() => { 
            tmp1.text = "" + (float.Parse(tmp1.text) - 1.0f); 
            Debug.Log(tmp1.text);
        })
        .OnComplete(() => {
            gameObject.SetActive(false);
            Debug.Log("effect complete");
            StopCoroutine(_someCorutione);
        });


        WaitForSeconds wait = new WaitForSeconds(1);
        while(true) {
            yield return wait;
        }
    }

}
