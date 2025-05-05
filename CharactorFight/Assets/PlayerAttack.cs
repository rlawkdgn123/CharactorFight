using UnityEngine;
using static PlayerBase;

public class PlayerAttack : MonoBehaviour
{
    [Header("AttackCollider")]
    [SerializeField] public Collider2D[] col;
    [SerializeField] public Collider2D[] Aircol;

    [Header("DefaultSettings")]
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerBase player;

    [Header("AnimationInfo")]
    [SerializeField] public int atkComboMax = 3;
    [SerializeField] public int airAtkComboMax = 3;
    [SerializeField] public int curAtkCombo = 0;
    [SerializeField] public int curAirAtkCombo = 0;
    [SerializeField] public float[] atkDuration = { 1, 1, 1 };
    [SerializeField] public float airAtkEndTime = 0.9f;
    [SerializeField] public float lastAttackTime = 0f;
    [SerializeField] public float comboResetTime = 1.0f; // �޺� �Է� ���� �ð�
    [SerializeField] public bool canAirAttack = true;
    
    private void Awake()
    {
        PlayerInitialize();
    }
    private void Start()
    {
        for(int i = 0; i < atkComboMax; i++)
        {
            if (col[i] && player.GetCurChoice(PlayerBase.PlayerChoice.P1))
            {
                col[i].gameObject.layer = LayerMask.NameToLayer("P1Attack");  // P1Attack ���̾� �Ҵ�
            }
            else if (col[i] && player.GetCurChoice(PlayerBase.PlayerChoice.P2))
            {
                col[i].gameObject.layer = LayerMask.NameToLayer("P2Attack");  // P2Attack ���̾� �Ҵ�
            }
        }
        for(int j = 0; j < airAtkComboMax; j++)
        {
            if (Aircol[j] && player.GetCurChoice(PlayerBase.PlayerChoice.P1))
            {
                Aircol[j].gameObject.layer = LayerMask.NameToLayer("P1Attack");  // P1AirAttack ���̾� �Ҵ�
            }
            else if (Aircol[j] && player.GetCurChoice(PlayerBase.PlayerChoice.P2))
            {
                Aircol[j].gameObject.layer = LayerMask.NameToLayer("P2Attack");  // P2AirAttack ���̾� �Ҵ�
            }
        }
    }
    public void Attack()

    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            curAtkCombo = 0; // ���� �ð� ������ �޺� �ʱ�ȭ
        }
    }
    public void AttackInput()
    {
        curAtkCombo++;

        if (curAtkCombo > atkComboMax)

            curAtkCombo = 1; // 3�޺� ���Ŀ��� �ٽ� 1�޺��� �̾������� (���� ����)

        //anim.SetInteger("Player_attack_", curAtkCombo);

        anim.SetTrigger("attackTrigger");

        lastAttackTime = Time.time;

    }
    public void AirAttackInput()
    {
        curAtkCombo++;

        if (curAtkCombo > airAtkComboMax)

            curAtkCombo = 1; // 3�޺� ���Ŀ��� �ٽ� 1�޺��� �̾������� (���� ����)

        //anim.SetInteger("Player_attack_", curAtkCombo);

        anim.SetTrigger(AnimationStrings.airAttackTrigger);

        lastAttackTime = Time.time;

    }
    // �ִϸ��̼� �̺�Ʈ�� �޺� �ʱ�ȭ �Լ� ���� ����

    public void ResetCombo()

    {

        curAtkCombo = 0;

        anim.SetInteger("comboStep", curAtkCombo);

    }
    bool InputTrue() { return true; }
    //if (IsAlive && !pMove.isDoubleTap)
    //{
    //    if(Time.time >= state_time) { print("�ð���"); }
    //    if (Time.time >= state_time && pMove.moveInput == Vector2.zero || curAtkCombo >= atkComboMax && pMove.moveInput == Vector2.zero) { PlayerStateStart(PlayerState.Idle); curAtkCombo = 0; break; }
    //    if (Time.time >= state_time && pMove.moveInput != Vector2.zero || curAtkCombo >= atkComboMax && pMove.moveInput != Vector2.zero) { PlayerStateStart(PlayerState.Move); curAtkCombo = 0; break; }
    //}
    //if (IsAlive && curAtkCombo <= atkComboMax && Time.time <= state_time && ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.V)) 
    //    || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Comma)))) { PlayerStateStart(PlayerState.Attack);
    //    }
    //if ((Time.time >= state_time || curAtkCombo >= atkComboMax) && pMove.moveInput == Vector2.zero)
    //{
    //    PlayerStateStart(PlayerState.Idle);
    //    curAtkCombo = 0;
    //}
    //if ((Time.time >= state_time || curAtkCombo >= atkComboMax) && pMove.moveInput != Vector2.zero)
    //{
    //    PlayerStateStart(PlayerState.Move);
    //    curAtkCombo = 0;
    //}
    //float comboInputWindow = 0.3f; // �޺� �Է� ���� �ð�
    //if (IsAlive && curAtkCombo <= atkComboMax &&
    //    (state_time - Time.time <= comboInputWindow) &&
    //    ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.V))
    //    || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Comma))))
    //{
    //    PlayerStateStart(PlayerState.Attack);
    //}
    //���ۺ��� // movespeed = 0;
    public void PlayerInitialize()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponent<PlayerBase>();

        col = new Collider2D[atkComboMax];
        Aircol = new Collider2D[airAtkComboMax];

        for (int i = 0; i < atkComboMax; i++)
        {
            string atkName = "DefaultAttack/Attack" + (i + 1);
            Transform atkTf = transform.Find(atkName);
            if (atkTf != null)
            {
                Collider2D collider = atkTf.GetComponent<Collider2D>();
                if (collider != null)
                {
                    col[i] = collider;
                }
                else
                {
                    Debug.LogWarning($"{atkName} ������Ʈ�� Collider2D�� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogWarning($"{atkName} ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        for (int i = 0; i < airAtkComboMax; i++)
        {
            string atkName = "DefaultAttack/AirAttack" + (i + 1);
            Transform atkTf = transform.Find(atkName);
            if (atkTf != null)
            {
                Collider2D collider = atkTf.GetComponent<Collider2D>();
                if (collider != null)
                {
                    Aircol[i] = collider;
                }
                else
                {
                    Debug.LogWarning($"{atkName} ������Ʈ�� Collider2D�� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogWarning($"{atkName} ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }

    }
}