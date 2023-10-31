using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDoTween : MonoBehaviour
{
    static TextMeshProUGUI tmp1;
    static TextDoTween self;
    // Start is called before the first frame update
    void Start()
    {
        self = this;
        tmp1 = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void EffectStageStartCountDown(Action act)
    {
        self.EffectScaleText(act);
        // Debug.Log("effect end");
    }
    public void EffectScaleText(Action act)
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
            act();
        });
    }

}
