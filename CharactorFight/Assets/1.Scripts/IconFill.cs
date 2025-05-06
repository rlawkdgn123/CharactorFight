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
        //�ڱ� �ڽ�[�θ� ��ü�� ���� Image ������Ʈ�� ���� ��� 0�� �迭�� ��ġ]�� ������ ������ �ڽ� ��ü ��������Ʈ ä���
        for (int i = 1; i < images.Length; i++)
        {
            images[i].sprite = sprite;
        }
    }
    public void FillIcon(Sprite sprite)
    {
        //�ڱ� �ڽ�[�θ� ��ü�� ���� Image ������Ʈ�� ���� ��� 0�� �迭�� ��ġ]�� ������ ������ �ڽ� ��ü ��������Ʈ ä���
        for(int i = 1; i<images.Length; i++)
        {
            images[i].sprite = sprite;
        }
    }
    
}
