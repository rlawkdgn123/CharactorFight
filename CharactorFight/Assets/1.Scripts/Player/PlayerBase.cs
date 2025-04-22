using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TouchingDirection))]
[RequireComponent(typeof(PlayerMove))]
public class PlayerBase : MonoBehaviour
{
    public enum PlayerChoice
    {
        None = 0,
        P1,
        P2,
    }
    [Header("PlayerStats")]
    [SerializeField] private int curHelth; // 현재체력
    [SerializeField] private int maxHelth; // 최대체력


    [SerializeField] protected bool IsAlive = true; // 선택 여부

    [Header("DefaultSettings")]
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    protected TouchingDirection td; //땅이나 벽에 닿아있는 방향을 판단

    [Header("Choice")]
    [SerializeField] private PlayerChoice currentChoice = PlayerChoice.None;
    [SerializeField] private bool IsChoice; // 선택 여부


    public int GetCurHelth() { return curHelth; }
    public int GetMaxHelth() { return maxHelth; }
    public bool GetIsAlive() { return IsAlive; }
    public PlayerChoice GetCurrentChoice() { return currentChoice; }
    public bool GetIsChoice() { return IsChoice; }

    private void Awake()
    {
        PlayerInitialize(); // 컴포넌트 연결
    }
    protected void PlayerInitialize()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        td = gameObject.GetComponent<TouchingDirection>();
    }
}
