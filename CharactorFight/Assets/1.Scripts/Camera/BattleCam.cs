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

    void Start()
    {
        cam = camObj.GetComponent<Camera>();
    }
    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

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

        // Orthographic ����� ��� (��� orthographicSize ���)
        // cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredSize, Time.deltaTime * zoomSpeed);
    }
}
