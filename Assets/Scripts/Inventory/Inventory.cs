using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int capacity = 18;   //인벤용량

    public static Inventory instance;

    //데이터 슬롯리스트
    public List<InventorySlotData> slots = new List<InventorySlotData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("Inventory.instance 등록");
        }
        else Destroy(gameObject);
    }

    //데이터슬롯 클래스
    [System.Serializable]
    public class InventorySlotData
    {
        public ItemInfo item;
        public int count;

        public InventorySlotData(ItemInfo item, int count)
        {
            this.item = item;
            this.count = count;
        }
    }

    //아이템추가
    public void AddItem(ItemInfo newItem, int amount = 1)
    {
        if (newItem == null) return;

        //기존 슬롯에 쌓이는지 확인하기
        foreach (var slot in slots)
        {
            if (slot.item.itemName == newItem.itemName && slot.count < newItem.MaxStack)
            {
                int addable = Mathf.Min(amount, newItem.MaxStack - slot.count);
                slot.count += addable;
                amount -= addable;

                if (amount <= 0) break;
            }
        }

        //안쌓이면 새 슬롯 생성
        while (amount > 0 && slots.Count < capacity)
        {
            int addCount = Mathf.Min(amount, newItem.MaxStack);
            slots.Add(new InventorySlotData(newItem, addCount));
            amount -= addCount;
        }
        InventoryUI.instance?.UpdateUI();
    }

    public void RemoveItem(ItemInfo item, int amount = 1)
    {
        for(int i = slots.Count - 1; i >= 0; i--)
        {
            if (slots[i].item.itemName == item.itemName)
            {
                int removeAmount = Mathf.Min(amount, slots[i].count);
                slots[i].count -= removeAmount;
                amount -= removeAmount;

                if (slots[i].count <= 0)
                {
                    slots.RemoveAt(i);
                }

                if (amount <= 0) break;
            }
        }
        InventoryUI.instance?.UpdateUI();
    }

    //아이템 수 세기
    public int GetItemCount(ItemInfo item)
    {
        int total = 0;
        foreach (var slot in slots)
        {
            if (slot.item.itemName == item.itemName)
                total += slot.count;
        }
        return total;
    }
}
