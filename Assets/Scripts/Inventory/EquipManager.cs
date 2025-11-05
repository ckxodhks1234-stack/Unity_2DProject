using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager instance;

    [Header("장착 슬롯")]
    public EquipSlot drillSlot;
    public EquipSlot helmetSlot;
    public EquipSlot shoesSlot;

    private PlayerController player;
    private PlayerHP hp;

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

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        hp = PlayerHP.instance;

        if (player == null) Debug.LogWarning("EquipManager: PlayerController를 찾을 수 없습니다!");
        if (hp == null) Debug.LogWarning("EquipManager: PlayerHP를 찾을 수 없습니다!");
    }

    public bool EquipItem(ItemInfo item)
    {
        if (item == null || item.itemType != ItemType.Equipment)
            return false;

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
            default:
                Debug.LogWarning($"EquipManager: {item.itemName}아이템타입이 안맞음");
                return false;
        }
        if(targetSlot == null)
        {
            Debug.LogWarning($"EquipManager: 슬롯이 없음 ({item.itemName})");
            return false;
        }

        if (targetSlot.equippedItem != null)
        {
            //기존 장비 해제 후 교체
            NoEquipItem(targetSlot.equippedItem);

            Inventory.instance.AddItem(targetSlot.equippedItem, 1);
        }

        targetSlot.SetItem(item);   //슬롯에 장착
        Debug.Log($"{item.itemName} 장착됨, 아이콘={item.itemSprite}");
        ApplyEquipStats(item);  //능력치 적용

        EquipUI.instance?.UpdateStatUI();
        return true;
    }

    public void NoEquipItem(ItemInfo item)
    {
        if (item == null) return;
        RemoveEquipStats(item);
        EquipUI.instance?.UpdateStatUI();
    }

    //아이템 능력치 적용
    private void ApplyEquipStats(ItemInfo item)
    {
        if(item == null) return;

        if (player == null || hp == null)
        {
            Debug.LogWarning("EquipManager: Player 참조가 null입니다. Start()에서 초기화되지 않았을 수 있습니다.");
            return;
        }

        switch (item.equipType)
        {
            case EquipType.Drill:
                if (player != null) player.ApplyDrillStat(item.damageUpAmount);
                break;
            case EquipType.Helmet:
                if (hp != null) hp.ApplyO2Stat(item.maxO2UpAmount);
                break;
            case EquipType.Shoes:
                if (player != null) player.ApplySpeedStat(item.speedUpAmount, item.flyUpAmount);
                break;
        }
    }

    private void RemoveEquipStats(ItemInfo item)
    {
        if (item == null || player == null || hp == null) return;

        switch (item.equipType)
        {
            case EquipType.Drill:
                player.ApplyDrillStat(-item.damageUpAmount);
                break;
            case EquipType.Helmet:
                hp.ApplyO2Stat(-item.maxO2UpAmount);
                break;
            case EquipType.Shoes:
                player.ApplySpeedStat(-item.speedUpAmount, -item.flyUpAmount);
                break;
        }
    }

    private void UpdateUI()
    {
        if (EquipUI.instance != null)
            EquipUI.instance.UpdateStatUI();
        else
            Debug.LogWarning("EquipUI.instance가 null입니다!");
    }
}
