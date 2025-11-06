using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;

    private void Awake()
    {
        //새게임 버튼은 항상 활성화
        newGameButton.interactable = true;

        //DataSave와 MapSave JSON 파일 확인
        string itemSavePath = Path.Combine(Application.persistentDataPath, "items.json");
        string mapSavePath = Path.Combine(Application.persistentDataPath, "mapSave.json");

        bool saveExist = false;
        if (File.Exists(itemSavePath) || File.Exists(mapSavePath))
        {
            saveExist = true;
        }
        loadGameButton.interactable = saveExist;

        //버튼 클릭 이벤트 연결
        newGameButton.onClick.AddListener(StartNewGame);
        loadGameButton.onClick.AddListener(LoadGame);
    }

    private void StartNewGame()
    {
        Debug.Log("새 게임 시작");
        SceneManager.LoadScene("SampleScene");
    }

    private void LoadGame()
    {
        Debug.Log("[StartMenu] 불러오기 시도");
        SaveLoadFlag.ShouldLoadGame = true; //JSON으로 불러올 플래그 설정
        SceneManager.LoadScene("SampleScene");
    }
}