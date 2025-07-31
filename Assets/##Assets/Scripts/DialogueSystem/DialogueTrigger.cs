using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSO dialogue;

    // Diyalog bitince çaðrýlacak
    public void OnDialogueEnd()
    {
        // Bu objede OnDialogueEnd fonksiyonu olan bir script varsa çaðýr
        SendMessage("OnDialogueEnd", SendMessageOptions.DontRequireReceiver);
    }
}

