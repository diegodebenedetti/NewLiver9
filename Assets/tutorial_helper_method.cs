using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_helper_method : MonoBehaviour
{

    public TutorialStates currentState;
    public GameObject handWithLetter;
    public GameObject handWithShotgun;
    public GameObject handWithPhone;
    public GameObject popup_phone_tutorial;
   public GameObject popup_neighborhood_message;
    public GameObject press_left_click_to_fire_msg;
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


    
    // Start is called before the first frame update
    void Start()
    {
        currentState = TutorialStates.tutorialNotStarted;
   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) {
            StartCoroutine(tutorial());
        }

        


    }

   

    IEnumerator tutorial() {

        if (currentState == TutorialStates.tutorialNotStarted)
        {
            currentState = TutorialStates.IntroductionLetter;
            yield return new WaitForSeconds(2f);
            AudioManager.Instance.Play("timetogetthisjobdone");
            yield return new WaitForSeconds(2f);
            AudioManager.Instance.Play("swish");
            popup_neighborhood_message.SetActive(true);
            currentState = TutorialStates.IntroductionLetterDone;
            yield return null;
        }
        else if (currentState == TutorialStates.IntroductionLetterDone)
        {
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

        } else if (currentState == TutorialStates.showPhoneTutorialDone) {
            handWithPhone.SetActive(false);
            popup_phone_tutorial.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            handWithShotgun.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.Play("imightneedthis");
            yield return new WaitForSeconds(0.5f);
            press_left_click_to_fire_msg.SetActive(true);
            currentState = TutorialStates.shotgunTutorialWaitForFire;
        } else if (currentState==TutorialStates.shotgunTutorialWaitForFire && Input.GetMouseButtonDown(0)) {
            yield return new WaitForSeconds(2f);
            press_left_click_to_fire_msg.SetActive(false);
            handWithPhone.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            handWithShotgun.SetActive(false);
            AudioManager.Instance.Play("timetogo");

        }
    }
}
