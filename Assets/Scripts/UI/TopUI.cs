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

    private void UpdateUI()
    {
        foreach (var ui in itemUIs)
        {
            var item = ItemData.instance.FindItemName(ui.itemName);
            int count = inventory.GetItemCount(item);
            ui.countText.text = count.ToString();
        }
    }
}
