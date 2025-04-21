using System;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerBase : MonoBehaviour
{
    public enum PlayerChoice
    {
        None = 0,
        P1,
        P2,
    }
    [System.Serializable]
    public struct StateValue
    {
        public StateValue(float walk, float run, float air)
        {
            this.walk = walk;
            this.run = run;
            this.air = air;
        }
        public float walk; // 걷기
        public float run; // 뛰기
        public float air; // 공중
    }
    [System.Serializable]
    public struct StartEnd
    {
        public StartEnd(float start, float end)
        {
            this.start = start;
            this.end = end;
        }
        public float start;
        public float end;
    }

    [Header("DefaultSettings")]
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    [SerializeField] bool IsSelect = false; // 나중에 플레이어가 이미 선택하면 선택못하게 할 것
    [SerializeField] protected PlayerChoice currentChoice = PlayerChoice.None;

    [Header("PlayerState")]
    [SerializeField] protected bool IsMove; // 이동 여부
    [SerializeField] protected bool IsRun; // 달리기 여부
    [SerializeField] protected bool IsDash; // 대쉬 여부
    [SerializeField] protected bool IsChoice; // 선택 여부
    public bool IsAlive // 생존 여부
    {
        get
        {
            return anim.GetBool(AnimationStrings.isAlive);
        }
    }
    /*
         //캐릭터가 올바르게 이동하고 있는지 여부
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                //캐릭터 스프라이트 좌우 반전
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
     */
    [SerializeField] protected float CurrentMoveSpeed { // 지상/공중 상태에 따른 현재 이동속도
        get // 조건 받아 반환
        {
            if (IsMove && !td.IsOnWall) // 걷고있으면 && 벽에 충돌하지 않은 상태면
            {
                if (td.IsGrounded && IsRun) // 지상일 때
                {
                    if (IsRun)  // 달리기일 때
                    {
                        return moveSpeed.run; // 달리기 속도
                    }
                    else        //걷기일 때
                    {
                        return moveSpeed.walk; // 걷기 속도
                    }
                }
                else // 공중일 때
                {
                    return moveSpeed.air; // 공중 속도
                }
            }
            else // 걷지 않거나 벽에 충돌하면
            {
                return 0; // 이동속도 반환 안함
            }
        }
    }
    [Header("PlayerMove")]
    [SerializeField] protected StateValue moveSpeed = new StateValue(5f, 8f, 6f); // 걷기/ 뛰기 / 공중 이동속도
    [SerializeField] protected float attackSpeed = 3f;
    [SerializeField] protected float walkMaxAcceleration = 10f; //걷기 최대 가속값
    [SerializeField] protected float runMaxAcceleration = 10f; //뛰기 최대 가속값
    [SerializeField] protected float deceleration = 2f; // 감속값
    [SerializeField] protected float jumpImpulse = 8f; //점프하는 힘

    [SerializeField] protected bool dashOn = true; //대쉬 활성화 여부

    [SerializeField] public int selectedSkillIndex = 0; // 선택된 스킬 인덱스
    //public SkillData[] skillList;// 사용 가능한 스킬 목록
    //회피기
    [Header("PlayerSpeed")]
    [SerializeField] protected StateValue dashSpeed = new StateValue(7f, 7f, 7f);
    [SerializeField] protected float dashSpeedIdle = 7f; // 기본 및 서있기
    [SerializeField] protected StateValue dashDuration = new StateValue(0.1f, 0.1f, 0.1f);
    [SerializeField] protected float dashDurationIdle = 0.1f; // 기본 및 서있기
    [SerializeField] private float dashCooltime = 1f; // 회피 쿨타임

    [Header("PlayerDoubleTap")]
    [SerializeField] protected StartEnd doubleTapCurTime = new StartEnd(0,0);// 더블탭 감지 타이머
    [SerializeField] protected StartEnd doubleTapDetectTime = new StartEnd(0.3f,0.1f); // 더블탭 감지 활성 시간
    [SerializeField] protected int tapCount; // 더블탭 카운트

    [SerializeField] protected Vector2 moveInput; //입력 방향
    [SerializeField] protected TouchingDirection td; //땅이나 벽에 닿아있는 방향을 판단

    public bool getKeyIgnore = false; // 모든 입력 무시 여부

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

