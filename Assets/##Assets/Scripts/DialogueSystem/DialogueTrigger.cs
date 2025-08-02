using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSO dialogue;

    // Diyalog bitince �a�r�lacak
    public void OnDialogueEnd()
    {
        // Bu objede OnDialogueEnd fonksiyonu olan bir script varsa �a��r
        SendMessage("OnDialogueEndEvent", SendMessageOptions.DontRequireReceiver);
    }
}

