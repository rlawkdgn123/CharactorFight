using UnityEngine;

public class playerSkill_Zoro : PlayerSkillBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerInitialize();
    }
    override public void  OnSkill2()
    {
        if (player.GetCurDirSetGround() && !isSkill) // 스킬 활성화(쿨다운이 끝나있는지) 여부 및 땅에 있는 지 확인
        {
            
            selectedSkillIndex = 1;

            if (!skillOn[selectedSkillIndex] && pm.GetCurMana() >= skillList[selectedSkillIndex].manaCost)  // 기존 조건(쿨다운) + 마나 소모량이 일정 이상이면 스킬 실행
            {
        /*        Debug.Log("스킬사용 1");
                skillOn[selectedSkillIndex] = true;    // 반복 스킬 사용 방지
                anim.SetTrigger(AnimationStrings.SkillTrigger2);    // 스킬 사용 캐릭터 애니메이션 실행
                pm.SubCurMana(skillList[selectedSkillIndex].manaCost); // 마나 소모량에 따라 감소
                StartCoroutine(JoroSkill2());  // 스킬 프리팹 생성*/
            }
            else
            {
                Debug.Log("스킬사용 1 실패");
            }
        }
    }

}
