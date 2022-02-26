using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class tutorial_helper_method : MonoBehaviour
{
    public static event Action OnTutorialEnded = delegate { };
    public TutorialStates currentState;
    public GameObject handWithLetter;
    public GameObject handWithShotgun;
    public GameObject handWithPhone;
    public GameObject popup_phone_tutorial;
   public GameObject popup_neighborhood_message;
    public GameObject press_left_click_to_fire_msg;
   
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
   
    }

    // Update is called once per frame
    void Update()
    {
        if(!_gameHasStarted || _hasTutorialEnded) return;
        
        if (Input.anyKeyDown && !_onIntroTutorial) {
            StartCoroutine(tutorial());
        }
        
    }


    public void SetGameHasStarted()
    {
        _gameHasStarted = true;
        _onIntroTutorial = true;
        AudioManager.Instance.Play("Ambient");
        StartCoroutine(tutorial());
    }
    
    

    IEnumerator tutorial() {

        if (currentState == TutorialStates.tutorialNotStarted)
        {
            currentState = TutorialStates.IntroductionLetter;
            yield return new WaitForSeconds(0.8f);
            AudioManager.Instance.Play("timetogetthisjobdone");
            yield return new WaitForSeconds(2f);
            AudioManager.Instance.Play("swish");
            popup_neighborhood_message.SetActive(true);
            currentState = TutorialStates.IntroductionLetterDone;
            yield return new WaitForSeconds(0.5f);
            _onIntroTutorial = false;
        }
        else if (currentState == TutorialStates.IntroductionLetterDone)
        {
            _onIntroTutorial = true;
            handWithLetter.SetActive(false);
            popup_neighborhood_message.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            handWithPhone.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.Play("thismightcomeinhandy");
            handWithPhone.GetComponent<hand_with_phone>().pressPhoneButtonAnimation();
            yield return new WaitForSeconds(2f);
            popup_phone_tutorial.SetActive(true);
            AudioManager.Instance.Play("swish");
            currentState = TutorialStates.showPhoneTutorialDone;
            yield return new WaitForSeconds(0.5f);
            _onIntroTutorial = false;

        } else if (currentState == TutorialStates.showPhoneTutorialDone) {
            _onIntroTutorial = true;
            handWithPhone.SetActive(false);
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
            yield return new WaitForSeconds(2f);
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
