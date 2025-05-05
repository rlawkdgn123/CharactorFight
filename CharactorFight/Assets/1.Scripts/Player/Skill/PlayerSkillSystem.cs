using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSkillystem : MonoBehaviour
{
    public Image[] SkillIcon = new Image[2];
    public Image[] SkillIconShadow = new Image[2];
    public SkillData[] skillList;// 사용 가능한 스킬 목록
    public PlayerBase player;
    public PlayerMana pm;
    //public bool skillOn;
    [SerializeField] private Vector3 scale;
    public bool[] skillOn = new bool[2];//스킬 활성화 여부
    public bool skillCoolDown = false; // 스킬 쿨타임 진행중 여부;
    //[SerializeField] private bool skillManaCheck = false; //스킬 사용 마나 확인 여부
    Rigidbody2D rb;
    Animator anim;
    TouchingDirection touchingDirection;
    [SerializeField] public int selectedSkillIndex;
    [SerializeField] private bool isCoolDown = false;
    [SerializeField] private bool isSkill = false;
    //[SerializeField] private PlayerSoundSystem soundSystem;
    private void Awake() {
        PlayerInitialize();

        /*for (int i = 0; i < skillList.Length; i++)
        {
            skillList[i] = skillList[i];
            SkillIcon[i].sprite = skillList[i].skillIcon;
            SkillIconShadow[i].sprite = skillList[i].skillIcon;
        }*/
    }

    public void PlayerInitialize()
    {
        player = gameObject.GetComponent<PlayerBase>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        pm = gameObject.GetComponent<PlayerMana>();
        //td = gameObject.GetComponent<TouchingDirection>();
        
    }


    void Update()
    {
        //print(player.skillOn + " " + isCoolDown);
/*        if (skillOn[player.selectedSkillIndex] && !isCoolDown) // 스킬 비활성화(쿨다운)중이면
        {
            print("진입");
            isCoolDown = true;
            
        }*/
    }
    public void OnSkill1()   // A키, 기둥 소환 공격
    {
        if (player.GetCurDirState(PlayerBase.DirState.Ground) && !player.GetKeyIgnore() && !isSkill) // 스킬 활성화(쿨다운이 끝나있는지) 여부 및 땅에 있는 지 확인
        {
            selectedSkillIndex = 0;
            Debug.Log("스킬사용 1");
            if (!skillOn[selectedSkillIndex] && pm.GetCurMana() >= skillList[selectedSkillIndex].manaCost)  // 기존 조건(쿨다운) + 마나 소모량이 일정 이상이면 스킬 실행
            {
                skillOn[selectedSkillIndex] = true;    // 반복 스킬 사용 방지
                anim.SetTrigger(AnimationStrings.SkillTrigger1);    // 스킬 사용 캐릭터 애니메이션 실행
                 pm.SubCurMana(skillList[selectedSkillIndex].manaCost); // 마나 소모량에 따라 감소
                StartCoroutine(SkillSpawn(selectedSkillIndex));  // 스킬 프리팹 생성
            }
            else
            {
                Debug.Log("스킬사용 1 실패");
            }
        }
    }
   

    IEnumerator SkillSpawn(int selectedSkillIndex) { // 스킬 스폰 시스템
        // 스킬 사용 로직
        //PlayerSkill playerSkill;
        if(!isSkill) // 스킬의 모션 연사 방지
        {
            isSkill = true;
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);    // 스킬 스폰 지점 구하기
            yield return new WaitForSeconds(skillList[selectedSkillIndex].spawnDelay);
            if (transform.localScale.x >= 0) // 플레이어가 오른쪽을 바라보고 있을 경우 == true
            {
                skillList[selectedSkillIndex].projectilePrefab.GetComponent<SpriteRenderer>().flipX = false;
                spawnPosition = new Vector2(transform.position.x + skillList[selectedSkillIndex].spawnPosx, transform.position.y + skillList[selectedSkillIndex].spawnPosy);
                //soundSystem.SkillSound(selectedSkillIndex);
                GameObject skill = Instantiate(skillList[selectedSkillIndex].projectilePrefab, spawnPosition, Quaternion.identity);
                skill.name = skillList[selectedSkillIndex].name; // 스킬 데이터에 따른 인스펙터 이름 변경
                if (skillList[selectedSkillIndex].freezePlayerPos == true)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    Invoke("PlayerFreezeStop", skillList[selectedSkillIndex].duration);
                }

                Destroy(skill, skillList[selectedSkillIndex].duration);
                print(skill + "스킬 사용. 우");
            }
            else
            {
                skillList[selectedSkillIndex].projectilePrefab.GetComponent<SpriteRenderer>().flipX = false;
                spawnPosition = new Vector2(transform.position.x - skillList[selectedSkillIndex].spawnPosx, transform.position.y + skillList[selectedSkillIndex].spawnPosy);
                GameObject skill = Instantiate(skillList[selectedSkillIndex].projectilePrefab, spawnPosition, Quaternion.identity);
                Vector3 localScale = skill.transform.localScale;
                localScale.x = localScale.x * (-1); // 스킬 뒤집기
                skill.transform.localScale = localScale;

                skill.name = skillList[selectedSkillIndex].name; // 스킬 데이터에 따른 인스펙터 이름 변경

                if (skillList[selectedSkillIndex].freezePlayerPos == true)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    Invoke("PlayerFreezeStop", skillList[selectedSkillIndex].duration);
                }

                Destroy(skill, skillList[selectedSkillIndex].duration);
                print(skill + "스킬 사용. 좌");
            }
            
        }
        //skillAudio.clip = skillList[selectedSkillIndex].skillSpawnAudioSource;  // skillAudio 자식 오브젝트에 사용할 사운드 할당 및 실행
        //skillAudio.Play();
        //cameraObj.GetComponent<CameraMange>().VibrateForTime(0.5f);
        yield return new WaitForSeconds(skillList[selectedSkillIndex].motionTime);
        isSkill = false;
        StartCoroutine(SkillCoolDown(selectedSkillIndex));
        //yield return new WaitForSeconds(skillList[selectedSkillIndex].cooldown); // 쿨타임 카운트
    }
    IEnumerator SkillCoolDown(int selectedSkillIndex) {
        float nowtime = 0;
        int SkillIndex = selectedSkillIndex;
        SkillIcon[SkillIndex].fillAmount = 0f;
        while (nowtime <= skillList[SkillIndex].cooldown)
        {
            nowtime += Time.deltaTime;
            SkillIcon[SkillIndex].fillAmount = nowtime / skillList[SkillIndex].cooldown;
            yield return null;
        }
        skillOn[SkillIndex] = false;     // 스킬 재사용 가능 전환
    }
    void PlayerFreezeStop() {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}