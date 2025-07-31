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
        public int targetCount;
        public int currentCount;
        public string targetType;
        public string questType;
        public bool isCompleted => currentCount >= targetCount;
    }

    public List<QuestData> quests = new List<QuestData>();

    [Header("UI")]
    public Transform questListContainer;
    public GameObject questButtonPrefab;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text rewardText;
    public Button acceptButton;

    private QuestData selectedQuest;

    void Start()
    {
        CreateQuests();
        PopulateQuestList();
        acceptButton.interactable = false;
        acceptButton.GetComponentInChildren<TMP_Text>().text = "Kabul Et";
        acceptButton.onClick.RemoveAllListeners();
    }

    void CreateQuests()
    {
        for (int i = 0; i < 5; i++)
        {
            quests.Add(QuestGenerator.GenerateRandomQuest());
        }
        //for (int i = 0; i < 2; i++)
        //{
        //    quests.Add(new QuestData
        //    {
        //        questID = $"placeholder_{i}",
        //        title = "Görev Henüz Eklenmedi",
        //        description = "Bu görev henüz hazýrlanmadý.",
        //        rewardGold = 0,
        //        isPlaceholder = true
        //    });
        //}
    }

    void PopulateQuestList()
    {
        foreach (Transform child in questListContainer)
            Destroy(child.gameObject);

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

        UpdateAcceptButtonUI();
    }

    void UpdateAcceptButtonUI()
    {
        acceptButton.onClick.RemoveAllListeners();

        bool isAccepted = ActiveQuestTracker.instance != null && ActiveQuestTracker.instance.activeQuests.Contains(selectedQuest);

        if (selectedQuest == null || selectedQuest.isPlaceholder)
        {
            acceptButton.interactable = false;
            acceptButton.GetComponentInChildren<TMP_Text>().text = "Kabul Et";
            return;
        }

        if (!isAccepted)
        {
            acceptButton.interactable = true;
            acceptButton.GetComponentInChildren<TMP_Text>().text = "Kabul Et";
            acceptButton.onClick.AddListener(() => AcceptQuest(selectedQuest));
        }
        else if (selectedQuest.isCompleted)
        {
            acceptButton.interactable = true;
            acceptButton.GetComponentInChildren<TMP_Text>().text = "Tamamla";
            acceptButton.onClick.AddListener(() => CompleteQuest(selectedQuest));
        }
        else
        {
            acceptButton.interactable = false;
            acceptButton.GetComponentInChildren<TMP_Text>().text = "Devam Ediliyor";
        }
    }

    void AcceptQuest(QuestData quest)
    {
        if (quest == null || quest.isPlaceholder) return;
        Debug.Log($"Görev kabul edildi: {quest.title}");
        ActiveQuestTracker.instance.AddActiveQuest(quest);
        UpdateAcceptButtonUI();
    }

    void CompleteQuest(QuestData quest)
    {
        Debug.Log($"Görev tamamlandý: {quest.title} - Ödül: {quest.rewardGold} altýn");
        if (ActiveQuestTracker.instance != null)
        {
            ActiveQuestTracker.instance.CompleteQuest(quest);
        }

        quests.Remove(quest);
        quests.Add(QuestGenerator.GenerateRandomQuest());

        PopulateQuestList();

        selectedQuest = null;
        titleText.text = "";
        descriptionText.text = "";
        rewardText.text = "";
        UpdateAcceptButtonUI();
    }
}