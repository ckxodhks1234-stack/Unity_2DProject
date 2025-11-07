using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    //슬롯들을 둔 부모
    [SerializeField] private Transform slotsParent;

    private List<InventorySlot> slots = new List<InventorySlot>();

    public ItemInfo item;
    private void Awake()
    {
        instance = this;

        //슬롯 부모밑에 있는 슬롯은 자동으로 자녀
        slots.AddRange(slotsParent.GetComponentsInChildren<InventorySlot>());
    }
    private void OnEnable()
    {
        if (Inventory.instance != null)
            UpdateUI();
        else
            Debug.LogWarning("Inventory.instance가 아직 생성되지 않아 UI 업데이트를 건너뜁니다.");
    }

    public void UpdateUI()
    {
        if (Inventory.instance == null)
        {
            Debug.LogWarning("Inventory.instance가 없어서 UpdateUI를 실행할 수 없습니다.");
            return;
        }

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
