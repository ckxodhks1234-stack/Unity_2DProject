using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopUI : MonoBehaviour
{
    [System.Serializable]
    public class ItemUI
    {
        public string itemName;
        public Image image;
        public TMP_Text countText;
    }

    public ItemUI[] itemUIs;

    private Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (ItemData.instance == null || inventory == null) return;

        foreach (var ui in itemUIs)
        {
            if (ui == null || ui.countText == null) continue;

            ItemInfo item = ItemData.instance.FindItemName(ui.itemName);

            if (item != null)
            {
                int count = inventory.GetItemCount(item);
                ui.countText.text = count.ToString();
            }
            else
            {
                ui.countText.text = "0";
            }
        }
    }
}
