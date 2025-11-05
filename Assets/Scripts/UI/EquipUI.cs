using UnityEngine;
using TMPro;


public class EquipUI : MonoBehaviour
{
    public static EquipUI instance;

    [SerializeField] private TextMeshProUGUI statText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateStatUI()
    {
        if (statText == null)
        {
            Debug.LogWarning("EquipUI: statText가 null입니다!");
            return;
        }

        PlayerController player = FindObjectOfType<PlayerController>();
        PlayerHP hp = PlayerHP.instance;

        Debug.Log($"EquipUI: player={(player != null)}, hp={(hp != null)}, statText={(statText != null)}");

        if (player == null || hp == null) return;

        statText.text =
            $"Drill Damage: {player.GetDrillDamage()}\n" +
            $"Max O2: {hp.GetMaxO2():0}\n" +
            $"Speed: {player.GetMoveSpeed():0.0}\n";

        Debug.Log($"EquipUI: UI 갱신 완료");
    }
}
