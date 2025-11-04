using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("슬롯 UI")]
    public Image image;                   // 아이템 이미지
    public TextMeshProUGUI countText;     // 아이템 수량 표시
    [SerializeField] private Button button;

    private ItemInfo currentItem;
    private int currentAmount;

    void Start()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ClickSlot);
        }

        ClearSlot();
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
        if(currentItem != null)
        {
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
            image.sprite = item.itemSprite;   // ItemInfo에서 가져온 Sprite
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
