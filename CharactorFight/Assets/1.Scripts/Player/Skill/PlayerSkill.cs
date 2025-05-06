using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자 : 김장후
public class PlayerSkill : MonoBehaviour
{
    public SkillData skillData; // 스킬 데이터(스크립터블 오브젝트) 받아오기
    SkillData_Sub skillDataSub; // 폭발 시 생길 폭발 프리팹 데이터(SkillData_sub) 받아오기
    PlayerSkill_Sub playerSkillSub; //폭발 시 생길 프리팹 데이터 스크립트
    public PlayerBase player;

    private int skillDamage;
    private Vector2 skillKnockback;
    private void Awake()
    {
        skillDamage = skillData.damage;
        skillKnockback = skillData.knockback;
    }
    private void Update() {
        //if (skillData.skillFire == true && GetComponent<SpriteRenderer>().flipX == true) // 왼쪽을 바라보고 있으면
        if (skillData.skillFire == true && transform.localScale.x < 0) // 왼쪽을 바라보고 있으면
        {
            this.transform.Translate(Vector3.left * (skillData.skillSpeed * 0.01f)); // 왼쪽으로 스피드*0.01f만큼 이동
        }//else if (skillData.skillFire == true && GetComponent<SpriteRenderer>().flipX == false) // 오른쪽을 바라보고 있으면
        else if (skillData.skillFire == true && transform.localScale.x >= 0) // 오른쪽을 바라보고 있으면
        {
            this.transform.Translate(Vector3.right * (skillData.skillSpeed * 0.01f));// 오른쪽으로 스피드*0.01f만큼 이동
        }
    }
    private void OnTriggerEnter2D(Collider2D col) {   // 콜라이더2D에 감지되면
        if (col.tag.Equals("Enemy") || col.tag.Equals("Player"))  // 태그값이 적이거나 플레이어일 경우
        {
            PlayerHealth damageable = col.GetComponent<PlayerHealth>();
            if (damageable != null && player.GetCurChoice() != col.GetComponent<PlayerBase>().GetCurChoice())
            {
                bool gotHit = damageable.Hit(skillDamage, skillKnockback, transform.position);
                if (gotHit)
                {
                    Debug.Log(col.name + " hit for " + skillDamage);

                }
            }
            /*Damageable damageable = collision.GetComponent<Damageable>();*/   // 해당 적의 Damageable스크립트 접근 (즉, 맞는 대상에게 Damageable 스크립트가 있어야 함)
                                                                                //Hit hit = collision.GetComponent<Hit>();
                                                                                //damageable.Hit(skillData.damage,skillData.knockback);   
            /*damageable.Hit(skillDamage, skillKnockback)*/
            ; // 데미지 및 넉백 적용


            print("1차 감지");

            if(skillData.explosionPrefab != null)   //  폭발을 표현할 프리팹이 있는 경우
            {
                print("2차 감지");
                Vector2 explosionSpawnPos = this.gameObject.transform.position; 
                GameObject explosionPrefab = Instantiate(skillData.explosionPrefab, explosionSpawnPos , Quaternion.identity);
                playerSkillSub = explosionPrefab.GetComponent<PlayerSkill_Sub>();
                playerSkillSub.target = col.gameObject;

                //Destroy(explosionPrefab, skillData_Sub.duration); // 자식의 스크립터블 오브젝트를 참조하여 미리 제거 예약*/
                if (skillData.skillPenetrate != true) // 관통이 활성화되지 않았을 경우
                    Destroy(this.gameObject);   // 프로젝타일 프리팹 소멸
            }
            /*if (damageable != null)
            {
                bool gotHit = damageable.Hit(skillData.damage, skillData.knockback);

                if (gotHit)
                {
                    Debug.Log(collision.name + " hit for " + skillData.damage);
                }
            }*/
        }
    }
}