using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 1차 작성자 : 김재성  2차 작성자 : 김장후
public class PlayerHealth : MonoBehaviour
{
    [Header("DefaultSettings")]
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerBase player;
    [SerializeField] private GameObject ui;
    //public UIManager uiManager;
    public Slider hpBar;
    private bool death = false;
    public LayerMask playerMask;
    public LayerMask EnemyMask;
    public int maxHealth = 1000;

    [SerializeField]
    public int curHealth = 1000;

    [Header("HitSettings")]
    [SerializeField]
    private bool isInvincible = false; // 무적 상태인지
    [SerializeField] private float defaultAtkstunTime = 0.2f;
    private float timeSinceHit = 0; //무적 시간 누적 시간
    public float invincibilityTime = 0.25f; //무적 시간

   

    private void Start()
    {
        PlayerInitialize();
    }

    private void Update() //무적 시간 상태면 누적 시간 갱신하고, 무적이 끝났을 경우 무적 상태 해제
    {
        if (hpBar != null) // 만약 hpBar 이미지가 할당되었다면
            hpBar.value = ((float)curHealth / (float)maxHealth); // hpBar fillAmount값 변경

        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
        if (curHealth <= 0)
        {
            player.SetisAlive(false);
        }
    }
    public bool LockVelocity //이동 제한 상태 여부
    {
        get
        {
            return anim.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            anim.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    public bool Hit(int damage, Vector2 knockback, Vector2 atkerPos) //데미지 처리 
    {
        if (player.GetIsAlive() && !isInvincible)
        {
            rb.linearVelocity = Vector2.zero;
            curHealth -= damage;

            LockVelocity = true;
            anim.SetTrigger(AnimationStrings.hitTrigger);

            player.StopAndIgnore(true);
            StartCoroutine(StopKnockback(defaultAtkstunTime));
            
            if(atkerPos.x < player.GetPosition().x)
            {
                rb.linearVelocity = knockback;
            }
            else if (atkerPos.x > player.GetPosition().x)
            {
                rb.linearVelocity = new Vector2(knockback.x * -1, knockback.y); ;
            }

        /*  if(player.transform.localScale.x < 0)
            {
                rb.linearVelocity = knockback;
            }else if (player.transform.localScale.x > 0)
            {
                rb.linearVelocity = new Vector2(knockback.x * -1, knockback.y); ;
            }*/

                return true;
        }
        //Unable to be hit
        return false;
    }
    private IEnumerator StopKnockback(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.StopAndIgnore(false);
    }
    /*    public void GameOver() //  새로 추가된 함수
        {
            this.gameObject.GetComponent<PlayerController>().getKeyIgnore = true; // 죽으면 플레이어 못 움직임
            Stop(); 
            uiManager.Show("오버",true); // 게임오버 팝업
        }*/
    public void Stop()
    {
        StopAllCoroutines();
    }
    public bool damageTest = false;
    public void hitTest(int damageTest)
    {
        curHealth -= damageTest;
    }
    public void PlayerInitialize()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponent<PlayerBase>();
        string tag = this.gameObject.tag;
        curHealth = maxHealth;

        ui = player.GetPlayerUI();

        string barName = "";
        if(player.GetCurChoice(PlayerBase.PlayerChoice.P1)) { barName = "P1_HPBar"; }
        else if (player.GetCurChoice(PlayerBase.PlayerChoice.P2)) { barName = "P2_HPBar"; }

        Transform tr = ui.transform.Find(barName);
        if (tr != null)
        {
            hpBar = tr.gameObject.GetComponentInChildren<Slider>();
            if (hpBar == null) { print("슬라이더가 존재하지 않습니다."); }
        }
        else { print(barName + "을 찾을 수 없습니다."); }
    }
}
