using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum ItemType
{
    Guitar,     //기타(광물)
    Consum,      //소비아이템(구상, 산소캡슐 등)
    Equipment   //장비아이템(신발, 헬멧, 드릴)
}
[System.Serializable]
public class ItemInfo
{
    [Header("기본 설정")]
    public ItemType itemType;
    public string itemName;
    public int sellPrice;
    public int buyPrice;
    public Sprite itemSprite;

    [Header("소비")]
    public int healAmount;
    public int O2UpAmount;
    public int damageAmount;

    [Header("장비")]
    public int damageUpAmount;
    public int maxO2UpAmount;
    public int flyForceUpAmount;

    public Sprite icon => itemSprite;


    public void Use()
    {
            
    }

    public int MaxStack
    {
        get
        {
            switch (itemType)
            {
                case ItemType.Guitar: return 5;
                case ItemType.Consum: return 1;
                case ItemType.Equipment: return 1;
                default: return 1;
            }
        }
    }
    public ItemInfo(string name, ItemType type, int sellPrice, int buyPrice, Sprite sprite)
    {
        itemName = name;
        itemType = type;
        this.sellPrice = sellPrice;
        this.buyPrice = buyPrice;
        itemSprite = sprite;
    }

    //오버라이드하여 내용이 같은 아이템이면 같은키로 인식하기
    public override bool Equals(object obj)
    {
        return obj is ItemInfo other && this.itemName == other.itemName;
    }
    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }
}
public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager instance;

    public List<ItemInfo> allItems = new List<ItemInfo>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
}
