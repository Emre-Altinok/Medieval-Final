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

        public int targetCount;     // Gereken �ld�rme say�s�
        public int currentCount;    // Oyuncunun ilerlemesi
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
        // Ger�ek g�rev
        quests.Add(new QuestData
        {
            questID = "001",
            title = "Orman Kurtlar�n� Yok Et",
            description = "Yak�ndaki ormanda 3 kurdu yok et.",
            rewardGold = 100,
            isPlaceholder = false
        });

        // Placeholder g�revler
        for (int i = 0; i < 5; i++)
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
