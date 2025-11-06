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
        Debug.Log($"[GameLoad] 씬 로드됨: {scene.name}, ShouldLoadGame = {SaveLoadFlag.ShouldLoadGame}");

        if (SaveLoadFlag.ShouldLoadGame)
        {
            Debug.Log("[GameLoad] 저장된 게임 로드 시작");

            //1. 아이템 로드
            if (DataSave.instance != null)
            {
                DataSave.instance.LoadItems();
                Debug.Log("[GameLoad] 아이템 로드 완료");
            }

            //2. 맵 로드
            Map map = FindObjectOfType<Map>();
            if (MapSave.instance != null && map != null)
            {
                MapSave.instance.LoadMap(map);
                Debug.Log("[GameLoad] 맵 로드 완료");
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
