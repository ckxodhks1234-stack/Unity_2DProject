using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("슬롯 UI")]
    public Image image;
    public TextMeshProUGUI countText;
    [SerializeField] private Button button;

    private ItemInfo currentItem;
    private int currentAmount;

    //마우스 올라왔는지
    private bool isHovered = false;

    void Start()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ClickSlot);
        }

        ClearSlot();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null) return;

        //우클릭은 장착
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            TryEquipItem();
        }
        //좌클릭은 기존 ClickSlot() 동작
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            ClickSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
    private void TryEquipItem()
    {
        if (EquipManager.instance == null)
        {
            Debug.LogError("EquipManager가 Scene에 없습니다");
            return;
        }
        if (currentItem == null)
        {
            Debug.LogError("currentItem이 null입니다");
            return;
        }

        //장비가 아니면 무시
        if (currentItem.itemType != ItemType.Equipment)
            return;

        // 장비 아이템일 때만 ItemData에서 가져오기
        ItemInfo itemFromData = ItemData.instance?.FindItemName(currentItem.itemName);
        if (itemFromData == null)
        {
            Debug.LogWarning($"ItemData에서 {currentItem.itemName} 아이템을 찾을 수 없음");
            return;
        }

        bool equipped = EquipManager.instance.EquipItem(itemFromData);

        if (equipped)
        {
            //인벤토리에서 제거
            Inventory.instance?.RemoveItem(currentItem, 1);

            //슬롯 초기화
            ClearSlot();
        }
        else
        {
            Debug.LogWarning($"{itemFromData.itemName} 장착 실패.");
        }
    }

    //슬롯 가득찼는지 확인
    public bool IsFull(int count)
    {
        if (currentItem == null) return false;
        return currentAmount >= currentItem.MaxStack;
    }
    //같은 아이템인지
    public bool IsSameItem(ItemInfo item)
    {
        if (currentItem == null || item == null) return false;
        return currentItem.itemName == item.itemName;
    }
    //슬롯 비어잉ㅆ는지
    public bool IsEmpty()
    {
        return currentItem == null;
    }
    //슬롯비우기
    public void ClearSlot()
    {
        currentItem = null;
        currentAmount = 0;

        image.sprite = null;
        image.enabled = false;
        countText.text = "";
    }

    private void ClickSlot()
    {
        if (currentItem == null) return;

        //상점이 열려 땐 판매
        if (ShopUI.instance != null && ShopUI.instance.gameObject.activeSelf)
        {
            ShopUI.instance.OpenSellConfirm(currentItem);
            Debug.Log($"{currentItem.itemName} 판매 시도");
        }
        else
        {
            //평소엔 아이템 사용
            currentItem.Use();
            Debug.Log($"{currentItem.itemName} 사용");
        }

    }

    //아이템 정보, 수량 설정
    public void SetItem(ItemInfo item, int amount)
    {
        currentItem = item;
        currentAmount = amount;

        if(item != null)
        {
            image.sprite = item.itemSprite;   //ItemInfo에서 가져온 Sprite
            image.enabled = true;

            //아이템 수량이 2부터 텍스트
            countText.text = amount > 1 ? $"x{amount}" : "";
        }
        else
        {
            ClearSlot();
        }
    }
}
