using System.Collections.Generic;
using UnityEngine;


public class ItemData : MonoBehaviour
{
    public static ItemData instance;

    public List<ItemInfo> allItems = new List<ItemInfo>();

    [Header("광물 이미지")]
    public Sprite copperSprite;
    public Sprite silverSprite;
    public Sprite goldSprite;
    public Sprite diamondSprite;
    public Sprite amethystSprite;
    public Sprite rainbowSprite;

    [Header("소비 이미지")]
    public Sprite boxSprite;
    public Sprite capsuleSprite;
    public Sprite bombSprite;

    [Header("장비 아이템 이미지")]
    public Sprite drill1Sprite;
    public Sprite drill2Sprite;
    public Sprite helmet1Sprite;
    public Sprite helmet2Sprite;
    public Sprite shoes1Sprite;
    public Sprite shoes2Sprite;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        //=====================기타===========================
        allItems.Add(new ItemInfo("Copper", ItemType.Guitar, 50, 0, copperSprite));
        allItems.Add(new ItemInfo("Silver", ItemType.Guitar, 100, 0, silverSprite));
        allItems.Add(new ItemInfo("Gold", ItemType.Guitar, 150, 0, goldSprite));
        allItems.Add(new ItemInfo("Diamond", ItemType.Guitar, 200, 0, diamondSprite));
        allItems.Add(new ItemInfo("Amethyst", ItemType.Guitar, 300, 0, amethystSprite));
        allItems.Add(new ItemInfo("Rainbow", ItemType.Guitar, 500, 0, rainbowSprite));

        //=====================소비===========================
        var box = new ItemInfo("Box", ItemType.Consum, 200, 500, boxSprite);
        allItems.Add(box);

        var capsule = new ItemInfo("Capsule", ItemType.Consum, 200, 500, capsuleSprite);
        allItems.Add(capsule);

        var bomb = new ItemInfo("Bomb", ItemType.Consum, 50, 100, bombSprite);
        allItems.Add(bomb);

        //=====================장비===========================
        var drill1 = new ItemInfo("RDrill", ItemType.Equipment, 2000, 3000, drill1Sprite, EquipType.Drill);
        drill1.damageUpAmount = 20;
        allItems.Add(drill1);

        var drill2 = new ItemInfo("BDrill", ItemType.Equipment, 5000, 7000, drill2Sprite, EquipType.Drill);
        drill2.damageUpAmount = 40;
        allItems.Add(drill2);

        var helmet1 = new ItemInfo("YHelmet", ItemType.Equipment, 2000, 3000, helmet1Sprite, EquipType.Helmet);
        helmet1.maxO2UpAmount = 50;
        allItems.Add(helmet1);

        var helmet2 = new ItemInfo("RHelmet", ItemType.Equipment, 5000, 7000, helmet1Sprite, EquipType.Helmet);
        helmet2.maxO2UpAmount = 100;
        allItems.Add(helmet2);

        var shoes1 = new ItemInfo("YShoes", ItemType.Equipment, 2000, 3000, shoes1Sprite, EquipType.Shoes);
        shoes1.speedUpAmount = 2;
        shoes1.flyUpAmount = 2;
        allItems.Add(shoes1);        
        
        var shoes2 = new ItemInfo("RShoes", ItemType.Equipment, 5000, 7000, shoes2Sprite, EquipType.Shoes);
        shoes2.speedUpAmount = 5;
        shoes2.flyUpAmount = 5;
        allItems.Add(shoes2);
    }

    //같은 아이템찾기
    public ItemInfo FindItemName(string name)
    {
        foreach (var item in allItems)
        {
            if(item.itemName == name)
                return item;
        }
        Debug.LogWarning($"[ItemData] {name}아이템 못찾음");
        return null;
    }
}
