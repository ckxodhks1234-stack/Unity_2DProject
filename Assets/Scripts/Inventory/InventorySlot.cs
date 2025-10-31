using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class InventorySlot : MonoBehaviour
{
    public Image image;     //아이템이미지
    public Button button;
    public TextMeshProUGUI countText;   //아이템 수량 표시
    private const int maxStack = 5; //최대 5개

    private ItemInfo currentItem;

    public Sprite icon;

    void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(ClickSlot);
        }

        ClearSlot();
    }

    //슬롯 가득찼는지 확인
    public bool IsFull(int count)
    {
        return currentItem != null && count >= maxStack;
    }
    //같은 아이템인지
    public bool IsSameItem(ItemInfo item)
    {
        return currentItem != null && currentItem.itemName == item.itemName;
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
    public void SetItem(ItemInfo item, int count)
    {
        currentItem = item;

        if(item != null)
        {
            //슬롯에 아이템이미지 가져오기
            if (item.icon != null)
            {
                image.enabled = true;
                image.sprite = item.icon;
            }
            else
            {
                //tile타입이 아니면
                Debug.LogWarning($"[InventorySlot] {item.itemName}은 Tile 타입이 아니라 sprite를 가져올 수 없습니다.");
                image.enabled=false;
            }

            //아이템 수량이 2부터 텍스트
            countText.text = count > 1 ? count.ToString() : "";
        }
        else
        {
            ClearSlot();
        }
    }
}
