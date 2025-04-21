using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    [SerializeField] private Transform sub_background; // �������� �̾����� ���

    [SerializeField] private float scrollSpeed;    // ���ȭ�� �̵��ӵ�
    [SerializeField] private float scrollAmount; // ���ȭ�� ����
    [SerializeField] private Vector3 moveDir;    // ���ȭ�� �̵�����
    [SerializeField] private bool isScroll = true;              // ��ũ�� ����

    void SetScroll(bool flag) { isScroll = flag; }
    void Update()
    {
        if(isScroll)
            transform.position += moveDir * scrollSpeed * Time.deltaTime;
    
        if (transform.position.x <= -scrollAmount && isScroll)
        {
            transform.position = sub_background.position - moveDir * scrollAmount;
        }
    }
}
