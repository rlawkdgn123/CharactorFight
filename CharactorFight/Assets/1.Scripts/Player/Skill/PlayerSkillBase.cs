using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSkillBase : MonoBehaviour
{
    [Header("DefaultSettings")]
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] private GameObject ui;
    public PlayerBase player;
    public PlayerMana pm;

    [Header("SkillSettings")]
    public Image[] SkillIcon;
    public Image[] SkillIconShadow;
    public SkillData[] skillList;// 사용 가능한 스킬 목록
    //public bool skillOn;
    [SerializeField] private Vector3 scale;
    public bool[] skillOn = new bool[2];//스킬 활성화 여부
    public bool skillCoolDown = false; // 스킬 쿨타임 진행중 여부;
    //[SerializeField] private bool skillManaCheck = false; //스킬 사용 마나 확인 여부
    [SerializeField] public int selectedSkillIndex;
    [SerializeField] private bool isCoolDown = false;
    [SerializeField] private bool isSkill = false;
    //[SerializeField] private PlayerSoundSystem soundSystem;
    private void Start() {
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
        this.tag = "Player";

        player = gameObject.GetComponent<PlayerBase>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        pm = gameObject.GetComponent<PlayerMana>();

        ui = player.GetPlayerUI();
        SkillIcon = new Image[skillList.Length];
        SkillIconShadow = new Image[skillList.Length];
        string barName = "";

        if (player.GetCurChoice(PlayerBase.PlayerChoice.P1)) { barName = "P1_PlayerSkillList"; }
        else if (player.GetCurChoice(PlayerBase.PlayerChoice.P2)) { barName = "P2_PlayerSkillList"; }

        Transform tr = ui.transform.Find(barName);
        if(tr != null)
        {
            for (int i = 0; i < skillList.Length; i++)
            {
                if (player.GetCurChoice(PlayerBase.PlayerChoice.P1)) { barName = "P1_SkillSprite" + (i + 1); }
                else if (player.GetCurChoice(PlayerBase.PlayerChoice.P2)) { barName = "P2_SkillSprite" + (i + 1); }

                Transform tr2 = tr.Find(barName);

                if (tr2 != null)
                {
                    IconFill icf = tr2.GetComponent<IconFill>();
                    icf.FillIcon(skillList[i].skillIcon);
                    SkillIcon[i] = icf.images[2];
                    SkillIconShadow[i] = icf.images[1];
                }
                else { print(barName + "을 찾을 수 없습니다."); }
            }
        }
        else
        {
            print(barName + "을 찾을 수 없습니다.");
        }
        
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
    public void OnSkill1()  
    {
        if (player.GetCurDirSetGround() && !isSkill) // 스킬 활성화(쿨다운이 끝나있는지) 여부 및 땅에 있는 지 확인
        {
            Debug.Log("통과 1");
            selectedSkillIndex = 0;
            
            if (!skillOn[selectedSkillIndex] && pm.GetCurMana() >= skillList[selectedSkillIndex].manaCost)  // 기존 조건(쿨다운) + 마나 소모량이 일정 이상이면 스킬 실행
            {
                Debug.Log("스킬사용 2");
                skillOn[selectedSkillIndex] = true;    // 반복 스킬 사용 방지
                anim.SetTrigger(AnimationStrings.SkillTrigger2);    // 스킬 사용 캐릭터 애니메이션 실행
                 pm.SubCurMana(skillList[selectedSkillIndex].manaCost); // 마나 소모량에 따라 감소
                StartCoroutine(SkillSpawn(selectedSkillIndex));  // 스킬 프리팹 생성
            }
            else
            {
                Debug.Log("스킬사용 1 실패");
            }
        }
    }
    public void OnSkill2()  
    {
        if (player.GetCurDirSetGround() && !isSkill) // 스킬 활성화(쿨다운이 끝나있는지) 여부 및 땅에 있는 지 확인
        {
            Debug.Log("통과 1");
            selectedSkillIndex = 1;

            if (!skillOn[selectedSkillIndex] && pm.GetCurMana() >= skillList[selectedSkillIndex].manaCost)  // 기존 조건(쿨다운) + 마나 소모량이 일정 이상이면 스킬 실행
            {
                Debug.Log("스킬사용 1");
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
            
            skillList[selectedSkillIndex].projectilePrefab.GetComponent<SpriteRenderer>().flipX = false;
            spawnPosition = new Vector2(transform.position.x + skillList[selectedSkillIndex].spawnPosx, transform.position.y + skillList[selectedSkillIndex].spawnPosy);
            //soundSystem.SkillSound(selectedSkillIndex);
            GameObject skill = Instantiate(skillList[selectedSkillIndex].projectilePrefab, spawnPosition, Quaternion.identity);
            if (skillList[selectedSkillIndex].spawnEffect && skillList[selectedSkillIndex].spawnEffectPrefab != null)
            {
                
                if (transform.localScale.x < 0) // 플레이어가 왼쪽을 바라보면
                {
                    GameObject spawnEffect = Instantiate(skillList[selectedSkillIndex].spawnEffectPrefab, new Vector3(spawnPosition.x - 2, spawnPosition.y, 0), Quaternion.identity);
                    Vector3 localScale = spawnEffect.transform.localScale;
                    spawnEffect.transform.localScale *= (-1);
                    Destroy(spawnEffect, 1f);
                    }
                else
                {
                    GameObject spawnEffect = Instantiate(skillList[selectedSkillIndex].spawnEffectPrefab, new Vector3(spawnPosition.x + 2, spawnPosition.y, 0), Quaternion.identity);
                    Destroy(spawnEffect, 1f);
                }
            }
            skill.GetComponent<PlayerSkill>().player = player;

            if (transform.localScale.x < 0) // 플레이어가 왼쪽을 바라보면
            {
                Vector3 localScale = skill.transform.localScale;
                localScale.x = localScale.x * (-1); // 스킬 뒤집기
                skill.transform.localScale = localScale;
                print(skill + "스킬 사용. 좌");
            }
            else { print(skill + "스킬 사용. 우"); }

            skill.name = skillList[selectedSkillIndex].name; // 스킬 데이터에 따른 인스펙터 이름 변경
            if (player.GetCurChoice(PlayerBase.PlayerChoice.P1)) { skill.layer = LayerMask.NameToLayer("P1Attack"); }
            else if (player.GetCurChoice(PlayerBase.PlayerChoice.P2)) { skill.layer = LayerMask.NameToLayer("P2Attack");  }
                
            if (skillList[selectedSkillIndex].freezePlayerPos == true)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                Invoke("PlayerFreezeStop", skillList[selectedSkillIndex].duration);
            }

            Destroy(skill, skillList[selectedSkillIndex].duration);
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