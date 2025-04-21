using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{

    public ContactFilter2D castFilter; //����ĳ��Ʈ �浹 ���� ���� ����
    public float groundDistance = 0.05f; // �ٴ����� �����ϴ� �ִ� �Ÿ�
    public float wallDistance = 0.6f; //������ �����ϴ� �ִ� �Ÿ�
    public float ceilingDistance = 0.05f; // õ������ �����ϴ� �ִ� �Ÿ�

    CapsuleCollider2D touchingCol; //ĸ�� �ݶ��̴� ������Ʈ
    Animator anim; //�ִϸ����� ������Ʈ

    //�� �������� ����ĳ��Ʈ ����� ��� �迭
    RaycastHit2D[] groundHits = new RaycastHit2D[5]; 
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded; // �ٴڿ� ����ִ��� ���� Ȯ��
    public bool IsGrounded { get {
            return _isGrounded;        
        } private set{
            _isGrounded = value;
            anim.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [SerializeField]
    private bool _isOnwall; // ���� ����ִ��� ���� Ȯ��
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
    private bool _isOnCeiling; // õ�忡 ����ִ��� ���� Ȯ��

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

    void FixedUpdate() //�Ʒ�, ��, �� �������� ����ĳ��Ʈ�� ��� ���� ���� �Ǵ�
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
