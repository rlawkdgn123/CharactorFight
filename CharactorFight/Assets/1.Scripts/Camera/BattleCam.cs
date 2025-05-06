using System.Reflection;
using UnityEngine;

public class BattleCam : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public float minDistance = 5f;     // ����� �� �Ÿ�
    public float maxDistance = 15f;    // �־��� �� �Ÿ�
    public float zoomSpeed = 2f;       // �� ��ȭ �ӵ�
    public float followSpeed = 5f;     // ī�޶� ���󰡱� �ӵ�

    public float minFov = 40f;         // �ּ� FOV (Zoom in)
    public float maxFov = 70f;         // �ִ� FOV (Zoom out)

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

                // �� �÷��̾� �߰� ��ġ ���
                Vector3 middlePoint = (player1.position + player2.position) / 2f;


                // ī�޶� ���󰡱�
                Vector3 newPos = new Vector3(middlePoint.x, transform.position.y, middlePoint.z - 10f); // Perspective��
                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * followSpeed);

                // �Ÿ� ���
                float distance = Vector3.Distance(player1.position, player2.position);

                // ī�޶� �� ���� (Perspective�� ��� FOV)
                float targetFov = Mathf.Lerp(minFov, maxFov, (distance - minDistance) / (maxDistance - minDistance));
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * zoomSpeed);
      
        */
        
        if (player1 == null || player2 == null) return;
        groundY = Vector3.Distance(player1.position, player2.position)/ave;
        // 1. �� �÷��̾� �߰� ��ġ ���
        Vector3 middlePoint = (player1.position + player2.position) / 2f;

        // 2. �Ÿ� ���
        float distance = Vector3.Distance(player1.position, player2.position);

        // 3. ī�޶� �� ����
        float targetFov = Mathf.Lerp(minFov, maxFov, (distance - minDistance) / (maxDistance - minDistance));
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * zoomSpeed);

        // 4. Y ��ġ ���� (�߰� ��ġ ����, �ٴ��� ������ �ʰ�)
        float lowerY = Mathf.Min(player1.position.y, player2.position.y);
        float clampedY = GetClampedCameraY(lowerY, groundY);

        // 5. ī�޶� ��ġ ���� (Z�� ����)
        Vector3 newPos = new Vector3(middlePoint.x, clampedY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * followSpeed);

        // Orthographic ����� ��� (��� orthographicSize ���)
        // cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredSize, Time.deltaTime * zoomSpeed);
    }
    private float GetClampedCameraY(float baseY, float groundY)
    {
        float camZ = transform.position.z;
        float distanceZ = Mathf.Abs(camZ - baseY); // baseY �������� �Ÿ� ���
        float halfHeight = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * distanceZ;

        float desiredY = baseY;
        float minAllowedY = groundY + halfHeight;

        if (desiredY < minAllowedY)
            desiredY = minAllowedY;

        return desiredY;
    }
}
