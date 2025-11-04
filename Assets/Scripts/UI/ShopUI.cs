using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;
    
    [SerializeField] TextMeshProUGUI moneyText;         //돈 표시
    [SerializeField] private GameObject confirmPanel;   //구매.판매확인 패널
    [SerializeField] private TextMeshProUGUI confirmText;//구매,판매확인 글자
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private Transform slotParent;      //슬롯 부모
    [SerializeField] private Button sellAllMinerals;    //광물파는 버튼

    [SerializeField] private List<ShopSlot> shopSlots = new List<ShopSlot>();   //슬롯들 연결

    private ItemInfo targetItem;
    private ConfirmType currentConfirmType;

    private enum ConfirmType { Buy, Sell, SellAllMineral }

    private void Awake()
    {
        instance = this;

        shopSlots.Clear();
        foreach (Transform t in slotParent)
        {
            var slot = t.GetComponent<ShopSlot>();
            if (slot != null)
                shopSlots.Add(slot);
        }
    }

    private void OnEnable()
    {
        UpdateUI();
        CloseConfirmPanel();
    }

    private void Start()
    {
        sellAllMinerals.onClick.AddListener(OnClickSellAllMineralsButton);
    }

    public void OnClickSellAllMineralsButton()
    {
        OpenSellAllMineralConfirm();
    }

    //상점 UI갱신
    public void UpdateUI()
    {
        //플레이어 돈 표시
        if(PlayerHP.instance != null && moneyText != null)
        {
            moneyText.text = $"{PlayerHP.instance.money}G";
        }

        //상점 아이템 슬롯 생성
        foreach(var slot in shopSlots)
        {
                slot.SetSlot();
        }
    }

    //구매 확인
    public void OpenBuyConfirm(ItemInfo item)
    {
        targetItem = item;
        currentConfirmType = ConfirmType.Buy;
        confirmPanel.SetActive(true);
        confirmText.text = $"{item.itemName} purchase? ({item.buyPrice}G)";
        SetupButtons();
    }

    //개별 판매 확인
    public void OpenSellConfirm(ItemInfo item)
    {
        targetItem = item;
        currentConfirmType = ConfirmType.Sell;
        confirmPanel.SetActive(true);
        confirmText.text = $"{item.itemName} sell? ({item.sellPrice}G)";
        SetupButtons();
    }

    //광물 전체판매 확인
    public void OpenSellAllMineralConfirm()
    {
        targetItem = null;
        currentConfirmType = ConfirmType.SellAllMineral;
        confirmPanel.SetActive(true);
        confirmText.text = "Sell all minerals?";
        SetupButtons();
    }

    private void SetupButtons()
    {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            switch (currentConfirmType)
            {
                case ConfirmType.Buy:
                    BuyItem();
                    break;
                case ConfirmType.Sell:
                    SellItem();
                    break;
                case ConfirmType.SellAllMineral:
                    SellAllMinerals();
                    break;
            }
        });

        noButton.onClick.AddListener(CloseConfirmPanel);
    }

    private void BuyItem()
    {
        bool success = ShopManager.instance.BuyItem(targetItem);
        confirmText.text = success ? $"{targetItem.itemName} purchase!" : "No enough money..";
        UpdateUI();
        Invoke(nameof(CloseConfirmPanel), 1f);
    }

    private void SellItem()
    {
        if (targetItem == null) return;
        ShopManager.instance.SellItem(targetItem);
        confirmText.text = $"You sell {targetItem.itemName}";
        UpdateUI();
        Invoke(nameof(CloseConfirmPanel), 1f);
    }

    private void SellAllMinerals()
    {
        bool success = ShopManager.instance.SellAllMineral();
        confirmText.text = success ? "You sell all minerals!" : "No mineral";
        UpdateUI();
        Invoke(nameof(CloseConfirmPanel), 1f);
    }

    public void CloseConfirmPanel()
    {
        confirmPanel.SetActive(false);
        targetItem = null;
    }
}
