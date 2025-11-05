using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Slider o2Slider;
    public Slider healthSlider;

    public Image o2FillImage;
    public Image healthFillImage;

    private PlayerHP playerHP;

    private Color fullHealthColor = Color.red;
    private Color emptyHealthColor = Color.black;
    private Color fullO2Color = Color.blue;
    private Color emptyO2Color = Color.black;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI o2Text;

    void Start()
    {
        playerHP = PlayerHP.instance;

        //최대값 세팅
        if (playerHP != null)
        {
            healthSlider.maxValue = playerHP.GetMaxHealth();
            o2Slider.maxValue = playerHP.GetMaxO2();
        }
    }

    void Update()
    {
        if (playerHP == null) return;

        //체력 현재값 업데이트
        healthSlider.value = playerHP.GetHealth();

        //산소 현재값 업데이트
        o2Slider.value = playerHP.GetO2Amount();

        //색상 변화
        healthFillImage.color = Color.Lerp(emptyHealthColor, fullHealthColor, healthSlider.normalizedValue);
        o2FillImage.color = Color.Lerp(emptyO2Color, fullO2Color, o2Slider.normalizedValue);

        //글씨 업데이트
        healthText.text = $"HP {playerHP.GetHealth()}/{playerHP.GetMaxHealth()}";
        o2Text.text = $"O2 {Mathf.RoundToInt(playerHP.GetO2Amount())}/{playerHP.GetMaxO2()}";
    }
}
