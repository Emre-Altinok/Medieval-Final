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

        public int targetCount;     // Gereken �ld�rme/toplama say�s�
        public int currentCount;    // Oyuncunun ilerlemesi
        public string targetType;   // Hedef d��man veya malzeme tipi
        public string questType;    // "kill" veya "gather"
        public bool isCompleted => currentCount >= targetCount;
    }

    public List<QuestData> quests = new List<QuestData>();

    [Header("UI")]
    public Transform questListContainer;       // ScrollView i�indeki Content objesi
    public GameObject questButtonPrefab;       // Buton prefab�
    public TMP_Text titleText;                 // Sa� panel ba�l�k
    public TMP_Text descriptionText;           // Sa� panel a��klama
    public TMP_Text rewardText;                // Sa� panel �d�l
    public Button acceptButton;                // Kabul et tu�u

    private QuestData selectedQuest;

    void Start()
    {
        CreateQuests();
        PopulateQuestList();
    }

    void CreateQuests()
    {
        // �rnek: 3 random g�rev olu�tur
        for (int i = 0; i < 3; i++)
        {
            quests.Add(GenerateRandomQuest(i));
        }

        // Placeholder g�revler
        for (int i = 0; i < 2; i++)
        {
            quests.Add(new QuestData
            {
                questID = $"placeholder_{i}",
                title = "G�rev Hen�z Eklenmedi",
                description = "Bu g�rev hen�z haz�rlanmad�.",
                rewardGold = 0,
                isPlaceholder = true
            });
        }
    }

    QuestData GenerateRandomQuest(int index)
    {
        // G�rev t�rleri
        string[] questTypes = { "kill", "gather" };
        string[] enemyTypes = { "Kurt", "Haydut" };
        string[] gatherTypes = { "Odun", "Ta�" };

        // Rastgele g�rev t�r� se�
        string selectedQuestType = questTypes[0]; //debug ama�l� av 
        //string selectedQuestType = questTypes[Random.Range(0, questTypes.Length)];

        switch (selectedQuestType)
        {
            case "kill":
                {
                    string target = enemyTypes[Random.Range(0, enemyTypes.Length)];
                    int count = Random.Range(1, 5); // 1-4 aras�
                    return new QuestData
                    {
                        questID = $"kill_{index}",
                        title = $"{count} {target} �ld�r",
                        description = $"�evredeki {target.ToLower()}lardan {count} tanesini yok et.",
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
                    int count = Random.Range(5, 11); // 5-10 aras�
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
            // Yeni g�rev tipleri i�in buraya ekleyebilirsiniz:
            // case "deliver":
            //     ...
            default:
                // Bilinmeyen tipte placeholder g�rev d�nd�r
                return new QuestData
                {
                    questID = $"unknown_{index}",
                    title = "Bilinmeyen G�rev",
                    description = "Bu g�rev tipi hen�z eklenmedi.",
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
        rewardText.text = $"�d�l: {quest.rewardGold} alt�n";

        acceptButton.interactable = !quest.isPlaceholder;

        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(() => AcceptQuest(quest));
    }

    void AcceptQuest(QuestData quest)
    {
        Debug.Log($"G�rev kabul edildi: {quest.title}");
        ActiveQuestTracker.instance.SetActiveQuest(quest);
    }
}