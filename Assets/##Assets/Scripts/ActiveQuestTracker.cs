using UnityEngine;

public class ActiveQuestTracker : MonoBehaviour
{
    public static ActiveQuestTracker instance;

    public QuestManager.QuestData activeQuest;

    private SpawnManager spawnManager;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
    }

    public void SetActiveQuest(QuestManager.QuestData quest)
    {
        activeQuest = quest;
        quest.currentCount = 0;
        Debug.Log($"Aktif g�rev: {quest.title}");

        // G�rev hedeflerini spawnla
        if (spawnManager != null && !quest.isPlaceholder)
        {
            spawnManager.SpawnQuestTargets(quest.questType, quest.targetType, quest.targetCount);
        }
    }



    public void RegisterKill(string targetType)
    {
        if (activeQuest == null || activeQuest.isCompleted) return;
        if (activeQuest.questType == "kill" && activeQuest.targetType == targetType)
        {
            activeQuest.currentCount++;
            Debug.Log($"{targetType} �ld�r�ld�. {activeQuest.currentCount}/{activeQuest.targetCount}");

            if (activeQuest.isCompleted)
            {
                Debug.Log("G�rev tamamland�!");
                // �d�l verimi ekleyebilirsin
            }
        }
    }

    public void RegisterGather(string materialType)
    {
        if (activeQuest == null || activeQuest.isCompleted) return;
        if (activeQuest.questType == "gather" && activeQuest.targetType == materialType)
        {
            activeQuest.currentCount++;
            Debug.Log($"{materialType} topland�. {activeQuest.currentCount}/{activeQuest.targetCount}");

            if (activeQuest.isCompleted)
            {
                Debug.Log("G�rev tamamland�!");
                // �d�l verimi ekleyebilirsin
            }
        }
    }

}
