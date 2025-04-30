using UnityEngine;
using static PlayerBase;

public class PlayerAttack : MonoBehaviour
{
    [Header("AttackCollider")]
    [SerializeField] public Collider2D col;
    [SerializeField] public Collider2D Aircol;

    [Header("DefaultSettings")]
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerBase player;

    [Header("AnimationInfo")]
    [SerializeField] public int atkComboMax = 3;
    [SerializeField] public int curAtkCombo = 0;
    [SerializeField] public float[] atkDuration = { 1, 1, 1 };
    [SerializeField] public float lastAttackTime = 0f;
    [SerializeField] public float comboResetTime = 1.0f; // 콤보 입력 제한 시간
    [SerializeField] public bool canAirAttack = true;
    [SerializeField] public float[] atkEndTime = { 0.75f * 1.2f, 0.75f * 1, 0.75f * 0.9f };
    [SerializeField] public float airAtkEndTime = 0.9f;
    private void Awake()
    {
        PlayerInitialize();
    }
    private void Start()
    {
        if (col && player.GetCurChoice(PlayerBase.PlayerChoice.P1))
        {
            col.gameObject.layer = LayerMask.NameToLayer("P1Attack");  // P1Attack 레이어 할당
        }
        else if (col && player.GetCurChoice(PlayerBase.PlayerChoice.P2))
        {
            col.gameObject.layer = LayerMask.NameToLayer("P2Attack");  // P2Attack 레이어 할당
        }

        if (Aircol && player.GetCurChoice(PlayerBase.PlayerChoice.P1))
        {
            Aircol.gameObject.layer = LayerMask.NameToLayer("P1Attack");  // P1AirAttack 레이어 할당
        }
        else if (Aircol && player.GetCurChoice(PlayerBase.PlayerChoice.P2))
        {
            Aircol.gameObject.layer = LayerMask.NameToLayer("P2Attack");  // P2AirAttack 레이어 할당
        }
    }
    public void Attack()

    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            curAtkCombo = 0; // 일정 시간 지나면 콤보 초기화
        }
    }
    public void AttackInput()
    {
        curAtkCombo++;

        if (curAtkCombo > atkComboMax)

            curAtkCombo = 1; // 3콤보 이후에는 다시 1콤보로 이어지도록 (선택 사항)

        //anim.SetInteger("Player_attack_", curAtkCombo);

        anim.SetTrigger("attackTrigger");

        lastAttackTime = Time.time;

    }
    public void AirAttackInput()
    {
/*        curAtkCombo++;

        if (curAtkCombo > atkComboMax)

            curAtkCombo = 1; // 3콤보 이후에는 다시 1콤보로 이어지도록 (선택 사항)*/

        //anim.SetInteger("Player_attack_", curAtkCombo);

        anim.SetTrigger(AnimationStrings.airAttackTrigger);

      /*  lastAttackTime = Time.time;*/

    }
    // 애니메이션 이벤트로 콤보 초기화 함수 연결 가능

    public void ResetCombo()

    {

        curAtkCombo = 0;

        anim.SetInteger("comboStep", curAtkCombo);

    }
    bool InputTrue() { return true; }
    //if (IsAlive && !pMove.isDoubleTap)
    //{
    //    if(Time.time >= state_time) { print("시간끝"); }
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
    //float comboInputWindow = 0.3f; // 콤보 입력 가능 시간
    //if (IsAlive && curAtkCombo <= atkComboMax &&
    //    (state_time - Time.time <= comboInputWindow) &&
    //    ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.V))
    //    || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Comma))))
    //{
    //    PlayerStateStart(PlayerState.Attack);
    //}
    //동작변경 // movespeed = 0;
    public void PlayerInitialize()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponent<PlayerBase>();

        col = transform.Find("Attack").GetComponent<Collider2D>();
        Aircol = transform.Find("AirAttack").GetComponent<Collider2D>();
    }
}