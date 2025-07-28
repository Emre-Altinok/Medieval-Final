using UnityEngine;

public class ActiveQuestTracker : MonoBehaviour
{
    public static ActiveQuestTracker instance;

    public QuestManager.QuestData activeQuest;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }


    public void SetActiveQuest(QuestManager.QuestData quest)
    {
        activeQuest = quest;
        quest.currentCount = 0;
        Debug.Log($"Aktif g�rev: {quest.title}");
    }

   

    public void RegisterKill(string targetType)
    {
        if (activeQuest == null || activeQuest.isCompleted) return;

        if (activeQuest.title.Contains("Kurt"))
        {
            activeQuest.currentCount++;
            Debug.Log($"Kurt �ld�r�ld�. {activeQuest.currentCount}/{activeQuest.targetCount}");

            if (activeQuest.isCompleted)
            {
                Debug.Log("G�rev tamamland�!");
                // Buraya �d�l verimi ekleyebilirsin
            }
        }

    }

}
