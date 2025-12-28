using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // หันหน้าเข้าหากล้อง
        transform.forward = cam.transform.forward;
    }
}
