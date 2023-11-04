using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public EyeBlink eyeblink;
    private float eyeValue;
    private bool isSlept;
    
    void Start()
    {
        isSlept = false;
        eyeValue = 1;
    }
    
    void Update()
    {
        eyeblink.BlinkEyes(eyeValue);
        
        if (Input.GetKeyDown(KeyCode.Space) && !isSlept)
        {
            DOTween
                .To(() => eyeValue, x => eyeValue = x, 0, 4)
                .OnComplete(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); });
            GetComponent<AudioSource>().Play();
            isSlept = true;
        }
    }
}
