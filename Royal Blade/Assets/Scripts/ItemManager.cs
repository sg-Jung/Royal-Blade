using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;
    public static ItemManager Instance { get { return instance; } }

    public List<ItemData> items = new List<ItemData>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ItemData GetRandomItem()
    {
        float totalWeight = 0f;

        foreach (ItemData item in items)
        {
            totalWeight += item.dropWeight;
        }

        float randomValue = Random.Range(0f, totalWeight); 
        float weightSum = 0f;

        foreach (ItemData item in items)
        {
            weightSum += item.dropWeight;

            if (randomValue <= weightSum)
            {
                return item;
            }
        }

        return null; 
    }

    public void AddRandomItem()
    {
        ItemData item = GetRandomItem();

        if (item == null) return;

        Debug.Log("Random Item: " + item.itemName);
        UIManager.Instance.infoText.text = "Random Item: " + item.itemName;

        switch (item.itemName)
        {
            case "heart":
                GameManager.Instance.AddHeart();
                break;
            case "money":
                GameManager.Instance.AddMoney(10);
                break;
        }
    }
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public float dropWeight;
}
