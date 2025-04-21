using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }
    }

    // 게임 상태, 점수 등 관리
    public int score = 0;
    public bool isGameover = false;
    public float yPosMax = 2.1f;
    public float yPosMin = -2.1f;
    
}