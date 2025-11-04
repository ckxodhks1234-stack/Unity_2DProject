using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager instance;

    [Header("장착 슬롯")]
    public EquipSlot drillSlot;
    public EquipSlot helmetSlot;
    public EquipSlot shoesSlot;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("EquipManager instance가 정상적으로 세팅되었습니다.");
        }
        else if (instance != this)
        {
            Debug.LogWarning("중복 EquipManager가 발견되어 제거되었습니다.");
            Destroy(gameObject);
        }
    }

    public void EquipItem(ItemInfo item)
    {
        Debug.Log($"EquipType of {item.itemName} is {item.equipType}");
        Debug.Log($"DrillSlot is {drillSlot}, HelmetSlot is {helmetSlot}, ShoesSlot is {shoesSlot}");

        if (item == null || item.itemType != ItemType.Equipment)
            return;

        EquipSlot targetSlot = null;

        switch (item.equipType)
        {
            case EquipType.Drill:
                targetSlot = drillSlot;
                break;
            case EquipType.Helmet:
                targetSlot = helmetSlot;
                break;
            case EquipType.Shoes:
                targetSlot = shoesSlot;
                break;
        }
        if(targetSlot == null)
        {
            Debug.LogWarning($"EquipManager: No target slot found for {item.itemName}");
            return;
        }
        if (targetSlot.equippedItem != null)
        {
            //기존 장비 해제 후 교체
            NoEquipItem(targetSlot.equippedItem);
            Inventory.instance.AddItem(targetSlot.equippedItem, 1);
        }

        targetSlot.SetItem(item);
        Debug.Log($"{item.itemName} 장착됨, 아이콘={item.itemSprite}");
        ApplyEquipStats(item);
        EquipUI.instance.UpdateStatUI();
    }

    public void NoEquipItem(ItemInfo item)
    {
        if (item == null) return;
        RemoveEquipStats(item);
        EquipUI.instance.UpdateStatUI();
    }

    private void ApplyEquipStats(ItemInfo item)
    {
        if (item.equipType == EquipType.Drill)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            player.ApplyDrillStat(item.damageUpAmount);
        }
        else if (item.equipType == EquipType.Helmet)
        {
            PlayerHP.instance.ApplyO2Stat(item.maxO2UpAmount);
        }
        else if (item.equipType == EquipType.Shoes)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            player.ApplySpeedStat(item.speedUpAmount, item.flyUpAmount);
        }
    }

    private void RemoveEquipStats(ItemInfo item)
    {
        if (item.equipType == EquipType.Drill)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            player.ApplyDrillStat(-item.damageUpAmount);
        }
        else if (item.equipType == EquipType.Helmet)
        {
            PlayerHP.instance.ApplyO2Stat(-item.maxO2UpAmount);
        }
        else if (item.equipType == EquipType.Shoes)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            player.ApplySpeedStat(-item.speedUpAmount, -item.flyUpAmount);
        }
    }
}
