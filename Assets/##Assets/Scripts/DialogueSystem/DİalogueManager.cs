using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    private DialogueLine[] currentLines;
    private int currentIndex = 0;
    private float autoDelay = 3f;

    public void StartDialogue(DialogueSO dialogue)
    {
        dialoguePanel.SetActive(true);
        currentLines = dialogue.lines;
        currentIndex = 0;
        StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        while (currentIndex < currentLines.Length)
        {
            var line = currentLines[currentIndex];
            dialogueText.text = $"{line.speakerName}: {line.sentence}";
            currentIndex++;
            yield return new WaitForSeconds(autoDelay);
        }

        EndDialogue();
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}

