using UnityEngine;
using TMPro;


public class EquipUI : MonoBehaviour
{
    public static EquipUI instance;

    [SerializeField] private TextMeshProUGUI statText; // 한 텍스트로 통합

    private void Awake()
    {
        instance = this;
    }

    public void UpdateStatUI()
    {
        var player = FindObjectOfType<PlayerController>();
        var hp = PlayerHP.instance;

        Debug.Log($"[EquipUI] player={(player != null)}, hp={(hp != null)}, statText={(statText != null)}");

        if (player != null && hp != null)
        {
            statText.text =
                $"Drill Damage: {player.GetDrillDamage()}\n" +
                $"Speed: {player.GetMoveSpeed():0.0}\n" +
                $"Max O₂: {hp.GetMaxO2():0}";
        }
    }
}
