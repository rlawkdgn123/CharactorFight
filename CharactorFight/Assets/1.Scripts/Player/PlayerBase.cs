using NUnit.Framework.Internal;
using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMove))]
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
    [Header("PlayerStats")]
    [SerializeField] private int curHelth; // 현재체력
    [SerializeField] private int maxHelth; // 최대체력
    [SerializeField] protected bool IsAlive = true; // 선택 여부

    [Header("DefaultSettings")]
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected CapsuleCollider2D col;
    protected Animator anim;
    protected PlayerMove pm;

    [Header("Choice")]
    [SerializeField] protected PlayerChoice currentChoice = PlayerChoice.None;
    [SerializeField] protected bool IsChoice; // 선택 여부

    [Header("Attack")]
    [SerializeField] protected int atkStack = 0;
    [SerializeField] bool IsAttack; // 선택 여부

    [Header("ChoiceCharactor")]
    [SerializeField] PlayerCharactor charactor = default;

    [Header("PlayerState")]
    [SerializeField] protected PlayerState curPlayerState = PlayerState.Idle;               // 현재 상태
    [SerializeField] float state_time;    //상태 시간
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

    public PlayerState GetCurPlayerState() { return curPlayerState;  }
    public bool GetCurPlayerState(PlayerState state) { if (curPlayerState == state) { return true; } else { return false; } }
    
    public DirState GetCurDirState() { return curDirState; }
    public bool GetCurDirState(DirState state) { if (curDirState == state) { return true; } else { return false; } }
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
        pm.GetMoveKey();                        // 이동 키 입력
    }
    protected void DefaultFixedUpadateSetting()
    {
        pm.Deceleration();
    }

    public void PlayerStateStart(PlayerState state)       //상태 변경 하는 함수
    {
        curPlayerState = state;                      //현재 상태 변경 
        state_time = Time.time + 1.0f;          //상태 시간


        switch (curPlayerState)
        {
            case PlayerState.Idle:
                anim.SetBool(AnimationStrings.isMoving, false);
                //pm.Deceleration(); // 감속
                break;
            case PlayerState.Move:
                anim.SetBool(AnimationStrings.isMoving, true);
                //pm.Deceleration(); // 감속
                break;
            case PlayerState.Dash:
                state_time = Time.time + 0.3f;
                break;
            case PlayerState.Attack:
                //동작변경 // movespeed = 0;
                break;
            case PlayerState.AirAttack:
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
        //pm.GetMoveKey();

        switch (curPlayerState)
        {
            case PlayerState.Idle:
                
                if (IsAlive && pm.moveInput != Vector2.zero && !pm.isDoubleTap) { PlayerStateStart(PlayerState.Move); }
                else if (IsAlive && pm.isDoubleTap) { PlayerStateStart(PlayerState.Dash); }
                break;
            case PlayerState.Move:
                pm.OnMove();
                if (IsAlive && pm.moveInput == Vector2.zero && !pm.isDoubleTap) { PlayerStateStart(PlayerState.Idle); }
                else if (IsAlive && pm.isDoubleTap) { PlayerStateStart(PlayerState.Dash); }
                break;
            case PlayerState.Dash:
                rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
                if (Time.time > state_time) { PlayerStateStart(PlayerState.Idle); break; }
                break;
            case PlayerState.Attack:
                //동작변경 // movespeed = 0;
                break;
            default:
                break;
        }
    }
    public void DirStateStart(DirState state)       //상태 변경 하는 함수
    {
        curDirState = state;                      //현재 상태 변경 
        state_time = Time.time + 1.0f;          //상태 시간

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
                if (!(col.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0))
                    DirStateStart(DirState.Air);
                else if (col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0)
                    DirStateStart(DirState.AirCeiling);
                else if (col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0)
                    DirStateStart(DirState.GroundWall); // AirWall -> GroundWall
                break;

            case DirState.AirCeiling:
                if (!(col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0))
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
    public PlayerChoice GetCurrentChoice() { return currentChoice; }
    public bool GetIsChoice() { return IsChoice; }


    protected void PlayerInitialize()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<CapsuleCollider2D>();
        pm = gameObject.GetComponent<PlayerMove>();
        //td = gameObject.GetComponent<TouchingDirection>();
        if (col == null) Debug.LogError("CapsuleCollider2D(col)이 null입니다!");
        if (rb == null) Debug.LogError("Rigidbody2D(rb)가 null입니다!");
        if (pm == null) Debug.LogError("PlayerMove(pm)가 null입니다!");
        if (anim == null) Debug.LogError("Animator(anim)가 null입니다!");
    }
}
