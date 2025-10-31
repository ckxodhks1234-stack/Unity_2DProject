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
    //public Sprite image;
    public TileBase tile;
    public Sprite icon;

    [Header("소비")]
    public int healAmount;
    public int O2UpAmount;
    public int damageAmount;

    [Header("장비")]
    public int damageUpAmount;
    public int maxO2UpAmount;
    public int flyForceUpAmount;

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
    public ItemInfo(string name, ItemType type, int price, TileBase itemTile = null)
    {
        itemName = name;
        itemType = type;
        sellPrice = price;
        tile = itemTile;
        //tile에서 sprite추출하기
        if(tile is Tile t && t.sprite != null)
        {
            this.icon = t.sprite;
            Debug.Log($"[ItemInfo] {name} 아이콘 설정됨: {t.sprite.name}");
        }
        else
        {
            this.icon = null;
            Debug.LogWarning($"[ItemInfo] {name} 아이콘 추출 실패 (tile: {tile})");
        }
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

        InitItems();
    }

    private void InitItems()
    {
        //아이템 가격, 능력치 등
        //==============기타 아이템=================
        allItems.Add(new ItemInfo("동", ItemType.Guitar, 50));
        allItems.Add(new ItemInfo("은", ItemType.Guitar, 100));
        allItems.Add(new ItemInfo("금", ItemType.Guitar, 150));
        allItems.Add(new ItemInfo("다이아", ItemType.Guitar, 200));
        allItems.Add(new ItemInfo("자수정", ItemType.Guitar, 300));
        allItems.Add(new ItemInfo("무지개", ItemType.Guitar, 500));
        //==============소비 아이템=================
        var box = new ItemInfo("구급상자", ItemType.Consum, 100);
        box.healAmount = 50;
        allItems.Add(box);

        var capsule = new ItemInfo("산소캡슐", ItemType.Consum, 100);
        capsule.O2UpAmount = 100;
        allItems.Add(capsule);

        var bomb = new ItemInfo("폭탄", ItemType.Consum, 100);
        bomb.damageAmount = 100;
        allItems.Add(bomb);
        //==============장비 아이템=================
        var drill1 = new ItemInfo("드릴1", ItemType.Equipment, 3000);
        drill1.damageUpAmount = 10;
        allItems.Add(drill1);
        var drill2 = new ItemInfo("드릴2", ItemType.Equipment, 5000);
        drill2.damageUpAmount = 20;
        allItems.Add(drill2);

        var helmet1 = new ItemInfo("헬멧1", ItemType.Equipment, 2000);
        helmet1.maxO2UpAmount = 50;
        allItems.Add(helmet1);
        var helmet2 = new ItemInfo("헬멧2", ItemType.Equipment, 5000);
        helmet2.maxO2UpAmount = 100;
        allItems.Add(helmet2);

        var shoes1 = new ItemInfo("신발1", ItemType.Equipment, 3000);
        shoes1.flyForceUpAmount = 5;
        allItems.Add(shoes1);
        var shoes2 = new ItemInfo("신발2", ItemType.Equipment, 5000);
        shoes2.flyForceUpAmount = 15;
        allItems.Add(shoes2);
    }

    public ItemInfo FindItemName(string name)
    {
        return allItems.Find(x => x.itemName == name);
    }
}
