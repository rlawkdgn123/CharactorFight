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

    [Header("Choice")]
    [SerializeField] protected PlayerChoice curChoice = PlayerChoice.None;
    [SerializeField] protected bool IsChoice; // 선택 여부

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
    [SerializeField] protected float groundActiveDis = 5f; // 바닥 활성화 거리
    [SerializeField] protected float groundRayWidth = 5f; // 바닥감지 레이 길이
    [SerializeField] protected float groundRayStartPosition = 5f; // 바닥감지 레이 길이
    //각 방향으로 레이캐스트 결과를 담는 배열
    protected RaycastHit2D[] groundHits = new RaycastHit2D[5];
    protected RaycastHit2D[] wallHits = new RaycastHit2D[5];
    protected RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    private List<GameObject> hitGroundObjs = new List<GameObject>();

    public PlayerState GetCurPlayerState() { return curPlayerState;  }
    public bool GetCurPlayerState(PlayerState state) { if (curPlayerState == state) { return true; } else { return false; } }
    
    public DirState GetCurDirState() { return curDirState; }
    public bool GetCurDirState(DirState state) { if (curDirState == state) { return true; } else { return false; } }
    public bool GetCurDirSetGround() { return GetCurDirState(DirState.Ground) || GetCurDirState(DirState.GroundWall); }
    public bool GetCurDirSetAir() { return GetCurDirState(DirState.Air) || GetCurDirState(DirState.AirWall) || GetCurDirState(DirState.AirCeiling); }
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
        GroundActive();
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
                //pMove.Deceleration(); // 감속
                break;
            case PlayerState.Move:
                anim.SetBool(AnimationStrings.isMoving, true);
                anim.SetBool(AnimationStrings.isDashing, false);
                //pMove.Deceleration(); // 감속
                break;
            case PlayerState.Dash:
                anim.SetBool(AnimationStrings.isDashing, true);
                state_time = Time.time + pMove.GetDashDuration();
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
        //pMove.GetMoveKey();

        switch (curPlayerState)
        {
            case PlayerState.Idle:
                
                if (IsAlive && pMove.moveInput != Vector2.zero && !pMove.isDoubleTap) { PlayerStateStart(PlayerState.Move); }
                else if (IsAlive && pMove.isDoubleTap) { PlayerStateStart(PlayerState.Dash); }
                break;
            case PlayerState.Move:
                pMove.OnMove();
                if (IsAlive && pMove.moveInput == Vector2.zero && !pMove.isDoubleTap) { PlayerStateStart(PlayerState.Idle); }
                else if (IsAlive && pMove.isDoubleTap) { PlayerStateStart(PlayerState.Dash); }
                break;
            case PlayerState.Dash:
                pMove.OnDash();
                if (Time.time >= state_time/2) anim.SetBool(AnimationStrings.isDashing, false);
                if (Time.time > state_time && !pMove.isDoubleTap && pMove.moveInput == Vector2.zero) { PlayerStateStart(PlayerState.Idle); break; }
                if (Time.time > state_time && !pMove.isDoubleTap && pMove.moveInput != Vector2.zero) { PlayerStateStart(PlayerState.Move); break; }
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
    private void GroundActive()
    {
        
        // 레이의 시작 위치 설정
        Vector2 rayStartPosition = new Vector2(transform.position.x - groundRayStartPosition, transform.position.y - groundActiveDis);
        // 오른쪽으로 `groundActiveDis`만큼 길이를 가진 레이캐스트 발사
        RaycastHit2D[] groundHits = Physics2D.RaycastAll(rayStartPosition, Vector2.right, groundRayWidth, LayerMask.GetMask("Ground"));

        // 레이 경로를 씬 뷰에서 그리기
        Debug.DrawRay(rayStartPosition, Vector2.right * groundRayWidth, Color.red);

        foreach (RaycastHit2D hit in groundHits)
        {
            // 예시로 "Ground" 태그를 가진 오브젝트만 배열에 추가
            if (hit.collider.CompareTag("Ground"))
            {
                // 이미 배열에 포함되어 있는지 확인하고 추가
                if (!hitGroundObjs.Contains(hit.collider.gameObject))
                {
                    hitGroundObjs.Add(hit.collider.gameObject);
                    hit.collider.gameObject.GetComponent<Platform>().ColOn(GetCurChoice());
                    Debug.Log("레이캐스트가 'Ground' 오브젝트에 닿았습니다: " + hit.collider.gameObject.name);
                }
            }
        }
        for (int i = hitGroundObjs.Count - 1; i >= 0; i--)
        {
            // 배열의 오브젝트가 더 이상 충돌하지 않으면 배열에서 제거
            if (!hitGroundObjs[i].GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Debug.Log("레이캐스트가 'Ground' 오브젝트에서 벗어났습니다: " + hitGroundObjs[i].name);
                hitGroundObjs[i].GetComponent<Platform>().ColOff(GetCurChoice());
                hitGroundObjs.RemoveAt(i);
            }
        }
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


    protected void PlayerInitialize()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<CapsuleCollider2D>();
        pMove = gameObject.GetComponent<PlayerMove>();
        pMana = gameObject.GetComponent<PlayerMana>();
    }
}
