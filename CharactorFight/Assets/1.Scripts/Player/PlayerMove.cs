using System;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static PlayerBase;
using static UnityEngine.RuleTile.TilingRuleOutput;
public class PlayerMove : MonoBehaviour
{
    [System.Serializable]
    public struct StateValue
    {
        public StateValue(float ground, float air)
        {
            this.ground = ground;
            this.air = air;
        }
        public float ground; // 걷기
        public float air; // 뛰기
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
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    //private TouchingDirection td; //땅이나 벽에 닿아있는 방향을 판단
    private PlayerBase player;
    
    [SerializeField] protected bool allWaysRun; // 기본값 달리기 여부


    [Header("PlayerMove")]
    [SerializeField] public StateValue moveSpeed = new StateValue(5f, 6f); // 걷기/ 뛰기 / 공중 이동속도
    [SerializeField] public float attackSpeed = 3f;
    [SerializeField] public StateValue maxAcceleration = new StateValue(10f, 10f); // 이동 최대 가속값
    [SerializeField] public float deceleration = 2f; // 감속값
    [SerializeField] public float jumpImpulse = 8f; //점프하는 힘
    [SerializeField] public int jumpCountMax = 2; //점프횟수 최대
    [SerializeField] public int curjumpCount = 0; //점프횟수

    [Header("PlayerDash")]
    [SerializeField] public bool dashOn = true; //대쉬 활성화 여부
    [SerializeField] public bool GhostEffect = false; // 대쉬 잔상 활성화 여부
    [SerializeField] public bool makeGhost = false; // 대쉬 잔상 활성화 여부
    [SerializeField] public StateValue dashSpeed = new StateValue(7f, 7f); // 대시 속도
    [SerializeField] public StateValue dashDuration = new StateValue(0.2f, 0.2f); // 대시 지속 시간
    [SerializeField] public float curDashTime = 0;
    [SerializeField] private float dashCooltime = 1f; // 회피 쿨타임


    //public SkillData[] skillList;// 사용 가능한 스킬 목록
    //회피기
    [Header("PlayerSpeed")]
    
    [SerializeField] public float dashSpeedIdle = 7f; // 기본 및 서있기
    [SerializeField] public float dashDurationIdle = 0.1f; // 기본 및 서있기


    [Header("PlayerDoubleTap")]
    [SerializeField] public KeyCode lastKey = KeyCode.None;
    [SerializeField] public StartEnd doubleTapCurTime = new StartEnd(0,0);// 더블탭 감지 타이머
    [SerializeField] public StartEnd doubleTapDetectTime = new StartEnd(0.3f,0.1f); // 더블탭 감지 활성 시간
    [SerializeField] public int tapCount; // 더블탭 카운트
    [SerializeField] public bool isDoubleTap = false; // 더블탭 여부

    [SerializeField] public Vector2 moveInput; //입력 방향


    
    float GetSpeed()
    {
        if (player.GetCurDirState() == DirState.Ground || player.GetCurDirState() == DirState.GroundWall) { return moveSpeed.ground; }
        else if (player.GetCurDirState() == DirState.Air || player.GetCurDirState() == DirState.AirWall || player.GetCurDirState() == DirState.AirCeiling) { return moveSpeed.air; }
        else { print("MoveSpeed 반환 에러"); return 0; }
    }
     float GetMaxAcceleration()
    {
        if (player.GetCurDirState(DirState.Ground) || player.GetCurDirState(DirState.GroundWall)) { return maxAcceleration.ground; }
        else if (player.GetCurDirState(DirState.Air) || player.GetCurDirState(DirState.AirWall) || player.GetCurDirState(DirState.AirCeiling)) { return maxAcceleration.air; }
        else { print("MoveSpeed 반환 에러"); return 0; }
    }

    private void Awake()
    {
        PlayerInitialize(); // 컴포넌트 연결
    }
    public void GetMoveKey()
    {
        DoubleTap();

        if (player.GetCurChoice() == PlayerBase.PlayerChoice.P1 && !player.getKeyIgnore) // Player 1
        {
            if (Input.GetKeyDown(KeyCode.W) && curjumpCount < jumpCountMax && !player.GetCurPlayerState(PlayerState.Dash)) // 점프
            {
                curjumpCount++;
                anim.SetTrigger(AnimationStrings.jumpTrgger);
                rb.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
            }
            
            if ((player.GetCurDirState(DirState.Ground) || player.GetCurDirState(DirState.GroundWall))&& curjumpCount >= jumpCountMax)
            {
                curjumpCount = 0;
            }

            moveInput.x = Input.GetAxisRaw("P1 Horizontal");

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (lastKey == KeyCode.A)
                {
                    tapCount++;
                }
                else
                {
                    tapCount = 1;
                    lastKey = KeyCode.A;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (lastKey == KeyCode.D)
                {
                    tapCount++;
                }
                else
                {
                    tapCount = 1;
                    lastKey = KeyCode.D;
                }
            }
        }
        else if (player.GetCurChoice() == PlayerBase.PlayerChoice.P2 && !player.getKeyIgnore) // Player 2
        {

            if (Input.GetKeyDown(KeyCode.UpArrow) && curjumpCount < jumpCountMax && !player.GetCurPlayerState(PlayerState.Dash)) // 점프
            {
                curjumpCount++;
                anim.SetTrigger(AnimationStrings.jumpTrgger);
                rb.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
            }

            if ((player.GetCurDirState(DirState.Ground) || player.GetCurDirState(DirState.GroundWall)) && curjumpCount >= jumpCountMax)
            {
                curjumpCount = 0;
            }
            moveInput.x = Input.GetAxisRaw("P2 Horizontal");
            // 더블탭 감지를 위한 키 입력 체크
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (lastKey == KeyCode.LeftArrow)
                {
                    tapCount++;
                }
                else
                {
                    tapCount = 1;
                    lastKey = KeyCode.LeftArrow;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (lastKey == KeyCode.RightArrow)
                {
                    tapCount++;
                }
                else
                {
                    tapCount = 1;
                    lastKey = KeyCode.RightArrow;
                }
            }
        }

        SetFacingDirection(); // 방향 플립
    }

    public void PlayerInitialize()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        //td = gameObject.GetComponent<TouchingDirection>();
        player = gameObject.GetComponent<PlayerBase>();
    }
    public void Deceleration()
    {
        if (moveInput.x == 0 && !player.GetCurPlayerState(PlayerState.Dash) && player.GetCurDirState(DirState.Ground) || player.GetCurDirState(DirState.GroundWall) && !player.getKeyIgnore)  // 감속
        {
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, deceleration * Time.fixedDeltaTime);
        }
    }
    public void OnMove()
    {
        if (moveInput.x != 0)
            rb.linearVelocity = new Vector2(moveInput.x * GetSpeed(), rb.linearVelocityY);

        if (player.GetCurPlayerState() == PlayerState.Move)
            ClampHorizontalSpeed(GetMaxAcceleration());
    }
    float GetDashSpeed()
    {
        if (player.GetCurDirSetGround()) { return dashSpeed.ground; } else return dashSpeed.air;
    }
    public float GetDashDuration()
    {
        if (player.GetCurDirSetGround()) { return dashDuration.ground; } else return dashDuration.air;
    }
    public bool OnDash()
    {
        Vector2 dashDir = moveInput;

        if (curDashTime < GetDashDuration())
        {
            if (moveInput == Vector2.zero)
                dashDir = transform.localScale.x < 0 ? Vector2.left : Vector2.right;

            rb.linearVelocity = dashDir.normalized * (GetDashSpeed() * 5f);
            curDashTime += Time.deltaTime;

            return false; // 아직 대시 진행 중
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            curDashTime = 0f;
            isDoubleTap = false;
            print("중단22");

            return true;  // 대시 종료
        }

        //Vector2 dashDir = moveInput;
        ////.ghost.makeGhost = true;
        //curDashTime += Time.deltaTime;

        //// 좌우 입력 기준 대시 방향 설정 (기본 오른쪽)
        //if (moveInput == Vector2.zero)
        //    dashDir = transform.localScale.x < 0 ? Vector2.left : Vector2.right;

        //// Time.deltaTime 제거 – velocity는 초당 속도
        //rb.linearVelocity = dashDir.normalized * (GetDashSpeed() * 5f);

        //if (curDashTime >= GetDashDuration())
        //{
        //    curDashTime = 0f;
        //    //ghost.makeGhost = false;
        //    isDoubleTap = false;
        //    // 대시 종료 시 velocity 초기화 or 기존 움직임으로 복귀 가능
        //    rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        //}
        /*  this.ghost.makeGhost = true;
          this.dashTime += Time.deltaTime;
          this.isDash = true;

          if (this.tmpDir == Vector2.zero) this.tmpDir = Vector2.right;
          this.rBody2d.velocity = this.tmpDir.normalized * (this.playerMoveSpeed * 5) * Time.deltaTime;
          if (this.dashTime >= this.maxaDashTime)
          {
              this.dashTime = 0;
              this.isDash = false;
              this.ghost.makeGhost = false;
          }*/
    }
        public void Ghost()
        {

        }
    // 방향전환
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    public void SetFacingDirection()
    {
        if (moveInput.x > 0 && transform.localScale.x < 0)
            Flip();
        // 왼쪽으로 가는 중이고, 현재 오른쪽을 보고 있으면 Flip() 호출
        else if (moveInput.x < 0 && transform.localScale.x > 0)
            Flip();
    }
    public void ClampHorizontalSpeed(float runMaxAcceleration)
    {
        if (rb.linearVelocityX > runMaxAcceleration && moveInput.x == 1 && !player.GetCurPlayerState(PlayerState.Dash))
            rb.linearVelocityX = runMaxAcceleration;
        if (rb.linearVelocityX < -runMaxAcceleration && moveInput.x == -1 && !player.GetCurPlayerState(PlayerState.Dash))
            rb.linearVelocityX = -runMaxAcceleration;
    }
    public void DoubleTap() // 더블탭 감지
    {
        if (tapCount > 0 && doubleTapCurTime.start <= doubleTapDetectTime.start)
        {
            doubleTapCurTime.start += Time.deltaTime;
            if (doubleTapCurTime.start >= doubleTapDetectTime.start)
            {
                tapCount = 0;
                doubleTapCurTime.start = 0;
            }
            else if (doubleTapCurTime.start < doubleTapDetectTime.start && tapCount == 2)
            {
                isDoubleTap = true;
            }
        }
        if (isDoubleTap && moveInput.x == 0)
        {
            doubleTapCurTime.end += Time.deltaTime;
            if (doubleTapCurTime.end >= doubleTapDetectTime.end)
            {
                doubleTapCurTime.start = 0;
                doubleTapCurTime.end = 0;
                
                tapCount = 0;
                isDoubleTap = false;
            }
        }
    }
}

