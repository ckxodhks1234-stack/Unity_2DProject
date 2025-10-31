using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ItemData : MonoBehaviour
{
    public static ItemData instance;

    public List<ItemInfo> allItems = new List<ItemInfo>();

    [Header("광물 이미지")]
    public Tile copperSprite;
    public Tile silverSprite;
    public Tile goldSprite;
    public Tile diamondSprite;
    public Tile amethystSprite;
    public Tile rainbowSprite;

    [Header("소비 이미지")]
    public TileBase boxSprite;
    public TileBase capsuleSprite;
    public TileBase bombSprite;

    [Header("장비 아이템 이미지")]
    public TileBase drill1Sprite;
    public TileBase drill2Sprite;
    public TileBase helmet1Sprite;
    public TileBase helmet2Sprite;
    public TileBase shoes1Sprite;
    public TileBase shoes2Sprite;

    private void Awake()
    {
        instance = this;

        //=====================기타===========================
        allItems.Add(new ItemInfo("동", ItemType.Guitar, 50, copperSprite));
        allItems.Add(new ItemInfo("은", ItemType.Guitar, 100, silverSprite));
        allItems.Add(new ItemInfo("금", ItemType.Guitar, 150, goldSprite));
        allItems.Add(new ItemInfo("다이아", ItemType.Guitar, 200, diamondSprite));
        allItems.Add(new ItemInfo("자수정", ItemType.Guitar, 300, amethystSprite));
        allItems.Add(new ItemInfo("무지개", ItemType.Guitar, 500, rainbowSprite));

        //=====================소비===========================
        var box = new ItemInfo("구급상자", ItemType.Consum, 100, boxSprite);
        allItems.Add(box);

        var capsule = new ItemInfo("산소캡슐", ItemType.Consum, 100, capsuleSprite);
        allItems.Add(capsule);

        var bomb = new ItemInfo("폭탄", ItemType.Consum, 100, bombSprite);
        allItems.Add(bomb);

        //=====================장비===========================
        var drill1 = new ItemInfo("드릴1", ItemType.Equipment, 3000, drill1Sprite);
        allItems.Add(drill1);
        var drill2 = new ItemInfo("드릴2", ItemType.Equipment, 5000, drill2Sprite);
        allItems.Add(drill2);

        var helmet1 = new ItemInfo("헬멧1", ItemType.Equipment, 2000, helmet1Sprite);
        allItems.Add(helmet1);
        var helmet2 = new ItemInfo("헬멧2", ItemType.Equipment, 5000, helmet1Sprite);
        allItems.Add(helmet2);

        var shoes1 = new ItemInfo("신발1", ItemType.Equipment, 3000, shoes1Sprite);
        allItems.Add(shoes1);                                      
        var shoes2 = new ItemInfo("신발2", ItemType.Equipment, 5000, shoes2Sprite);
        allItems.Add(shoes2);
    }

    //같은 아이템찾기
    public ItemInfo FindItemName(string name)
    {
        foreach (var item in allItems)
        {
            if( item.itemName == name)
                return item;
        }
        Debug.LogWarning($"[ItemData] {name}아이템 못찾음");
        return null;
    }
}
