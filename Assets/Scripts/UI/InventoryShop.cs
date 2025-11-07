using System.Collections;
using TMPro;
using UnityEngine;

public class InventoryShop : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private TextMeshProUGUI moneyText; //플레이어 돈 글자

    private bool isInventoryOpen = false;
    private bool isShopOpen = false;

    void Start()
    {
        //인벤토리 UI 갱신을 위해 켜둔 상태에서 바로 끄기
        StartCoroutine(HideInventoryStart());
    }

    IEnumerator HideInventoryStart()
    {
        //한 프레임 기다려서 Awake/Start 다 실행되게 하기
        yield return null;
        inventoryUI.SetActive(false);
    }
    void Update()
    {
        //i누르면 인벤토리 열고 닫힘
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isInventoryOpen)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }

        //esc로 전부 다 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAll();
        }
    }

    public void OpenInventory()
    {
        isInventoryOpen = true;
        inventoryUI.SetActive(true);

        //인벤토리 열 때 최신상태 갱신
        if(InventoryUI.instance != null)
        {
            InventoryUI.instance.UpdateUI();
        }

        moneyText.text = $"You have {PlayerHP.instance.money}G";
    }

    public void CloseInventory()
    {
        isInventoryOpen = false;
        inventoryUI.SetActive(false);
    }

    public void OpenShop()
    {
        isShopOpen = true;
        shopUI.SetActive(true);

        //플레이어가 상점으로 가면 자동으로 열림
        //상점 열릴 때 인벤토리도 자동으로 열리게
        OpenInventory();
    }

    public void CloseAll()
    {
        isInventoryOpen = false;
        if(inventoryUI) inventoryUI.SetActive(false);
        isShopOpen = false;
        if(shopUI) shopUI.SetActive(false);
    }
}
