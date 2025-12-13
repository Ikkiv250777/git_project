using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // ตัวละคร
    public Vector3 offset;     // ระยะห่างของกล้องจาก Player
    public float smoothSpeed = 10f;
    

    void LateUpdate()
    {
        if (target == null) return;

        // ตำแหน่งที่กล้องควรไป
        Vector3 desiredPosition = target.position + offset;

        // ทำให้กล้องเคลื่อนแบบลื่น ๆ
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothed;

        // ให้กล้องหันลงไปหาพื้น (ถ้าต้องการ)
        // transform.LookAt(target);
    }
}
