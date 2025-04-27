using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkill_Sub : MonoBehaviour
{
    public SkillData skillData; // 폭발 전 원본 스킬 데이터(스크립터블 오브젝트) 받아오기
    public SkillData_Sub skillDataSub; // 폭발 시 생길 폭발 프리팹 데이터(SkillData_sub) 받아오기
    public GameObject target;
    Animator anim;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }
    private void Start() 
    {
        Destroy(this.gameObject,skillDataSub.duration);   // 지속시간 이후 소환 프리팹 소멸
    }
    private void Update() 
    {
        if (!skillDataSub.skillAttach)// 부착 상태가 아니면 (발사)
        {
            if (skillData.skillFire == true && GetComponent<SpriteRenderer>().flipX == true) // 왼쪽을 바라보고 있으면
                this.transform.Translate(Vector3.left * (skillDataSub.skillSpeed * 0.01f)); // 왼쪽으로 스피드*0.01f만큼 이동
            else if (skillData.skillFire == true && GetComponent<SpriteRenderer>().flipX == false) // 오른쪽을 바라보고 있으면
                this.transform.Translate(Vector3.right * (skillDataSub.skillSpeed * 0.01f));// 오른쪽으로 스피드*0.01f만큼 이동
        }else if (skillDataSub.skillAttach)
        {
             this.transform.position = target.transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   // 콜라이더2D에 감지되면
        if (collision.tag.Equals("Enemy") || collision.tag.Equals("Boss"))  // 태그값이 보스나 적일 경우, 또한 할당 받은 적과 같은 콜라이더일 경우
        {
            /*Damageable damageable = collision.GetComponent<Damageable>();   // 해당 적의 Damageable스크립트 접근
            damageable.Hit(skillDataSub.damage, skillDataSub.knockback);    // 데미지 및 넉백 적용

            if (skillDataSub.Stun == true)
            {
                //StartCoroutine(CCStun(target, skillDataSub.StunTime));  // 적에게 스턴 적용
                StartCoroutine(CCStuntick(target, skillDataSub.StunTime, skillDataSub.stunTick, skillDataSub.stunCycle));  // 적에게 스턴 적용
            }
            if (damageable != null)
            {
                bool gotHit = damageable.Hit(skillData.damage, skillData.knockback);

                if (gotHit)
                {
                    Debug.Log(collision.name + " hit for " + skillData.damage);
                }
            }*/
        }
    }
    private IEnumerator CCStun(GameObject target, float time) { // CC기 - 스턴
        print("스턴 실행");
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        target.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.4f);
        targetRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(time);
        print("스턴 해제");
        targetRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        target.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
    private IEnumerator CCStuntick(GameObject target, float time, float tick, int cycle) {  // CC기 - 스턴 (여러 번 나누어)
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        for (int i =0; i < cycle; i++)
        {
            print("경직 시작"+i);
            target.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
            targetRb.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSeconds(time);
            print("경직 해제"+i);
            target.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            targetRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            yield return new WaitForSeconds(tick);
        }
    }
}
