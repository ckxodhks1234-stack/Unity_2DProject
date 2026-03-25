using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    public static TilePool instance;
    public TileObject tilePrefab;
    public int poolSize = 200;

    private Queue<TileObject> pool = new Queue<TileObject>();

    void Awake()
    {
        instance = this;

        //pool √ ±‚»≠
        for (int i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(tilePrefab, transform);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public TileObject GetTile()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            var obj = Instantiate(tilePrefab, transform);
            obj.gameObject.SetActive(false);
            return obj;
        }
    }

    public void ReturnTile(TileObject tile)
    {
        tile.Hide();
        pool.Enqueue(tile);
    }
}
