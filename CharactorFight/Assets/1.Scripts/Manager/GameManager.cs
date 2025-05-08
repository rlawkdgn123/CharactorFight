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
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ� ����
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� �� �ٽ� �ε�
    }
    
    // ���� ����, ���� �� ����
    public int p1Score = 0;
    public int p2Score = 0;
    public int endScore = 3;
    public bool isGameSet = false;
    public float yPosMax = 2.1f;
    public float yPosMin = -2.1f;
    
}