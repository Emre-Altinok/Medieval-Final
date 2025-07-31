using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    [Header("Typewriter Ayarlarý")]
    public float typewriterSpeed = 0.04f;

    [Header("Input System")]
    public InputActionReference nextLineAction;

    private DialogueLine[] currentLines;
    private int currentIndex = 0;
    private Coroutine typewriterCoroutine;
    private bool isTypewriting = false;
    private bool skipTypewriter = false;

    public UnityEvent OnDialogueEndEvent;
    private DialogueTrigger currentTrigger;
    private bool dialogueActive = false;

    void Awake()
    {
        if (OnDialogueEndEvent == null)
            OnDialogueEndEvent = new UnityEvent();
    }

    public void StartDialogue(DialogueSO dialogue, DialogueTrigger trigger)
    {
        if (dialogueActive) return;
        dialogueActive = true;

        currentTrigger = trigger;
        dialoguePanel.SetActive(true);
        currentLines = dialogue.lines;
        currentIndex = 0;
        ShowCurrentLine();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (nextLineAction != null)
        {
            nextLineAction.action.performed += OnNextLineInput;
            nextLineAction.action.Enable();
        }
    }

    void ShowCurrentLine()
    {
        if (currentIndex >= currentLines.Length)
        {
            EndDialogue();
            return;
        }

        var line = currentLines[currentIndex];
        if (speakerNameText != null)
            speakerNameText.text = line.speakerName;
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        typewriterCoroutine = StartCoroutine(TypewriterEffect(line.sentence));
    }

    IEnumerator TypewriterEffect(string sentence)
    {
        isTypewriting = true;
        skipTypewriter = false;
        dialogueText.text = "";

        for (int i = 0; i < sentence.Length; i++)
        {
            if (skipTypewriter)
            {
                dialogueText.text = sentence;
                break;
            }
            dialogueText.text += sentence[i];
            yield return new WaitForSecondsRealtime(typewriterSpeed);
        }

        isTypewriting = false;
    }

    private void OnNextLineInput(InputAction.CallbackContext context)
    {
        if (!dialoguePanel.activeSelf) return;

        if (isTypewriting)
        {
            skipTypewriter = true;
        }
        else
        {
            currentIndex++;
            ShowCurrentLine();
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        OnDialogueEndEvent.Invoke();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (nextLineAction != null)
        {
            nextLineAction.action.performed -= OnNextLineInput;
            nextLineAction.action.Disable();
        }

        if (currentTrigger != null)
        {
            currentTrigger.OnDialogueEnd();
            currentTrigger = null;
        }

        dialogueActive = false;
    }

    public void OnDialogueEnd()
    {
        //gerekirse event eklenebilir
    }
}