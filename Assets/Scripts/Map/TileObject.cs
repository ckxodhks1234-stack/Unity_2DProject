using UnityEngine;

public class TileObject : MonoBehaviour
{
    public Vector3Int tilePos;
    public SpriteRenderer sr;

    public void Setup(TileData data, Sprite sprite, Vector3 worldPos)
    {
        tilePos = new Vector3Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y), 0);
        sr.sprite = sprite;
        transform.position = worldPos;
        gameObject.SetActive(true);
    }

    //안보이면 비활성화하기
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
