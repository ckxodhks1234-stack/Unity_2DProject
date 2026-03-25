using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    public Transform player;

    public Vector3 offset = new Vector3(0, 0, -10);

    private float fixedX;

    private void Start()
    {
        fixedX = transform.position.x;
    }
    void LateUpdate()
    {
        if (player == null) return;

        Vector3 newPos = new Vector3(fixedX, player.position.y, player.position.z) + offset;
        transform.position = newPos;
    }
}