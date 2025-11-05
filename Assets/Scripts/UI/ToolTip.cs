using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public static ToolTip instance;

    [SerializeField] private GameObject toolTipPanel;
    [SerializeField] private TextMeshProUGUI toolTipText;

    private string lastMessage = "";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public void ShowToolTip(string message)
    {
        if (lastMessage != message)
        {
            toolTipText.text = message;
            lastMessage = message;
        }
        toolTipPanel.SetActive(true);
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector2 pos = Input.mousePosition;
        toolTipPanel.transform.position = pos + new Vector2(10f, -10f);
    }
    public void HideToolTip()
    {
        toolTipPanel.SetActive(false);
        lastMessage = "";
    }
}
