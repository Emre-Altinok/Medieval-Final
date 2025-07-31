using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable]
public class QuestTypeList
{
    public List<QuestTypeData> types;
}

[Serializable]
public class QuestTypeData
{
    public string questType;
    public List<string> targets;
    public int minCount;
    public int maxCount;
    public int rewardBase;
    public int rewardPerTarget;
    public string titleFormat;
    public string descFormat;
}

public static class QuestGenerator
{
    private static QuestTypeList questTypeList;

    static QuestGenerator()
    {
        LoadQuestTypes();
    }

    public static void LoadQuestTypes()
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>("quest_types");
        if (jsonAsset == null)
        {
            // Dosya Assets/Resources/quest_types.json olmalý!
            string path = Path.Combine(Application.dataPath, "##Assets/Scripts/quest_types.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                questTypeList = JsonUtility.FromJson<QuestTypeList>(json);
            }
        }
        else
        {
            questTypeList = JsonUtility.FromJson<QuestTypeList>(jsonAsset.text);
        }
    }

    public static QuestManager.QuestData GenerateRandomQuest()
    {
        if (questTypeList == null || questTypeList.types == null || questTypeList.types.Count == 0)
        {
            Debug.LogError("QuestGenerator: quest_types.json yüklenemedi veya boþ!");
            return new QuestManager.QuestData
            {
                questID = $"unknown_{Guid.NewGuid()}",
                title = "Bilinmeyen Görev",
                description = "Görev tipi bulunamadý.",
                rewardGold = 0,
                isPlaceholder = true
            };
        }

        var typeData = questTypeList.types[UnityEngine.Random.Range(0, questTypeList.types.Count)];
        string target = typeData.targets[UnityEngine.Random.Range(0, typeData.targets.Count)];
        int count = UnityEngine.Random.Range(typeData.minCount, typeData.maxCount + 1);
        int reward = typeData.rewardBase + count * typeData.rewardPerTarget;

        string title = typeData.titleFormat.Replace("{count}", count.ToString()).Replace("{target}", target);
        string desc = typeData.descFormat.Replace("{count}", count.ToString()).Replace("{target}", target.ToLower());

        return new QuestManager.QuestData
        {
            questID = $"{typeData.questType}_{Guid.NewGuid()}",
            title = title,
            description = desc,
            rewardGold = reward,
            isPlaceholder = false,
            targetCount = count,
            currentCount = 0,
            targetType = target,
            questType = typeData.questType
        };
    }
}