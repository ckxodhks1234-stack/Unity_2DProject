using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData
{
    public bool isMineral;
    public ItemInfo itemData;
    public int hp;
}

public class Map : MonoBehaviour
{
    public Tilemap groundTile;

    [Header("광물 설정")]
    public Tile[] topMineralTiles;     //위쪽 광물 타일 배열
    public Tile[] midMineralTiles;     //중간 광물 타일 배열
    public Tile[] botMineralTiles;     //아래 광물 타일 배열
    public float mineralChance = 0.1f; //광물 생셩확률

    [Header("타일 설정")]
    public int baseHp = 100;           //기본 HP
    public int upHp = 100;             //hp증가
    public float baseTakeO2 = 1.0f;    //산소 소모량
    public float takeO2Up = 1.0f;      //산소소모량 증가

    [Header("균열")]
    public Tilemap crackTilemap;
    public TileBase crack1;
    public TileBase crack2;
    public TileBase crack3;
    public TileBase crack4;

    //타일별 HP 설정하기 위한 딕셔너리
    public Dictionary<Vector3Int, int> tileHp = new Dictionary<Vector3Int, int>();
    //광물 타일 구별하기 위한 클래스가져오기
    public Dictionary<Vector3Int, TileData> groundData = new Dictionary<Vector3Int, TileData>();

    void Start()
    {
        if (!SaveLoadFlag.ShouldLoadGame)
        { 
             StartTileHP();
        }

    }

    void StartTileHP()
    {
        //그린 타일맵 범위를 가져오기
        BoundsInt bounds = groundTile.cellBounds;

        int topY = bounds.yMax; //땅 가장 위 y좌표 부르기

        //타일맵 전범위 위치
        foreach (var pos in bounds.allPositionsWithin)
        {
            //해당 위치에 타일 확인
            TileBase tile = groundTile.GetTile(pos);

            if (tile == null) continue; //타일이 없으면 넘어가기

            int hp = baseHp;
            int depth = topY - pos.y;   //위에서 얼마나 내려왔는지

            float chance = mineralChance;

            //24, 58부터 단단해지고 광물 많이 나오게
            if (depth > 0 && depth <= 24)
            {
                hp = baseHp;
                chance = mineralChance;
            }
            else if (depth > 24 && depth <= 58)
            {
                hp = baseHp + upHp;
                chance *= 2f;
            }
            else
            {
                hp = baseHp + upHp * 2;
                chance *= 3f;
            }

            //랜덤 광물 나오기
            if (Random.value < chance)
            {
                //깊이에 따라서 광물 나오기(아래 함수에 있음)
                Tile mineralTile = RandomMineralDepth(depth);

                if (mineralTile != null)
                {
                    groundTile.SetTile(pos, mineralTile);
                    ItemInfo item = null;

                    //광물 아이템 찾기
                    if (ItemData.instance != null)
                    {
                        item = ItemData.instance.FindItemName(mineralTile.name);
                    }
                    if(item != null)
                    {
                        MineralTile(pos, true, item, hp);
                    }
                }
            }
            //딕셔너리에 hp저장
            tileHp[pos] = hp;
        }
    }

    //깊이에 따른 산소소모량
    public float TakeO2(float playerY)
    {
        int topY = groundTile.cellBounds.yMax;  //가장 위 타일
        int depth = topY - Mathf.RoundToInt(playerY);
        //RoundToInt -> int로 반올림

        float takeO2 = baseTakeO2;

        if(depth > 0 && depth <= 24)
        {
            takeO2 = baseTakeO2;
        }
        else if(depth > 24 && depth <= 58)
        {
            takeO2 = baseTakeO2 + takeO2Up;
        }
        else
        {
            takeO2 = baseTakeO2 + takeO2Up * 2;
        }
        return takeO2;
    }

    //깊이에 따라 광물 배치
    Tile RandomMineralDepth(int depth)
    {
        List<Tile> tiles = new List<Tile>();

        if (depth > 0 && depth <= 24)
        {
            tiles.AddRange(topMineralTiles);
        }
        //미드 광물이 더 많이 나오게 2번
        else if (depth > 24 && depth <= 58)
        {
            tiles.AddRange(topMineralTiles);
            tiles.AddRange(midMineralTiles);
            tiles.AddRange(midMineralTiles);
        }
        else
        {
            tiles.AddRange(topMineralTiles);
            tiles.AddRange(midMineralTiles);
            tiles.AddRange(botMineralTiles);
            tiles.AddRange(botMineralTiles);
        }

        if (tiles.Count == 0) return null;
        return tiles[Random.Range(0, tiles.Count)];
    }

    //플레이어가 땅을 팔 때 호출
    public bool DamagedTile(Vector3Int tilePosition, int damage)
    {
        if (!tileHp.ContainsKey(tilePosition)) return false;

        //각각의 타일은 위치에 맞는 hp를 가지고 있음
        tileHp[tilePosition] -= damage;
        int hp = tileHp[tilePosition];

        TileBase crackTile = null;

        if (hp > 80)  crackTile = null;    //원래타일 유지
        else if (hp > 60) crackTile = crack1;
        else if (hp > 40) crackTile = crack2;
        else if (hp > 20) crackTile = crack3;
        else if (hp > 0) crackTile = crack4;

        // 광물 데이터 확인
        if (groundData.ContainsKey(tilePosition))
        {
            var tileData = groundData[tilePosition];
            Debug.Log($"타일좌표 : {tilePosition} - 광물유무 : {tileData.isMineral}, 데이터 : {tileData.itemData.itemName}");
        }
        else
        {
            Debug.Log($"타일좌표 {tilePosition} 데이터가 없음");
        }


        if (crackTilemap != null)
        {
            crackTilemap.SetTile(tilePosition, crackTile);
        }

        if(hp <= 0)  //hp 0이면 제거하기
        {
            groundTile.SetTile(tilePosition, null); //타일 제거

            if(crackTilemap != null)
            {
                crackTilemap.SetTile(tilePosition, null);   //균열도 제거
            }
            //땅을 판 타일이 광물이면 아이템 얻기
            if(groundData.ContainsKey(tilePosition) && groundData[tilePosition].isMineral)
            {
                var tileData = groundData[tilePosition];
                if (tileData.itemData == null)
                {
                    return false;
                }
                ItemInfo minedItem = tileData.itemData;

                if (Inventory.instance != null)
                {
                    Debug.Log($"템 획득 : {minedItem.itemName}");
                    Inventory.instance.AddItem(minedItem);
                    InventoryUI.instance.UpdateUI();
                }   
                //파괴된 타일 삭제
                groundData.Remove(tilePosition);
            }
            tileHp.Remove(tilePosition);
            return true;
        }
        return false;   //땅 파는거 중간에 멈추면 false
    }

    //타일의 현재 hp가져오기
    public int GetTileHp(Vector3Int tilePosition)
    {
        if (tileHp.ContainsKey(tilePosition))
        {
            return tileHp[tilePosition];
        }
        return -1;  //타일없음
    }


    public void MineralTile(Vector3Int pos, bool isMineral, ItemInfo item, int hp)
    {
        //광물 데이터얻기
        TileData data = new TileData();
        data.isMineral = isMineral;
        data.itemData = item;
        data.hp = hp;

        groundData[pos] = data; //딕셔너리에 등록
    }

    private ItemInfo FindMineralItem(string tileName)
    {
        if (ItemData.instance == null) return null;

        //타일 이름과 같은 아이템이름 찾기
        foreach(var item in ItemData.instance.allItems)
        {
            if(item.itemName == tileName)
                return item;
        }
        return null;
    }
    public ItemInfo GetMineralItem(Vector3Int tilePos)
    {
        //타일이 광물이면 아이템
        if (groundData[tilePos].isMineral)
        {
            return groundData[tilePos].itemData;
        }
        return null;
    }
}
