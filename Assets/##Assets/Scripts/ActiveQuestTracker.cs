using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActiveQuestTracker : MonoBehaviour
{
    public static ActiveQuestTracker instance;

    public List<QuestManager.QuestData> activeQuests = new List<QuestManager.QuestData>();

    private SpawnManager spawnManager;

    [Header("Aktif G�rev UI")]
    public Transform activeQuestListContainer; // ScrollView > G�revListesi objesi
    public GameObject activeQuestUIPrefab;     // 2 TMP_Text + 1 Button i�eren prefab

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        UpdateQuestUI();
    }

    public void AddActiveQuest(QuestManager.QuestData quest)
    {
        if (activeQuests.Contains(quest)) return;

        quest.currentCount = 0;
        activeQuests.Add(quest);

        if (spawnManager != null && !quest.isPlaceholder)
        {
            spawnManager.SpawnQuestTargets(quest.questType, quest.targetType, quest.targetCount);
        }
        UpdateQuestUI();
    }

    public void RegisterKill(string targetType)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.questType == "kill" && quest.targetType == targetType && !quest.isCompleted)
            {
                quest.currentCount++;
                Debug.Log($"{targetType} �ld�r�ld�. {quest.currentCount}/{quest.targetCount}");
            }
        }
        UpdateQuestUI();
    }

    public void RegisterGather(string materialType)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.questType == "gather" && quest.targetType == materialType && !quest.isCompleted)
            {
                quest.currentCount++;
                Debug.Log($"{materialType} topland�. {quest.currentCount}/{quest.targetCount}");
            }
        }
        UpdateQuestUI();
    }

    public void CompleteQuest(QuestManager.QuestData quest)
    {
        Debug.Log($"G�rev tamamland�: {quest.title} - �d�l: {quest.rewardGold} alt�n");
        activeQuests.Remove(quest);
        // Burada �d�l verme, envanter g�ncelleme vs. ekleyebilirsin
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        if (activeQuestListContainer == null || activeQuestUIPrefab == null)
            return;

        foreach (Transform child in activeQuestListContainer)
            Destroy(child.gameObject);

        foreach (var quest in activeQuests)
        {
            GameObject questUIObj = Instantiate(activeQuestUIPrefab, activeQuestListContainer, false);
            TMP_Text[] texts = questUIObj.GetComponentsInChildren<TMP_Text>();
            Button completeButton = questUIObj.GetComponentInChildren<Button>();

            if (texts.Length >= 2)
            {
                texts[0].text = quest.title;
                if (quest.questType == "kill")
                    texts[1].text = $"{quest.targetType} �ld�r: {quest.currentCount}/{quest.targetCount}";
                else if (quest.questType == "gather")
                    texts[1].text = $"{quest.targetType} topla: {quest.currentCount}/{quest.targetCount}";
                else
                    texts[1].text = "";
            }

            if (completeButton != null)
            {
                completeButton.onClick.RemoveAllListeners();
                completeButton.gameObject.SetActive(quest.isCompleted);
                if (quest.isCompleted)
                {
                    completeButton.GetComponentInChildren<TMP_Text>().text = "Tamamla";
                    completeButton.onClick.AddListener(() => CompleteQuest(quest));
                }
            }
        }
    }
}