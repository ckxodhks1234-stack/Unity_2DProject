using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] public List<ItemInfo> shopItems = new List<ItemInfo>();

    private void Awake()
    {
        instance = this;
    }

    //아이템 구매
    public bool BuyItem(ItemInfo item)
    {
        if (item == null) return false;
        if(PlayerHP.instance == null || Inventory.instance == null)
        {
            Debug.LogWarning("ShopManager : PlayerStats 또는 Inventory 인스턴스가 없음");
            return false;
        }

        int playerMoney = PlayerHP.instance.money;
        int price = item.buyPrice;

        //인벤토리 공간 확인
        if(!Inventory.instance.HasSpaceFor(item))
        {
            return false;
        }

        if (playerMoney >= price)
        {
            PlayerHP.instance.money -= price;
            Inventory.instance.AddItem(item, 1);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SellItem(ItemInfo item)
    {
        if(item == null) return false;

        if (Inventory.instance == null || PlayerHP.instance == null)
        {
            return false;
        }

        int itemCount = Inventory.instance.GetItemCount(item);

        if(itemCount <= 0)
        {
            return false;
        }

        //슬롯에 아이템 모두 판매
        Inventory.instance.RemoveItem(item, itemCount);
        int totalSellPrice = item.sellPrice * itemCount;
        PlayerHP.instance.money += totalSellPrice;

        return true;
    }

    public bool SellAllMineral()
    {
        if (Inventory.instance == null || PlayerHP.instance == null)
            return false;

        //인벤토리에서 광물만 찾기
        List<ItemInfo> minerals = Inventory.instance.GetItemsType(ItemType.Guitar);

        if (minerals.Count == 0)
        {
            return false;
        }

        int totalGain = 0;

        foreach (var item in minerals)
        {
            int count = Inventory.instance.GetItemCount(item); //슬롯에 있는 개수
            Inventory.instance.RemoveItem(item, count);
            totalGain += item.sellPrice * count;
        }
        PlayerHP.instance.money += totalGain;

        return true;
    }
}
