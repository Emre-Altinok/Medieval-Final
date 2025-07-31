using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDialogueController : MonoBehaviour
{
    private DialogueTrigger currentTrigger;
    public DialogueManager dialogueManager;

    private InputAction interactAction;

    private void OnEnable()
    {
        interactAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/e");
        interactAction.performed += ctx => TryStartDialogue();
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    void TryStartDialogue()
    {
        // Diyalog paneli a��ksa tekrar ba�latma!
        if (dialogueManager == null || dialogueManager.dialoguePanel.activeSelf)
            return;

        if (currentTrigger != null)
        {
            dialogueManager.StartDialogue(currentTrigger.dialogue, currentTrigger);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DialogueTrigger trigger))
        {
            currentTrigger = trigger;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out DialogueTrigger trigger) && currentTrigger == trigger)
        {
            currentTrigger = null;
        }
    }
}