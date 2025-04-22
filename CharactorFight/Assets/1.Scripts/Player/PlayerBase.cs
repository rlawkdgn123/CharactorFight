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
