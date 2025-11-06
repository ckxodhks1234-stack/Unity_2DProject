using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Guitar,     //기타(광물)
    Consum,      //소비아이템(구상, 산소캡슐 등)
    Equipment   //장비아이템(신발, 헬멧, 드릴)
}

public enum EquipType
{
    None,
    Helmet,
    Shoes,
    Drill
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
    public EquipType equipType;

    [Header("소비")]
    public int healAmount;
    public int O2UpAmount;
    public int damageAmount;

    [Header("장비")]
    public int damageUpAmount;
    public int maxO2UpAmount;
    public int speedUpAmount;
    public int flyUpAmount;

    public Sprite icon => itemSprite;


    public void Use()
    {
        switch (itemType)
        {
            case ItemType.Consum:
                //실제 소비 효과 적용
                if (healAmount > 0)
                    Debug.Log($"{itemName} 사용: HP {healAmount} 회복");
                if (O2UpAmount > 0)
                    Debug.Log($"{itemName} 사용: O2 {O2UpAmount} 증가");
                if (damageAmount > 0)
                    Debug.Log($"{itemName} 사용: 공격력 {damageAmount} 증가");
                break;

            case ItemType.Equipment:
            case ItemType.Guitar:
                //소비가 아닌 아이템은 Use못함
                Debug.Log($"{itemName}은 사용할 수 없습니다");
                break;
        }
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
    public ItemInfo(string name, ItemType type, int sellPrice, int buyPrice, Sprite sprite, EquipType equip = EquipType.None)
    {
        itemName = name;
        itemType = type;
        this.sellPrice = sellPrice;
        this.buyPrice = buyPrice;
        itemSprite = sprite;
        equipType = equip;
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
