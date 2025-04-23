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

    [Header("Attack")]
    [SerializeField]  protected int atkStack = 0;
    [SerializeField]  bool IsAttack; // 선택 여부

    [Header("PlayerState")]
    [SerializeField] public bool _isMove = false;
    protected bool IsMove// 이동 여부
    {
        get
        {
            return _isMove;
        }
        set
        {
            _isMove = value;
            anim.SetBool(AnimationStrings.isMoving, value);
        }
    }
    [SerializeField] public bool _isRun = false;
    protected bool IsRun // 달리기 여부
    {
        get
        {
            return _isRun;
        }
        set
        {
            _isRun = value;
            anim.SetBool(AnimationStrings.isRunning, value);
        }
    }
    [SerializeField] public bool _isDash; // 대쉬 여부
    protected bool IsDash // 달리기 여부
    {
        get
        {
            return _isDash;
        }
        set
        {
            _isDash = value;
            //anim.SetBool(AnimationStrings.Dash, value);
        }
    }

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
