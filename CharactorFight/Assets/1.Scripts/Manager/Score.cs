using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    TMP_Text score;
    TMP_Text winText;
    void Awake()
    {
        Transform scoreChild = transform.Find("GameScore");
        Transform winTextChild = transform.Find("WinText");   
        if(scoreChild != null)
            score = scoreChild.GetComponent<TMP_Text>();
        if (winTextChild != null)
            winText = winTextChild.GetComponent<TMP_Text>();
    }
    private void Start()
    {
        score.text = GameManager.Instance.p1Score + " : " + GameManager.Instance.p2Score;
    }
    private void Update()
    {
        if (GameManager.Instance.p1Score >= GameManager.Instance.endScore)
        {
            winText.text = "Player1 Win!!";
            winText.gameObject.SetActive(true);
        }
        else if (GameManager.Instance.p2Score >= GameManager.Instance.endScore)
        {
            winText.text = "Player2 Win!!";
            winText.gameObject.SetActive(true);
        }
    }
}