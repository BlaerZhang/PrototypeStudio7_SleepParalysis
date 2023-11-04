using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WakeupFadein : MonoBehaviour
{
    public GameManager gameManager;
    private bool isGameStarted;
    
    void Start()
    {
        isGameStarted = false;
        Invoke("FadeIn", 3f);
    }  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGameStarted)
        {
            CancelInvoke("FadeIn");
            GetComponent<SpriteRenderer>().DOPause();
            GetComponent<SpriteRenderer>().DOFade(0, 2);
            isGameStarted = true;
        }
    }

    void FadeIn()
    {
        GetComponent<SpriteRenderer>().DOFade(1, 10f);
    }
}
