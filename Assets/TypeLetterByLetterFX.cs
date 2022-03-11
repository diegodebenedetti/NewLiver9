using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeLetterByLetterFX : MonoBehaviour
{
    public float letterPause = 0.2f;


     string message;
    public TextMeshProUGUI textComp;
    char[] message_chararray;

    // Use this for initialization
    void Start()
    {
       
        
        
      
       

    }
    public void startTypingMessage() {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        Debug.Log("type text");
        message = textComp.text;
        message_chararray = message.ToCharArray();
        textComp.text = "" ;

        for (int i = 0; i < message_chararray.Length; i++)
        {
            textComp.text += message_chararray[i];

            int num = Random.Range(1, 3);
            AudioManager.Instance.PlayOneShot("typingkey_" + num,true,0.3f,0.7f);
            yield return 0;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }

       
    }
}
