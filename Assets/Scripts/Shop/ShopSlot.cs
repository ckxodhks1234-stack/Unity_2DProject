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
        if (ShopManager.instance == null) return;
        int index = transform.GetSiblingIndex();

        if (index < ShopManager.instance.shopItems.Count)
        {
            itemData = ShopManager.instance.shopItems[index];
            iconImage.sprite = itemData.icon;
            nameText.text = itemData.itemName;

            GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<Button>().onClick.AddListener(ClickSlot);
        }
    }

    //상점 클릭 시
    private void ClickSlot()
    {
        if (itemData != null)
        {
            ShopUI.instance.OpenBuyConfirm(itemData);
        }
    }
}