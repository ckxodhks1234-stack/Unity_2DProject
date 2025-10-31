using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    //슬롯들을 둔 부모
    [SerializeField] private Transform slotsParent;

    private List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        instance = this;
        Debug.Log("InventoryUI.instance 등록");

        //슬롯 부모밑에 있는 슬롯은 자동으로 자녀
        slots.AddRange(slotsParent.GetComponentsInChildren<InventorySlot>());
        Debug.Log($"슬롯 {slots.Count}개 등록");
    }
    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        var inventorySlots = Inventory.instance.slots;

        int i = 0;
        foreach (var slotData in inventorySlots)
        {
            if (i >= slots.Count) break;    //슬롯초과 방지
            slots[i].SetItem(slotData.item, slotData.count);
            i++;
        }

        //남은 슬롯은 비우기
        for (int k = i; k < slots.Count; k++)
        {
                slots[k].ClearSlot();
        }
    }
}
