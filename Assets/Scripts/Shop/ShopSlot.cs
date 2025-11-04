using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    [Header("UI요소들")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    //public TextMeshProUGUI priceText;
    
    [SerializeField] private Button button;

    public ItemInfo itemData;

    private void Start()
    {
        //버튼 클릭하면 확인창 열기
        if(button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ClickSlot);
        }

        SetSlot();
    }
    //슬롯 세팅
    public void SetSlot()
    {
        if (itemData == null)
        {
            Debug.LogWarning($"[ShopSlot] {gameObject.name}에 아이템 정보가 없습니다!");
            return;
        }

        iconImage.sprite = itemData.icon;
        nameText.text = itemData.itemName;
        //priceText.text = $"{itemData.sellPrice}G";
    }

    //구매 시
    private void ClickSlot()
    {
        if(itemData == null) return;
        else
            ShopUI.instance.OpenSellConfirm(itemData);
    }
}