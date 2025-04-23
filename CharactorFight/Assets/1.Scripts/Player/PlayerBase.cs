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
    [SerializeField] private int curHelth; // ����ü��
    [SerializeField] private int maxHelth; // �ִ�ü��

    [SerializeField] protected bool IsAlive = true; // ���� ����

    [Header("DefaultSettings")]
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    protected TouchingDirection td; //���̳� ���� ����ִ� ������ �Ǵ�

    [Header("Choice")]
    [SerializeField] private PlayerChoice currentChoice = PlayerChoice.None;
    [SerializeField] private bool IsChoice; // ���� ����

    [Header("Attack")]
    [SerializeField]  protected int atkStack = 0;
    [SerializeField]  bool IsAttack; // ���� ����

    [Header("PlayerState")]
    [SerializeField] public bool _isMove = false;
    protected bool IsMove// �̵� ����
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
    protected bool IsRun // �޸��� ����
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
    [SerializeField] public bool _isDash; // �뽬 ����
    protected bool IsDash // �޸��� ����
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
        PlayerInitialize(); // ������Ʈ ����
    }
    protected void PlayerInitialize()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        td = gameObject.GetComponent<TouchingDirection>();
    }
}
