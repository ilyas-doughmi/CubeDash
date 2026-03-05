using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    public float smooth = 8f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 pos = transform.position;
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, pos.z);
        transform.position = Vector3.Lerp(pos, targetPos, smooth * Time.deltaTime);
    }
}