using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoad : MonoBehaviour
{
    [SerializeField] private Map map;
    private static GameLoad instance;

    void Awake()
    {
        //싱글턴 중복 방지
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        //씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SaveLoadFlag.ShouldLoadGame)
        {
            //1. 아이템 로드
            if (DataSave.instance != null)
            {
                DataSave.instance.LoadItems();
            }

            //2. 맵 로드
            Map map = FindObjectOfType<Map>();
            if (MapSave.instance != null && map != null)
            {
                MapSave.instance.LoadMap(map);
            }

            SaveLoadFlag.ShouldLoadGame = false;
        }
        else
        {
            Debug.Log("[GameLoad] 새 게임 시작");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
