using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlink : MonoBehaviour
{
    public RectTransform upLid;
    public RectTransform downLid;
    
    // public static EyeBlink instance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlinkEyes(float blinkValue)
    {
        //1190 - 300
        upLid.anchoredPosition = new Vector2(0, 300 + blinkValue * 890);
        downLid.anchoredPosition = new Vector2(0, -blinkValue * 890 - 300);
    }
}
