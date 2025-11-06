using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ItemSaveData
{
    public string name;
    public int count;

    public ItemSaveData(string name, int count)
    {
        this.name = name;
        this.count = count;
    }
}

[System.Serializable]
public class ItemSaveWrapper
{
    public List<ItemSaveData> items = new List<ItemSaveData>();
}
public class DataSave : MonoBehaviour
{
    public static DataSave instance;

    //실제 게임에서 사용할 아이템 개수 딕셔너리
    public Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    private string savePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        savePath = Path.Combine(Application.persistentDataPath, "items.json");
        LoadItems(); //시작하면 자동 로드
    }

    //아이템 추가
    public void AddItem(ItemInfo item, int amount = 1)
    {
        if (item == null) return;

        if (itemCounts.ContainsKey(item.itemName))
            itemCounts[item.itemName] += amount;
        else
            itemCounts[item.itemName] = amount;

        InventoryUI.instance?.UpdateUI();
    }

    //아이템 개수 가져오기
    public int GetItemCount(ItemInfo item)
    {
        if (item == null) return 0;
        return itemCounts.ContainsKey(item.itemName) ? itemCounts[item.itemName] : 0;
    }

    public void SaveItems()
    {
        ItemSaveWrapper wrapper = new ItemSaveWrapper();

        foreach (var kvp in itemCounts)
        {
            wrapper.items.Add(new ItemSaveData(kvp.Key, kvp.Value));
        }

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
        Debug.Log("[DataSave] 아이템 저장 완료 : " + savePath);
    }

    public void LoadItems()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("[DataSave] 저장된 아이템 없음 새로 시작");
            return;
        }

        string json = File.ReadAllText(savePath);
        ItemSaveWrapper wrapper = JsonUtility.FromJson<ItemSaveWrapper>(json);

        itemCounts.Clear();
        foreach (var itemData in wrapper.items)
        {
            itemCounts[itemData.name] = itemData.count;
        }

        InventoryUI.instance?.UpdateUI();
        Debug.Log("[DataSave] 아이템 로드 완료");
    }

    //테스트용 초기화
    public void ClearItems()
    {
        itemCounts.Clear();
        if (File.Exists(savePath)) File.Delete(savePath);
        InventoryUI.instance?.UpdateUI();
        Debug.Log("[DataSave] 아이템 초기화 완료");
    }
}
