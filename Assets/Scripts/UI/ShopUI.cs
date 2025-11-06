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

    [SerializeField] private Button saveButton;
    [SerializeField] private Map map;

    [SerializeField] private GameObject saveMessagePanel;
    [SerializeField] private TextMeshProUGUI saveMessageText;

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

        Map map = FindObjectOfType<Map>();
        if (map != null && MapSave.instance != null)
        {
            MapSave.instance.LoadMap(map);
        }
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
            moneyText.text = $"You have {PlayerHP.instance.money}G";
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

    public void OnSaveButtonClicked()
    {
        bool success = true;

        //1. 아이템 저장
        if (DataSave.instance != null)
        {
            try
            {
                DataSave.instance.SaveItems();
            }
            catch (System.Exception e)
            {
                Debug.LogError("[Save] 아이템 저장 실패: " + e.Message);
                success = false;
            }
        }
        else
        {
            Debug.LogWarning("[Save] DataSave 인스턴스 없음");
            success = false;
        }

        //2. 맵 저장
        if (MapSave.instance != null && map != null)
        {
            try
            {
                MapSave.instance.SaveMap(map);
            }
            catch (System.Exception e)
            {
                Debug.LogError("[Save] 맵 저장 실패: " + e.Message);
                success = false;
            }
        }
        else
        {
            Debug.LogWarning("[Save] MapSave 또는 Map 인스턴스 없음");
            success = false;
        }

        //3. 결과 메시지 표시
        if (saveMessagePanel != null && saveMessageText != null)
        {
            saveMessageText.text = success ? "Game Save Complete!" : "Can't Save";
            saveMessagePanel.SetActive(true);

            //2초 후 자동으로 닫기
            Invoke(nameof(HideSaveMessage), 2f);
        }

        //4. 콘솔 창
        if (success)
            Debug.Log("[Save] 모든 저장 완료!");
        else
            Debug.LogWarning("[Save] 일부 저장 실패");
    }

    private void HideSaveMessage()
    {
        if (saveMessagePanel != null)
            saveMessagePanel.SetActive(false);
    }
}
