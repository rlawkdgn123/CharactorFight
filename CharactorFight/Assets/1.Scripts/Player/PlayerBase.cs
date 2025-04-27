using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMove))] 
[RequireComponent(typeof(PlayerMana))]
public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    public enum PlayerChoice
    {
        None = 0,
        P1,
        P2,
    }
    enum PlayerCharactor
    {
        Default,
        Zoro,
    }
    [SerializeField]
    public enum PlayerState
    {
        Idle,
        Move,
        Dash,
        Attack,
        AirAttack,
        Skill1,
        Skill2,
        Skill3,
        Hit,
        Death,
    }
    [SerializeField]
    public enum DirState
    {
        Ground,
        GroundWall,
        GroundCelling,
        Air,
        AirWall,
        AirCeiling,
    }
    public struct DefaultAtk
    {
        
    }
    [Header("PlayerStats")]
    [SerializeField] protected int curHelth; // 현재체력
    [SerializeField] protected int maxHelth; // 최대체력

    [Header("PlayerDefaultAttack")]
    List<Attack> attackStack = new List<Attack>();

    [Header("PlayerSkill")]
    [SerializeField] public int selectedSkillIndex = 0; // 선택된 스킬 인덱스

    [SerializeField] protected bool IsAlive = true; // 선택 여부
    [Header("DefaultSettings")]
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected CapsuleCollider2D col;
    protected Animator anim;
    protected PlayerMove pMove;
    protected PlayerMana pMana;
    protected PlayerAttack pAtk;
    protected GameObject psObj;
    protected PlatformSensor ps;

    [Header("Choice")]
    [SerializeField] protected PlayerChoice curChoice = PlayerChoice.None;
    [SerializeField] protected bool IsChoice; // 선택 여부

    [Header("ChoiceCharactor")]
    [SerializeField] PlayerCharactor charactor = default;

    [Header("PlayerState")]
    [SerializeField] protected PlayerState curPlayerState = PlayerState.Idle;               // 현재 상태
    [SerializeField] float state_time;    //상태 시간
    [SerializeField] float dirState_time;    //상태 시간
    [SerializeField] public bool getKeyIgnore = false; // 모든 입력 무시 여부

    [Header("DirState")]
    [SerializeField] protected DirState curDirState = DirState.Ground;
    [SerializeField] protected float groundDistance = 0.05f; // 바닥으로 판정하는 최대 거리
    [SerializeField] protected float wallDistance = 0.6f; //벽으로 판정하는 최대 거리
    [SerializeField] protected float ceilingDistance = 0.05f; // 천장으로 판정하는 최대 거리
    [SerializeField] protected ContactFilter2D castFilter; //레이캐스트 충돌 판정 필터 조건
    //각 방향으로 레이캐스트 결과를 담는 배열
    protected RaycastHit2D[] groundHits = new RaycastHit2D[5];
    protected RaycastHit2D[] wallHits = new RaycastHit2D[5];
    protected RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [Header("AnimationInfo")]
    [SerializeField] protected float dashEndTime;
    [SerializeField] protected float[] atkEndTime = { 0.75f * 1.2f, 0.75f * 1, 0.75f * 0.9f };
    





    void Start()
    {
        PlayerStateStart(PlayerState.Idle);           // 상태 변경        
        DirStateStart(DirState.Ground);
        state_time = Time.time;
    }
    private void Awake()
    {
        PlayerInitialize(); // 컴포넌트 연결
    }
    void Update()
    {
        DefaultUpadateSetting();
    }
    private void FixedUpdate()
    {
        DefaultFixedUpadateSetting();
    }

    protected void DefaultUpadateSetting()
    {
        PlayerStateUpdate();                     // 상태 프로세서
        DirStateUpdate();
        pMove.GetMoveKey();                        // 이동 키 입력
        //GroundActive();
    }

    protected void DefaultFixedUpadateSetting()
    {
        pMove.Deceleration();
    }

    public void PlayerStateStart(PlayerState state)       //상태 변경 하는 함수
    {
        curPlayerState = state;                      //현재 상태 변경 
        state_time = Time.time + 1.0f;          //상태 시간


        switch (curPlayerState)
        {
            case PlayerState.Idle:
                anim.SetBool(AnimationStrings.isMoving, false);
                anim.SetBool(AnimationStrings.isDashing, false);
                anim.SetBool(AnimationStrings.isAttack, false);
                //pMove.Deceleration(); // 감속
                break;
            case PlayerState.Move:
                anim.SetBool(AnimationStrings.isMoving, true);
                anim.SetBool(AnimationStrings.isDashing, false);
                anim.SetBool(AnimationStrings.isAttack, false);
                //pMove.Deceleration(); // 감속
                break;
            case PlayerState.Dash:
                anim.SetBool(AnimationStrings.isDashing, true);
                state_time = Time.time + pMove.GetDashDuration();
                break;
            case PlayerState.Attack:
                getKeyIgnore = true;
                pAtk.Attack();
                pAtk.AttackInput();
                pAtk.col.enabled = true;
                anim.SetBool(AnimationStrings.isAttack, true);
                state_time = Time.time + pAtk.atkDuration[pAtk.curAtkCombo - 1];
                anim.SetTrigger(AnimationStrings.attackTrigger);

                /*                anim.SetInteger(AnimationStrings.AttackCombo, ++pAtk.curAtkCombo);
                                anim.SetTrigger(AnimationStrings.attackTrigger);*/
                //state_time = Time.time + atkEndTime[atkCombo];

                //동작변경 // movespeed = 0;
                break;
            case PlayerState.AirAttack:
                anim.SetBool(AnimationStrings.isAttack, true);
                anim.SetTrigger(AnimationStrings.airAttackTrigger);
                //동작변경 // movespeed = 0;
                break;
            case PlayerState.Skill1:
                break;
            case PlayerState.Skill2:
                break;
            case PlayerState.Skill3:
                break;
            case PlayerState.Hit:
                state_time = Time.time + 0.3f;
                break;
            case PlayerState.Death:
                state_time = Time.time + 1.0f;
                break;
            default:
                break;
        }
    }
    /*       Idle,
       Walk,
       Run,
       Attack,
       AirAttack,
       Skill1,
       Skill2,
       Skill3,
       Hit,
       Death,*/
    public void PlayerStateUpdate()
    {
        //pMove.GetMoveKey();

        switch (curPlayerState)
        {
            case PlayerState.Idle:
                
                if (IsAlive && pMove.moveInput != Vector2.zero && !pMove.isDoubleTap &&!getKeyIgnore) { PlayerStateStart(PlayerState.Move); }
                else if (IsAlive && pMove.isDoubleTap) { PlayerStateStart(PlayerState.Dash); }
                else if (IsAlive && !getKeyIgnore && GetCurDirSetGround() && ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.V)) || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Comma)))) { PlayerStateStart(PlayerState.Attack);}
                else if (IsAlive && !getKeyIgnore && GetCurDirSetAir() && ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.V)) || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Comma)))) { PlayerStateStart(PlayerState.AirAttack); }
                //else if (IsAlive && !getKeyIgnore && ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.B)) || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Period)))) { PlayerStateStart(PlayerState.Skill1);}
                break;
            case PlayerState.Move:
                pMove.OnMove();
                if (IsAlive && pMove.moveInput == Vector2.zero && !pMove.isDoubleTap && !getKeyIgnore) { PlayerStateStart(PlayerState.Idle); }
                else if (IsAlive && pMove.isDoubleTap && !getKeyIgnore) { PlayerStateStart(PlayerState.Dash); }
                else if (IsAlive && !getKeyIgnore && GetCurDirSetGround() && ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.V)) || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Comma)))) { PlayerStateStart(PlayerState.Attack); }
                else if (IsAlive && !getKeyIgnore && GetCurDirSetAir() && ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.V)) || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Comma)))) { PlayerStateStart(PlayerState.AirAttack); }
                //else if (IsAlive && !getKeyIgnore && ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.B)) || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Period)))) { PlayerStateStart(PlayerState.Skill1); }
                break;
            case PlayerState.Dash:
                if (pMove.OnDash())
                {
                    if (IsAlive && Time.time >= state_time - dashEndTime) { anim.SetBool(AnimationStrings.isDashing, false); print(state_time - dashEndTime); }
                    if (IsAlive && Time.time > state_time && !pMove.isDoubleTap && pMove.moveInput == Vector2.zero) { PlayerStateStart(PlayerState.Idle); break; }
                    if (IsAlive && Time.time > state_time && !pMove.isDoubleTap && pMove.moveInput != Vector2.zero) { PlayerStateStart(PlayerState.Move); break; }
                }
                break;
            case PlayerState.Attack:

                if (Time.time >= state_time || pAtk.curAtkCombo > pAtk.atkComboMax)
                {
                    getKeyIgnore = false;
                    if (IsAlive && !pMove.isDoubleTap && pMove.moveInput == Vector2.zero)
                    { PlayerStateStart(PlayerState.Idle); pAtk.col.enabled = false; }
                    else if (IsAlive && !pMove.isDoubleTap && pMove.moveInput == Vector2.zero)
                    { PlayerStateStart(PlayerState.Move); pAtk.col.enabled = false; }
                }
                else
                {
                    if (IsAlive && pAtk.curAtkCombo <= pAtk.atkComboMax && ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.V))
                                || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Comma)))) { PlayerStateStart(PlayerState.Attack); }
                }
                /*                
                                if (IsAlive && !pMove.isDoubleTap)
                                {
                                    if (Time.time >= state_time && pMove.moveInput == Vector2.zero || pAtk.curAtkCombo >= pAtk.atkComboMax && pMove.moveInput == Vector2.zero) { PlayerStateStart(PlayerState.Idle); break; }
                                    if (Time.time >= state_time && pMove.moveInput != Vector2.zero || pAtk.curAtkCombo >= pAtk.atkComboMax && pMove.moveInput != Vector2.zero) { PlayerStateStart(PlayerState.Move); break; }
                                }
                                if (IsAlive && pAtk.curAtkCombo <= pAtk.atkComboMax && Time.time <= state_time && ((PlayerChoice.P1 == curChoice && Input.GetKeyDown(KeyCode.V))
                                || (PlayerChoice.P2 == curChoice && Input.GetKeyDown(KeyCode.Comma)))) { PlayerStateStart(PlayerState.Attack); }*/

                break;
            default:
                break;
        }
    }
    public void DirStateStart(DirState state)       //상태 변경 하는 함수
    {
        curDirState = state;                      //현재 상태 변경 
        dirState_time = Time.time + 1.0f;          //상태 시간

        switch (curDirState)
        {
            case DirState.Ground:
                break;
            case DirState.GroundWall:
                break;
            case DirState.Air:
                break;
            case DirState.AirWall:
                break;
            case DirState.AirCeiling:
                break;
            default:
                break;
        }
    }

    private Vector2 wallCheckDirection =>
    gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    private void DirStateUpdate()
    {
        UpdateAnimationBools();
        switch (curDirState)
        {
            case DirState.Ground:
                if (col.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0)
                    DirStateStart(DirState.GroundWall);
                else if (!(col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0))
                    DirStateStart(DirState.Air);
                break;

            case DirState.GroundWall:
                if (!(col.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0))
                    DirStateStart(DirState.Ground);
                else if (!(col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0))
                    DirStateStart(DirState.AirWall);
                break;

            case DirState.Air:
                if (col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0)
                    DirStateStart(DirState.Ground);
                else if (col.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0)
                    DirStateStart(DirState.AirWall);
                else if (col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0)
                    DirStateStart(DirState.AirCeiling);
                break;

            case DirState.AirWall:
                if (col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0)
                    DirStateStart(DirState.Ground);
                else if (!(col.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0))
                    DirStateStart(DirState.Air);
                else if (col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0)
                    DirStateStart(DirState.AirCeiling);
                else if (col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0)
                    DirStateStart(DirState.GroundWall); // AirWall -> GroundWall
                break;

            case DirState.AirCeiling:
                if (col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0)
                    DirStateStart(DirState.Ground);
                else if (!(col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0))
                    DirStateStart(DirState.Air);
                else if (col.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0)
                    DirStateStart(DirState.AirWall);
                break;
        }
    }
    private void UpdateAnimationBools()
    {
        // Ground 상태는 상태로 구분되니까 여기서만 처리
        anim.SetBool(AnimationStrings.isGrounded,
            curDirState == DirState.Ground || curDirState == DirState.GroundWall);

        // Wall: 실제로 벽에 닿아 있는가?
        bool touchingWall = col.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        anim.SetBool(AnimationStrings.isOnwall, touchingWall);

        // Ceiling: 실제로 천장에 닿아 있는가?
        bool touchingCeiling = col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
        anim.SetBool(AnimationStrings.isOnCeiling, touchingCeiling);
    }
    
    /*    IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
            IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
            IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;*/
    public int GetCurHelth() { return curHelth; }
    public int GetMaxHelth() { return maxHelth; }
    public bool GetIsAlive() { return IsAlive; }
    public PlayerChoice GetCurChoice() { return curChoice; }
    public bool GetCurChoice(PlayerChoice choice) { if (curChoice == choice) { return true; } else { return false; } }
    public bool GetIsChoice() { return IsChoice; }
    public PlayerState GetCurPlayerState() { return curPlayerState; }
    public bool GetCurPlayerState(PlayerState state) { if (curPlayerState == state) { return true; } else { return false; } }
    public DirState GetCurDirState() { return curDirState; }
    public bool GetCurDirState(DirState state) { if (curDirState == state) { return true; } else { return false; } }
    public bool GetCurDirSetGround() { return GetCurDirState(DirState.Ground) || GetCurDirState(DirState.GroundWall); }
    public bool GetCurDirSetAir() { return GetCurDirState(DirState.Air) || GetCurDirState(DirState.AirWall) || GetCurDirState(DirState.AirCeiling); }
    protected void PlayerInitialize()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<CapsuleCollider2D>();
        pMove = gameObject.GetComponent<PlayerMove>();
        pMana = gameObject.GetComponent<PlayerMana>();
        pAtk = gameObject.GetComponent<PlayerAttack>();

        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "PlatformSensor":
                    ps = child.gameObject.GetOrAddComponent<PlatformSensor>();
                    Debug.Log("PlatformSensor에 추가되었습니다.");
                    break;
                default:
                    // "PlatformSensor"가 아니면 아무 작업도 하지 않음
                    break;
            }
        }
            


        switch (curChoice)
        {
            case PlayerChoice.P1:
                gameObject.layer = LayerMask.NameToLayer("P1");
                break;
            case PlayerChoice.P2:
                gameObject.layer = LayerMask.NameToLayer("P2");
                break;
            default:
                gameObject.layer = LayerMask.NameToLayer("Default");
                break;
        }
    }
}
