using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    [SerializeField] private Transform sub_background; // 다음으로 이어지는 배경

    [SerializeField] private float scrollSpeed;    // 배경화면 이동속도
    [SerializeField] private float scrollAmount; // 배경화면 간격
    [SerializeField] private Vector3 moveDir;    // 배경화면 이동방향
    [SerializeField] private bool isScroll = true;              // 스크롤 여부

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
