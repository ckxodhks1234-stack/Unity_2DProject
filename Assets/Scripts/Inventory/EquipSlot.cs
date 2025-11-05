using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour
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

        //인벤토리에 되돌리기
        if (Inventory.instance != null)
        {
            Inventory.instance.AddItem(equippedItem, 1);
            Debug.Log($"{equippedItem.itemName} 인벤토리로 되돌림");
        }

        //능력치 원복
        EquipManager.instance?.NoEquipItem(equippedItem);

        //슬롯 비우기
        ClearSlot();

        //UI 갱신
        EquipUI.instance?.UpdateStatUI();

        Debug.Log($"{equippedItem.itemName} 장비 해제 완료!");
    }
}
