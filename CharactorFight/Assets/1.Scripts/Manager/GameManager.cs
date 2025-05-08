using UnityEngine;
using UnityEngine.SceneManagement;
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
    private void Update()
    {
        if (isGameSet && Input.GetKeyDown(KeyCode.Space))
        {
            IntroScene();
            p1Score = 0;
            p2Score = 0;
            isGameSet = false;
        }
    }
    public void IntroScene()
    {
        SceneManager.LoadScene("0.Intro");
    }
    public void MainScene()
    {
        SceneManager.LoadScene("1.Main");
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
    }
    
    // 게임 상태, 점수 등 관리
    public int p1Score = 0;
    public int p2Score = 0;
    public int endScore = 3;
    public bool isGameSet = false;
    public float yPosMax = 2.1f;
    public float yPosMin = -2.1f;
    
}