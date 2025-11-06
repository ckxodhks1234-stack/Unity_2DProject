using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

//각 타일 데이터 저장용
[Serializable]//-> JSON으로 직렬화 가능하게 만듦
public class TileSaveData
{
    public int x;
    public int y;
    public int z;
    public int hp;
    public bool isMineral;
    public string itemName;
}

//맵 전체 저장용
[Serializable]
public class MapSaveWrapper
{
    //List<T>만 JSON으로 직렬화 가능
    public List<TileSaveData> tiles = new List<TileSaveData>();
}
public class MapSave : MonoBehaviour
{
    public static MapSave instance;
    public string savePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        savePath = Path.Combine(Application.persistentDataPath, "mapSave.json");
        //씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedLoad());
    }

    private IEnumerator DelayedLoad()
    {
        yield return null; //한 프레임 대기

        if (SaveLoadFlag.ShouldLoadGame)
        {
            //아이템 로드
            DataSave.instance?.LoadItems();
            //맵 로드
            Map map = FindObjectOfType<Map>();
            if (map != null)
                MapSave.instance?.LoadMap(map);

            SaveLoadFlag.ShouldLoadGame = false;
        }
    }

    //맵 저장
    public void SaveMap(Map map)
    {
        MapSaveWrapper saveWrapper = new MapSaveWrapper();

        foreach (var kvp in map.groundData)
        {
            TileData data = kvp.Value;
            Vector3Int pos = kvp.Key;

            TileSaveData tileSave = new TileSaveData()
            {
                //JSON은 Vector3Int를 지원하지 않으므로 개별 좌표로 저장
                x = pos.x,
                y = pos.y,
                z = pos.z,
                hp = data.hp,
                isMineral = data.isMineral,
                itemName = data.itemData != null ? data.itemData.itemName : null
            };

            saveWrapper.tiles.Add(tileSave);
        }

        string json = JsonUtility.ToJson(saveWrapper, true);
        File.WriteAllText(savePath, json);
        Debug.Log("[MapSave] 맵 저장 완료");
    }

    //맵 로드
    public void LoadMap(Map map)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("[MapSave] 저장된 맵 없음, 새로 시작");
            return;
        }

        string json = File.ReadAllText(savePath);
        MapSaveWrapper saveWrapper = JsonUtility.FromJson<MapSaveWrapper>(json);

        foreach (var tileSave in saveWrapper.tiles)
        {
            Vector3Int pos = new Vector3Int(tileSave.x, tileSave.y, tileSave.z);

            //새로운 TileData 생성/설정
            TileData data = new TileData();
            data.hp = tileSave.hp;
            data.isMineral = tileSave.isMineral;

            if (data.isMineral && !string.IsNullOrEmpty(tileSave.itemName))
            {
                data.itemData = ItemData.instance.FindItemName(tileSave.itemName);
            }

            //grondData 반영
            map.groundData[pos] = data;

            //tileHp 반영
            map.tileHp[pos] = tileSave.hp;

            //HP에 따라 균열 타일 적용
            TileBase crackTile = null;
            if (tileSave.hp <= 0)
            {
                map.groundTile.SetTile(pos, null);
                if (map.crackTilemap != null) map.crackTilemap.SetTile(pos, null);
            }
            else if (tileSave.hp <= 20) crackTile = map.crack4;
            else if (tileSave.hp <= 40) crackTile = map.crack3;
            else if (tileSave.hp <= 60) crackTile = map.crack2;
            else if (tileSave.hp <= 80) crackTile = map.crack1;

            if (map.crackTilemap != null)
                map.crackTilemap.SetTile(pos, crackTile);
        }

        Debug.Log("[MapSave] 맵 로드 완료");

        //아이템 인벤토리도 UI업데이트
        if (Inventory.instance != null)
            InventoryUI.instance.UpdateUI();
    }
}
