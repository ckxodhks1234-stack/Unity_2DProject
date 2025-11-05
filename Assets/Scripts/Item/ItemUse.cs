using UnityEngine;

public class ItemUse : MonoBehaviour
{
    private Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UseItem("폭탄");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            UseItem("산소캡슐");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            UseItem("구급상자");
        }
    }

    private void UseItem(string itemName)
    {
        var item = ItemData.instance.FindItemName(itemName);
        if (item == null)
        {
            Debug.LogWarning($"{itemName} 아이템을 찾을 수 없습니다.");
            return;
        }

        // 인벤토리에서 해당 아이템 존재 확인
        if (!inventory.HasItem(item))
        {
            Debug.Log($"{itemName}이 없습니다");
            return;
        }

        switch (itemName)
        {
            case "폭탄":
                Debug.Log("폭탄 사용");
                break;

            case "산소캡슐":
                var player = PlayerHP.instance;
                player.ApplyO2Stat(100f);
                Debug.Log("산소 회복");
                break;

            case "구급상자":
                var hp = PlayerHP.instance;
                hp.ApplyHeal(50);
                Debug.Log("체력 회복!");
                break;
        }

        //아이템 1개 소모
        inventory.RemoveItem(item, 1);
        InventoryUI.instance.UpdateUI();
    }
}
