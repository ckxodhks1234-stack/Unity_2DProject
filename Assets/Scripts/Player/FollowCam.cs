using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;
    public float yOffset = 0f;
    public float minX;
    public float maxX;

    void LateUpdate()
    {
        if (player == null) return;

        //목표 위치 계산
        float targetX = Mathf.Clamp(player.position.x, minX, maxX);
        float targetY = player.position.y + yOffset;

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
