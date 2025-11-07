using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;

    public ItemInfo equippedItem;

    void Start()
    {
        ClearSlot();
    }

    public void SetItem(ItemInfo item)
    {
        equippedItem = item;
        icon.sprite = item?.itemSprite;
        icon.enabled = item != null;
    }

    public void ClearSlot()
    {
        equippedItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 우클릭 감지
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            TryNoEquip();
        }
    }

    private void TryNoEquip()
    {
        if (equippedItem == null)
            return;

        //능력치 복구
        EquipManager.instance?.NoEquipItem(equippedItem);
        //인벤토리에 추가
        Inventory.instance?.AddItem(equippedItem, 1);
        //슬롯 비우기
        ClearSlot();

        InventoryUI.instance?.UpdateUI();
        EquipUI.instance?.UpdateStatUI();
    }
}
