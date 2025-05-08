using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClickButton);
        }
    }
    void Start()
    {
        
    }

    void OnClickButton()
    {
        GameManager.Instance.MainScene();
    }
}
