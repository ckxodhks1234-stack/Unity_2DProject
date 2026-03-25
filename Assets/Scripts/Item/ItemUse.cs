using UnityEngine;

public class ItemUse : MonoBehaviour
{
    private Inventory inventory;
    private TopUI topUI;

    public GameObject bombPrefab;
    void Start()
    {
        inventory = Inventory.instance;
        topUI = FindAnyObjectByType<TopUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseItem("Bomb");
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            UseItem("Capsule");
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            UseItem("Box");
        }
    }

    private void UseItem(string itemName)
    {
        ItemInfo item = ItemData.instance.FindItemName(itemName);
        if (item == null)
        {
            Debug.LogWarning($"{itemName} 아이템을 찾을 수 없습니다.");
            return;
        }

        //인벤토리에서 해당 아이템 존재 확인
        if (!inventory.HasItem(item))
        {
            return;
        }

        switch (itemName)
        {
            case "Bomb":
                //폭탄 프리팹 생성
                if (bombPrefab != null)
                {
                    Instantiate(bombPrefab, PlayerHP.instance.transform.position, Quaternion.identity);
                }
                break;

            case "Capsule":
                if (PlayerHP.instance != null)
                {
                    PlayerHP.instance.ApplyO2Up(100f);
                }
                break;

            case "Box":
                if (PlayerHP.instance != null)
                {
                    PlayerHP.instance.ApplyHeal(50);
                }
                break;
        }

        //아이템 1개 소모
        inventory.RemoveItem(item, 1);
        topUI?.UpdateUI();
    }
}
