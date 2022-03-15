using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class tutorial_helper_method : MonoBehaviour
{
    public static event Action OnTutorialEnded = delegate { };
    public TutorialStates currentState;
    public bool DoTutorial;
    public GameObject handWithLetter;
    public GameObject handWithShotgun;
    public GameObject handWithPhone;
    public GameObject popup_phone_tutorial;
   public GameObject popup_neighborhood_message;
    public TypeLetterByLetterFX popup_neighborhood_message_type_FX;
    public TypeLetterByLetterFX popup_phone_tutorial_FX;
    public GameObject press_left_click_to_fire_msg;
    public GameObject panel_fadetoblack_homescreen;
    private CanvasGroup panel_canvasgroup_fadetoblack_homescreen;
    public GameObject image_homescreen;
    private bool _gameHasStarted;
    private bool _hasTutorialEnded;
    private bool _onIntroTutorial;

    public enum TutorialStates
    {
        tutorialNotStarted,
        IntroductionLetter,
        IntroductionLetterDone,
        showPhoneTutorial,
        showPhoneTutorialDone,
        shotgunTutorial,
        shotgunTutorialWaitForFire,
        shotgunTutorialDone
    }


    
    void Start()
    {
        currentState = TutorialStates.tutorialNotStarted;
        panel_canvasgroup_fadetoblack_homescreen = panel_fadetoblack_homescreen.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_gameHasStarted || _hasTutorialEnded) return;
        
        if(DoTutorial)
        { 
            if (Input.anyKeyDown && !_onIntroTutorial) 
                StartCoroutine(tutorial());
        } 
        else
            OnTutorialEnded.Invoke(); 
        
    }


    public void SetGameHasStarted()
    {
        if (_gameHasStarted)
        {
            return;
        }

        _gameHasStarted = true;
        _onIntroTutorial = true;
        AudioManager.Instance.Play("Ambient");
        AudioManager.Instance.Play("startnewgamebell");

        StartCoroutine(FadeToTutorial());

        StartCoroutine(tutorial());
    }

    IEnumerator FadeToTutorial() {
        //fade to black
        LeanTween.value(0f, 1f, 0.2f).setOnUpdate((float value) =>
        { 
            panel_canvasgroup_fadetoblack_homescreen.alpha = value;
        });

        yield return new WaitForSeconds(1f);

        LeanTween.value(1f, 0f, 0.3f).setOnUpdate((float value) =>
        { 
            panel_canvasgroup_fadetoblack_homescreen.alpha = value;
        });

    }
    

    IEnumerator tutorial() {

        if (currentState == TutorialStates.tutorialNotStarted)
        {
            yield return new WaitForSeconds(1f);
            image_homescreen.SetActive(false);
            currentState = TutorialStates.IntroductionLetter;
            yield return new WaitForSeconds(0.8f);
            AudioManager.Instance.Play("timetogetthisjobdone");
            yield return new WaitForSeconds(2f);
            AudioManager.Instance.Play("swish");
            popup_neighborhood_message.SetActive(true);
            popup_neighborhood_message_type_FX.startTypingMessage();

            currentState = TutorialStates.IntroductionLetterDone;
            yield return new WaitForSeconds(0.5f);
            _onIntroTutorial = false;
        }
        else if (currentState == TutorialStates.IntroductionLetterDone)
        {
            _onIntroTutorial = true;
            handWithLetter.SetActive(false);
            popup_neighborhood_message.SetActive(false);
            AudioManager.Instance.Play("swish");
            yield return new WaitForSeconds(0.2f);
            handWithPhone.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.Play("thismightcomeinhandy");
            handWithPhone.GetComponent<hand_with_phone>().pressPhoneButtonAnimation();
            yield return new WaitForSeconds(2f);
            popup_phone_tutorial.SetActive(true);
            popup_phone_tutorial_FX.startTypingMessage();
            AudioManager.Instance.Play("swish");
            currentState = TutorialStates.showPhoneTutorialDone;
            yield return new WaitForSeconds(0.5f);
            _onIntroTutorial = false;

        } else if (currentState == TutorialStates.showPhoneTutorialDone) {
            _onIntroTutorial = true;
            handWithPhone.SetActive(false);
            AudioManager.Instance.Play("swish");
            popup_phone_tutorial.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            handWithShotgun.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.Play("imightneedthis");
            yield return new WaitForSeconds(0.5f);
            press_left_click_to_fire_msg.SetActive(true);
            currentState = TutorialStates.shotgunTutorialWaitForFire;
            yield return new WaitForSeconds(0.5f);
            _onIntroTutorial = false;
        } else if (currentState==TutorialStates.shotgunTutorialWaitForFire && Input.GetMouseButtonDown(0)) {
            _onIntroTutorial = true;
            yield return new WaitForSeconds(.7f);
            press_left_click_to_fire_msg.SetActive(false);
            handWithShotgun.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            handWithPhone.SetActive(true);
            AudioManager.Instance.Play("timetogo");
            _hasTutorialEnded = true;
            yield return new WaitForSeconds(0.5f);
            _onIntroTutorial = false;
            OnTutorialEnded.Invoke();

        }
    }
}
