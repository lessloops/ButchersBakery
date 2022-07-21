using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public GameObject dialogueParent;
    Text messageText;
    Text proceedText;
    Image portraitSr;
    RectTransform backgroundRt;
    int currentSentence;
    string[] sentences;
    bool speaking;
    bool showProceed;
    int visibleLetterIndex;
    float speechDelay = 0.02f;
    float timer;
    float ellipsesDelay = 0.5f;
    float ellipsesTimer;

    private void Start()
    {
        messageText = dialogueParent.transform.Find("DialogueText").GetComponent<Text>();
        proceedText = dialogueParent.transform.Find("ProceedText").GetComponent<Text>();
        portraitSr = dialogueParent.transform.Find("CharacterPortrait").GetComponent<Image>();
        backgroundRt = dialogueParent.transform.Find("Background").GetComponent<RectTransform>();
        dialogueParent.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        if (GameStateController.instance.currentState != "Dialogue")
            return;

        if (Input.GetButtonDown("Interact"))
            Proceed();

        if (speaking)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = speechDelay;
                Speak();
            }
        }
        else if (showProceed)
        {
            if (proceedText.text == "(End)")
                return;

            ellipsesTimer -= Time.deltaTime;

            if (ellipsesTimer <= 0)
            {
                if (proceedText.text == ".")
                    proceedText.text = "..";
                else if (proceedText.text == "..")
                    proceedText.text = "...";
                else
                    proceedText.text = ".";

                ellipsesTimer = ellipsesDelay;
            }
        }
    }

    public void StartDialogue(Sprite portrait, string message)
    {
        portraitSr.sprite = portrait;
        backgroundRt.sizeDelta = new Vector2(Screen.width, backgroundRt.sizeDelta.y);
        sentences = message.Split('|');
        messageText.text = "";
        GameStateController.instance.SetState("Dialogue");
        currentSentence = 0;
        NextSentence();
    }

    void Proceed()
    {
        if (speaking)
            FinishSpeaking();
        else
        {
            currentSentence++;

            if (currentSentence < sentences.Length)
                NextSentence();
            else
                EndDialogue();
        }
    }

    void Speak()
    {
        if (visibleLetterIndex < sentences[currentSentence].Length)
        {
            messageText.text = sentences[currentSentence].Substring(0, visibleLetterIndex);
            messageText.text += "<color=#00000000>" + sentences[currentSentence].Substring(visibleLetterIndex) + "</color>";
            visibleLetterIndex++;
        }
        else
        {
            FinishSpeaking();
        }
    }

    void FinishSpeaking()
    {
        speaking = false;

        if (currentSentence == sentences.Length-1)
            proceedText.text = "(End)";
        else
            proceedText.text = ".";

        messageText.text = sentences[currentSentence];
        showProceed = true;
    }

    void NextSentence()
    {
        proceedText.text = "";
        visibleLetterIndex = 0;
        timer = speechDelay;
        speaking = true;
    }

    void EndDialogue()
    {
        proceedText.text = "";
        StartCoroutine(DialogueCooldown());
    }
    
    IEnumerator DialogueCooldown()
    {
        yield return 0;
        GameStateController.instance.SetState("Live");
    }
}
