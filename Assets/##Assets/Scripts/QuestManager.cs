using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestData
    {
        public string questID;
        public string title;
        public string description;
        public int rewardGold;
        public bool isPlaceholder;

        public int targetCount;     // Gereken öldürme/toplama sayýsý
        public int currentCount;    // Oyuncunun ilerlemesi
        public string targetType;   // Hedef düþman veya malzeme tipi
        public string questType;    // "kill" veya "gather"
        public bool isCompleted => currentCount >= targetCount;
    }

    public List<QuestData> quests = new List<QuestData>();

    [Header("UI")]
    public Transform questListContainer;       // ScrollView içindeki Content objesi
    public GameObject questButtonPrefab;       // Buton prefabý
    public TMP_Text titleText;                 // Sað panel baþlýk
    public TMP_Text descriptionText;           // Sað panel açýklama
    public TMP_Text rewardText;                // Sað panel ödül
    public Button acceptButton;                // Kabul et tuþu

    private QuestData selectedQuest;

    void Start()
    {
        CreateQuests();
        PopulateQuestList();
    }

    void CreateQuests()
    {
        // Örnek: 3 random görev oluþtur
        for (int i = 0; i < 3; i++)
        {
            quests.Add(GenerateRandomQuest(i));
        }

        // Placeholder görevler
        for (int i = 0; i < 2; i++)
        {
            quests.Add(new QuestData
            {
                questID = $"placeholder_{i}",
                title = "Görev Henüz Eklenmedi",
                description = "Bu görev henüz hazýrlanmadý.",
                rewardGold = 0,
                isPlaceholder = true
            });
        }
    }

    QuestData GenerateRandomQuest(int index)
    {
        // Görev türleri
        string[] questTypes = { "kill", "gather" };
        string[] enemyTypes = { "Kurt", "Haydut" };
        string[] gatherTypes = { "Odun", "Taþ" };

        // Rastgele görev türü seç
        string selectedQuestType = questTypes[0]; //debug amaçlý av 
        //string selectedQuestType = questTypes[Random.Range(0, questTypes.Length)];

        switch (selectedQuestType)
        {
            case "kill":
                {
                    string target = enemyTypes[Random.Range(0, enemyTypes.Length)];
                    int count = Random.Range(1, 5); // 1-4 arasý
                    return new QuestData
                    {
                        questID = $"kill_{index}",
                        title = $"{count} {target} öldür",
                        description = $"Çevredeki {target.ToLower()}lardan {count} tanesini yok et.",
                        rewardGold = 50 + count * 30,
                        isPlaceholder = false,
                        targetCount = count,
                        currentCount = 0,
                        targetType = target,
                        questType = "kill"
                    };
                }
            case "gather":
                {
                    string material = gatherTypes[Random.Range(0, gatherTypes.Length)];
                    int count = Random.Range(5, 11); // 5-10 arasý
                    return new QuestData
                    {
                        questID = $"gather_{index}",
                        title = $"{count} {material} topla",
                        description = $"{count} adet {material.ToLower()} bul ve topla.",
                        rewardGold = 40 + count * 20,
                        isPlaceholder = false,
                        targetCount = count,
                        currentCount = 0,
                        targetType = material,
                        questType = "gather"
                    };
                }
            // Yeni görev tipleri için buraya ekleyebilirsiniz:
            // case "deliver":
            //     ...
            default:
                // Bilinmeyen tipte placeholder görev döndür
                return new QuestData
                {
                    questID = $"unknown_{index}",
                    title = "Bilinmeyen Görev",
                    description = "Bu görev tipi henüz eklenmedi.",
                    rewardGold = 0,
                    isPlaceholder = true
                };
        }
    }

    void PopulateQuestList()
    {
        foreach (var quest in quests)
        {
            GameObject buttonObj = Instantiate(questButtonPrefab, questListContainer);
            buttonObj.GetComponentInChildren<TMP_Text>().text = quest.title;

            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectQuest(quest);
            });
        }
    }

    void SelectQuest(QuestData quest)
    {
        selectedQuest = quest;

        titleText.text = quest.title;
        descriptionText.text = quest.description;
        rewardText.text = $"Ödül: {quest.rewardGold} altýn";

        acceptButton.interactable = !quest.isPlaceholder;

        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(() => AcceptQuest(quest));
    }

    void AcceptQuest(QuestData quest)
    {
        Debug.Log($"Görev kabul edildi: {quest.title}");
        ActiveQuestTracker.instance.SetActiveQuest(quest);
    }
}