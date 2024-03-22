using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager Instance => instance;

    private Queue<string> sentences;

    [SerializeField] Image dialoguePanel;
    [SerializeField] GameObject dialogueObject;
    //[SerializeField] float dialogueDelay = 3f;

    private TMP_Text dialogueText;
    private string dialogueName;

    private bool isRunning;
    public bool IsRunning => isRunning;

    [SerializeField] Animator animator;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        dialogueObject.TryGetComponent(out dialogueText);
    }

    private void Start()
    {
        dialoguePanel.enabled = false;

        if (animator != null)
        {
            animator.SetBool("IsRunning", false);
        }
        isRunning = false;

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {

        dialoguePanel.enabled = false;
        //dialoguePanel.SetActive(true);

        isRunning = true;

        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
        }

        dialogueName = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(PrintSentence(sentence));
    }

    void EndDialogue()
    {
        isRunning = false;
        //dialoguePanel.SetActive(false);

        if (animator != null)
        {
            animator.SetBool("IsRunning", false);
        }

        dialoguePanel.enabled = false;
    }

    IEnumerator PrintSentence(string sentence)
    {
        if (dialogueName != "")
        {
            dialogueText.text = dialogueName + ": ";
        }
        else
        {
            dialogueText.text = "";
        }

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;

            ///SoundManager.PlaySound(SoundManager.Sound.Dialog);

            //if (isDialogueBox)
            //{
            //    yield return new WaitForSeconds(0.025f);
            //}
            //else
            //{
            //    yield return new WaitForSeconds(timeTaken);
            //}

            yield return new WaitForSeconds(0.025f);
        }

        if (animator != null)
        {
            yield return new WaitForSeconds(4f);
            dialogueText.text = "";
            dialogueText.name = "";
            animator.SetBool("IsRunning", false);
            yield return new WaitForSeconds(1.5f);
            DisplayNextSentence();
            animator.SetBool("IsRunning", true);
        }
    }

    public void ResetDialogue()
    {
        sentences.Clear();
        EndDialogue();
    }
}