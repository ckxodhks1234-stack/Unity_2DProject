using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button quitButton;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void Show()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void OnClickNewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickQuit()
    {
        Time.timeScale = 1f;
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false; //에디터에서만 플레이 중지
    }

    public void OnClickLoadGame()
    {
        Time.timeScale = 1f;
        SaveLoadFlag.ShouldLoadGame = true;
        SceneManager.LoadScene("SampleScene");
    }
}