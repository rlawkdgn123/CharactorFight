using UnityEngine;

public class PlayerMark : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite[] mark = new Sprite[2];
    private PlayerBase player;
    void Awake()
    {
        player = gameObject.GetComponentInParent<PlayerBase>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (player.GetCurChoice(PlayerBase.PlayerChoice.P1))
        {
            sr.sprite = mark[0];
        }
        else if (player.GetCurChoice(PlayerBase.PlayerChoice.P2))
        {
            sr.sprite = mark[1];
        }
    }
}
