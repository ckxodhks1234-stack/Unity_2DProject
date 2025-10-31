using UnityEngine;

public class MineralBlock : MonoBehaviour
{
    [SerializeField] private ItemInfo itemData;

    public void Mineral()
    {
        Inventory.instance.AddItem(itemData);

        Destroy(gameObject);
    }
}
