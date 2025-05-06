using UnityEngine;
using UnityEngine.UI;

public class IconFill : MonoBehaviour
{
    public Sprite sprite;
    public Image[] images;
    private void Awake()
    {
        images = GetComponentsInChildren<Image>();
    }
    public void FillIcon()
    {
        //자기 자신[부모 객체가 같은 Image 컴포넌트가 있을 경우 0번 배열에 배치]을 제외한 나머지 자식 객체 스프라이트 채우기
        for (int i = 1; i < images.Length; i++)
        {
            images[i].sprite = sprite;
        }
    }
    public void FillIcon(Sprite sprite)
    {
        //자기 자신[부모 객체가 같은 Image 컴포넌트가 있을 경우 0번 배열에 배치]을 제외한 나머지 자식 객체 스프라이트 채우기
        for(int i = 1; i<images.Length; i++)
        {
            images[i].sprite = sprite;
        }
    }
    
}
