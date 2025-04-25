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
    [SerializeField] protected int curHelth; // ����ü��
    [SerializeField] protected int maxHelth; // �ִ�ü��

    [Header("PlayerDefaultAttack")]
    List<Attack> attackStack = new List<Attack>();

    [Header("PlayerSkill")]
    [SerializeField] public int selectedSkillIndex = 0; // ���õ� ��ų �ε���

    [SerializeField] protected bool IsAlive = true; // ���� ����
    [Header("DefaultSettings")]
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected CapsuleCollider2D col;
    protected Animator anim;
    protected PlayerMove pMove;
    protected PlayerMana pMana;

    [Header("Choice")]
    [SerializeField] protected PlayerChoice curChoice = PlayerChoice.None;
    [SerializeField] protected bool IsChoice; // ���� ����

    [Header("ChoiceCharactor")]
    [SerializeField] PlayerCharactor charactor = default;

    [Header("PlayerState")]
    [SerializeField] protected PlayerState curPlayerState = PlayerState.Idle;               // ���� ����
    [SerializeField] float state_time;    //���� �ð�
    [SerializeField] public bool getKeyIgnore = false; // ��� �Է� ���� ����

    [Header("DirState")]
    [SerializeField] protected DirState curDirState = DirState.Ground;
    [SerializeField] protected float groundDistance = 0.05f; // �ٴ����� �����ϴ� �ִ� �Ÿ�
    [SerializeField] protected float wallDistance = 0.6f; //������ �����ϴ� �ִ� �Ÿ�
    [SerializeField] protected float ceilingDistance = 0.05f; // õ������ �����ϴ� �ִ� �Ÿ�
    [SerializeField] protected ContactFilter2D castFilter; //����ĳ��Ʈ �浹 ���� ���� ����
    [SerializeField] protected float groundActiveDis = 5f; // �ٴ� Ȱ��ȭ �Ÿ�
    [SerializeField] protected float groundRayWidth = 5f; // �ٴڰ��� ���� ����
    [SerializeField] protected float groundRayStartPosition = 5f; // �ٴڰ��� ���� ����
    //�� �������� ����ĳ��Ʈ ����� ��� �迭
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
        PlayerStateStart(PlayerState.Idle);           // ���� ����        
        DirStateStart(DirState.Ground);
        state_time = Time.time;
    }
    private void Awake()
    {
        PlayerInitialize(); // ������Ʈ ����
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
        PlayerStateUpdate();                     // ���� ���μ���
        DirStateUpdate();
        pMove.GetMoveKey();                        // �̵� Ű �Է�
        GroundActive();
    }
    protected void DefaultFixedUpadateSetting()
    {
        pMove.Deceleration();
    }

    public void PlayerStateStart(PlayerState state)       //���� ���� �ϴ� �Լ�
    {
        curPlayerState = state;                      //���� ���� ���� 
        state_time = Time.time + 1.0f;          //���� �ð�


        switch (curPlayerState)
        {
            case PlayerState.Idle:
                anim.SetBool(AnimationStrings.isMoving, false);
                anim.SetBool(AnimationStrings.isDashing, false);
                //pMove.Deceleration(); // ����
                break;
            case PlayerState.Move:
                anim.SetBool(AnimationStrings.isMoving, true);
                anim.SetBool(AnimationStrings.isDashing, false);
                //pMove.Deceleration(); // ����
                break;
            case PlayerState.Dash:
                anim.SetBool(AnimationStrings.isDashing, true);
                state_time = Time.time + pMove.GetDashDuration();
                break;
            case PlayerState.Attack:
                //���ۺ��� // movespeed = 0;
                break;
            case PlayerState.AirAttack:
                //���ۺ��� // movespeed = 0;
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
                //���ۺ��� // movespeed = 0;
                break;
            default:
                break;
        }
    }
    public void DirStateStart(DirState state)       //���� ���� �ϴ� �Լ�
    {
        curDirState = state;                      //���� ���� ���� 
        state_time = Time.time + 1.0f;          //���� �ð�

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
        // Ground ���´� ���·� ���еǴϱ� ���⼭�� ó��
        anim.SetBool(AnimationStrings.isGrounded,
            curDirState == DirState.Ground || curDirState == DirState.GroundWall);

        // Wall: ������ ���� ��� �ִ°�?
        bool touchingWall = col.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        anim.SetBool(AnimationStrings.isOnwall, touchingWall);

        // Ceiling: ������ õ�忡 ��� �ִ°�?
        bool touchingCeiling = col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
        anim.SetBool(AnimationStrings.isOnCeiling, touchingCeiling);
    }
    private void GroundActive()
    {
        
        // ������ ���� ��ġ ����
        Vector2 rayStartPosition = new Vector2(transform.position.x - groundRayStartPosition, transform.position.y - groundActiveDis);
        // ���������� `groundActiveDis`��ŭ ���̸� ���� ����ĳ��Ʈ �߻�
        RaycastHit2D[] groundHits = Physics2D.RaycastAll(rayStartPosition, Vector2.right, groundRayWidth, LayerMask.GetMask("Ground"));

        // ���� ��θ� �� �信�� �׸���
        Debug.DrawRay(rayStartPosition, Vector2.right * groundRayWidth, Color.red);

        foreach (RaycastHit2D hit in groundHits)
        {
            // ���÷� "Ground" �±׸� ���� ������Ʈ�� �迭�� �߰�
            if (hit.collider.CompareTag("Ground"))
            {
                // �̹� �迭�� ���ԵǾ� �ִ��� Ȯ���ϰ� �߰�
                if (!hitGroundObjs.Contains(hit.collider.gameObject))
                {
                    hitGroundObjs.Add(hit.collider.gameObject);
                    hit.collider.gameObject.GetComponent<Platform>().ColOn(GetCurChoice());
                    Debug.Log("����ĳ��Ʈ�� 'Ground' ������Ʈ�� ��ҽ��ϴ�: " + hit.collider.gameObject.name);
                }
            }
        }
        for (int i = hitGroundObjs.Count - 1; i >= 0; i--)
        {
            // �迭�� ������Ʈ�� �� �̻� �浹���� ������ �迭���� ����
            if (!hitGroundObjs[i].GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Debug.Log("����ĳ��Ʈ�� 'Ground' ������Ʈ���� ������ϴ�: " + hitGroundObjs[i].name);
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
