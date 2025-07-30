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
        if (currentTrigger != null)
        {
            dialogueManager.StartDialogue(currentTrigger.dialogue);
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

