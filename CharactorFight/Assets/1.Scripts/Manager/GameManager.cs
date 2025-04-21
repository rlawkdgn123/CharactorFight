using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ� ����
        }
    }

    // ���� ����, ���� �� ����
    public int score = 0;
    public bool isGameover = false;
    public float yPosMax = 2.1f;
    public float yPosMin = -2.1f;
    
}