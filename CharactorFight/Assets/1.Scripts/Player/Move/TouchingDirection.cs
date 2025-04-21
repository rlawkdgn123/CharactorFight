using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{

    public ContactFilter2D castFilter; //레이캐스트 충돌 판정 필터 조건
    public float groundDistance = 0.05f; // 바닥으로 판정하는 최대 거리
    public float wallDistance = 0.6f; //벽으로 판정하는 최대 거리
    public float ceilingDistance = 0.05f; // 천장으로 판정하는 최대 거리

    CapsuleCollider2D touchingCol; //캡슐 콜라이더 컴포넌트
    Animator anim; //애니메이터 컴포넌트

    //각 방향으로 레이캐스트 결과를 담는 배열
    RaycastHit2D[] groundHits = new RaycastHit2D[5]; 
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded; // 바닥에 닿아있는지 여부 확인
    public bool IsGrounded { get {
            return _isGrounded;        
        } private set{
            _isGrounded = value;
            anim.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [SerializeField]
    private bool _isOnwall; // 벽에 닿아있는지 여부 확인
    public bool IsOnWall
    {
        get
        {
            return _isOnwall;
        }
        private set
        {
            _isOnwall = value;
            anim.SetBool(AnimationStrings.isOnwall, value);
        }
    }

    [SerializeField]
    private bool _isOnCeiling; // 천장에 닿아있는지 여부 확인

    private Vector2 wallCheckDirection => 
        gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            anim.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        anim =  GetComponent<Animator>();
    }

    void FixedUpdate() //아래, 벽, 위 방향으로 레이캐스트를 쏘아 접촉 여부 판단
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
