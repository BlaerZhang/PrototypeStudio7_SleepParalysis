using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float fakeEyeValue;
    [Range(0, 1)] public float eyeValue;

    public Animator leftHand;
    [Range(0, 1)] public float leftHandValue;
    
    public Animator rightHand;
    [Range(0, 1)] public float rightHandValue;
    
    public Animator leftLeg;
    [Range(0, 1)] public float leftLegValue;
    
    public Animator rightLeg;
    [Range(0, 1)] public float rightLegValue;

    [Range(0, 1)] public float deathValue;
    [Range(0, 1)] public float wakeValue;

    public EyeBlink fakeEyeBlink;
    public EyeBlink eyeBlink;

    public AudioMixer mixer;

    public Volume globalVolume;

    public bool isGameStarted;

    public bool isDead;

    public bool isAwake;

    private bool isGameStateChanged;

    public Image redMask;

    public Image whiteMask;

    void Start()
    {
        isGameStateChanged = false;
        isGameStarted = false;
        isDead = false;
        isAwake = false;
        fakeEyeValue = 0;
    }


    void Update()
    {
        LimitValueRange();
        SetAnimatorValue();
        mixer.SetFloat("MasterVolume", -75 + 72 * Mathf.Sqrt(deathValue));
        
        fakeEyeBlink.BlinkEyes(fakeEyeValue);
        if (Input.GetKeyDown(KeyCode.Space) && !isGameStarted)
        {
            Sequence intro = DOTween.Sequence();
            intro
                .AppendInterval(2)
                .Append(DOTween.To(() => fakeEyeValue, x => fakeEyeValue = x, 1, 1f))
                .Play();
            
            Invoke("ShowKeys", 5f);
            AudioSource[] audios = GetComponents<AudioSource>();
            foreach (var audioSource in audios)
            {
                audioSource.Play();
            }
            isGameStarted = true;
        }

        if (isGameStarted && !isAwake && !isDead)
        {
            wakeValue = eyeValue * 0.25f + leftHandValue * 0.25f + rightHandValue * 0.25f + leftLegValue * 0.25f +
                        rightLegValue * 0.25f;
            
            eyeBlink.BlinkEyes(wakeValue);
            globalVolume.weight = deathValue;

            eyeValue -= Time.deltaTime / 5;
            leftHandValue -= Time.deltaTime / 5;
            rightHandValue -= Time.deltaTime / 5;
            leftLegValue -= Time.deltaTime / 5;
            rightLegValue -= Time.deltaTime / 5;
            deathValue -= Time.deltaTime / 1.5f;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                eyeValue += 0.1f;
                deathValue += 0.075f;
            }
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                leftHandValue += 0.1f;
                deathValue += 0.05f;
            }
            
            if (Input.GetKeyDown(KeyCode.M))
            {
                rightHandValue += 0.1f;
                deathValue += 0.05f;
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                leftLegValue += 0.1f;
                deathValue += 0.05f;
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                rightLegValue += 0.1f;
                deathValue += 0.05f;
            }
        }

        if (deathValue >= 1)
        {
            isDead = true;
            ChangeGameState();
        }

        if (wakeValue >= 1)
        {
            isAwake = true;
            ChangeGameState();
        }
    }
    
    void LimitValueRange()
    {
        if (eyeValue >= 1) eyeValue = 1;
        if (eyeValue <= 0) eyeValue = 0;
        
        if (leftHandValue >= 1) leftHandValue = 1;
        if (leftHandValue <= 0) leftHandValue = 0;
        
        if (rightHandValue >= 1) rightHandValue = 1;
        if (rightHandValue <= 0) rightHandValue = 0;
        
        if (leftLegValue >= 1) leftLegValue = 1;
        if (leftLegValue <= 0) leftLegValue = 0;
        
        if (rightLegValue >= 1) rightLegValue = 1;
        if (rightLegValue <= 0) rightLegValue = 0;
        
        if (deathValue >= 1) deathValue = 1;
        if (deathValue <= 0) deathValue = 0;
        
        if (wakeValue >= 1) wakeValue = 1;
        if (wakeValue <= 0) wakeValue = 0;
    }

    void SetAnimatorValue()
    {
        leftHand.SetFloat("Value", leftHandValue);
        rightHand.SetFloat("Value", rightHandValue);
        leftLeg.SetFloat("Value", leftLegValue);
        rightLeg.SetFloat("Value", rightLegValue);
    }

    void ShowKeys()
    {
        GameObject.Find("Keys").GetComponent<SpriteRenderer>().DOFade(1,2f);
    }

    void ChangeGameState()
    {
        if (!isGameStateChanged)
        {
            if (isAwake && isDead)
            {
                isAwake = false;
                isDead = true;
                ChangeGameState();
            }
            else
            {
                if (isAwake)
                {
                    AudioSource[] audios = GetComponents<AudioSource>();
                    foreach (var audioSource in audios)
                    {
                        audioSource.DOFade(0,2f);
                    }
                    
                    Sequence awakeEnding = DOTween.Sequence();
                    awakeEnding
                        .Append(whiteMask.DOFade(1, 2f))
                        .Insert(0, DOTween.To(() => globalVolume.weight, x => globalVolume.weight = x, 0, 2f))
                        .Insert(0, DOTween.To(() => GameObject.Find("TV Effect").GetComponent<Volume>().weight, x => GameObject.Find("TV Effect").GetComponent<Volume>().weight = x, 0, 2f))
                        .Append(GameObject.Find("Reality").GetComponent<Image>().DOFade(1, 0))
                        .Append(whiteMask.DOFade(0, 2f))
                        .AppendInterval(4)
                        .Append(whiteMask.DOFade(1, 0))
                        .Append(whiteMask.DOColor(Color.black, 0))
                        .Append(GameObject.Find("Credit").GetComponent<SpriteRenderer>().DOFade(1, 0f));

                    awakeEnding.Play();
                        
                    DOTween.To(() => deathValue, x => deathValue = x, 0, 1f);
                    isGameStateChanged = true;
                }
                else if (isDead)
                {
                    Sequence deadEnding = DOTween.Sequence();
                    deadEnding
                        .Append(redMask.DOFade(1, 2f))
                        .Append(DOTween.To(() => globalVolume.weight, x => globalVolume.weight = x, 0, 2f))
                        .Append(GameObject.Find("Dead Line").GetComponent<SpriteRenderer>().DOFade(1, 0f))
                        .AppendInterval(4)
                        .Append(GameObject.Find("Dead Line").GetComponent<SpriteRenderer>().DOFade(0, 0f))
                        .Append(GameObject.Find("Credit").GetComponent<SpriteRenderer>().DOFade(1, 0f))
                        .Append(DOTween.To(() => deathValue, x => deathValue = x, 0, 5f));
                    
                    deadEnding.Play();
                        
                    DOTween.To(() => wakeValue, x => wakeValue = x, 0, 1f);
                    isGameStateChanged = true;
                }
            }
        }
    }
}


