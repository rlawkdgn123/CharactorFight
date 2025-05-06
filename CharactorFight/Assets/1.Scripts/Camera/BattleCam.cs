using System.Reflection;
using UnityEngine;

public class BattleCam : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public float minDistance = 5f;     // 가까울 때 거리
    public float maxDistance = 15f;    // 멀어질 때 거리
    public float zoomSpeed = 2f;       // 줌 변화 속도
    public float followSpeed = 5f;     // 카메라 따라가기 속도

    public float minFov = 40f;         // 최소 FOV (Zoom in)
    public float maxFov = 70f;         // 최대 FOV (Zoom out)

    public GameObject camObj;
    private Camera cam;
    public float groundY = 0f;
    public float ave = 0f;
    void Start()
    {
        cam = camObj.GetComponent<Camera>();
    }
    void LateUpdate()
    {
        /*        if (player1 == null || player2 == null) return;

                // 두 플레이어 중간 위치 계산
                Vector3 middlePoint = (player1.position + player2.position) / 2f;


                // 카메라 따라가기
                Vector3 newPos = new Vector3(middlePoint.x, transform.position.y, middlePoint.z - 10f); // Perspective용
                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * followSpeed);

                // 거리 계산
                float distance = Vector3.Distance(player1.position, player2.position);

                // 카메라 줌 조절 (Perspective일 경우 FOV)
                float targetFov = Mathf.Lerp(minFov, maxFov, (distance - minDistance) / (maxDistance - minDistance));
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * zoomSpeed);
      
        */
        
        if (player1 == null || player2 == null) return;
        groundY = Vector3.Distance(player1.position, player2.position)/ave;
        // 1. 두 플레이어 중간 위치 계산
        Vector3 middlePoint = (player1.position + player2.position) / 2f;

        // 2. 거리 계산
        float distance = Vector3.Distance(player1.position, player2.position);

        // 3. 카메라 줌 조절
        float targetFov = Mathf.Lerp(minFov, maxFov, (distance - minDistance) / (maxDistance - minDistance));
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * zoomSpeed);

        // 4. Y 위치 보정 (중간 위치 기준, 바닥이 보이지 않게)
        float lowerY = Mathf.Min(player1.position.y, player2.position.y);
        float clampedY = GetClampedCameraY(lowerY, groundY);

        // 5. 카메라 위치 설정 (Z는 유지)
        Vector3 newPos = new Vector3(middlePoint.x, clampedY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * followSpeed);

        // Orthographic 모드일 경우 (대신 orthographicSize 사용)
        // cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredSize, Time.deltaTime * zoomSpeed);
    }
    private float GetClampedCameraY(float baseY, float groundY)
    {
        float camZ = transform.position.z;
        float distanceZ = Mathf.Abs(camZ - baseY); // baseY 기준으로 거리 계산
        float halfHeight = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * distanceZ;

        float desiredY = baseY;
        float minAllowedY = groundY + halfHeight;

        if (desiredY < minAllowedY)
            desiredY = minAllowedY;

        return desiredY;
    }
}
