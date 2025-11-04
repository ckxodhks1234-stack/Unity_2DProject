using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        icon.sprite = item.itemSprite;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        equippedItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    private void Update()
    {
        //우클릭으로 장비 해제
        if (equippedItem != null && Input.GetMouseButtonDown(1))
        {
            Inventory.instance.AddItem(equippedItem, 1);
            EquipManager.instance.NoEquipItem(equippedItem);
            ClearSlot();
        }
    }

    private void NoEquip()
    {
        //인벤토리로 되돌리기
        Inventory.instance.AddItem(equippedItem, 1);
        //플레이어 능력치 원상복귀
        EquipManager.instance.NoEquipItem(equippedItem);

        ClearSlot();
        //UI 갱신
        EquipUI.instance.UpdateStatUI();
    }
}
