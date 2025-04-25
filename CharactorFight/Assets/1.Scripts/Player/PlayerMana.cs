using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static PlayerBase;
// 작성자 : 김장후
public class PlayerMana : MonoBehaviour
{
    [SerializeField] private int curMana;
    [SerializeField] private int maxMana;
    [SerializeField] private Image mpBar;                // 마나 게이지 UI
    [SerializeField] private PlayerBase player;        // 플레이어 오브젝트
    PlayerBase playerBase;  // 플레이어 컨트롤러 스크립트
    private void Awake() {
        PlayerInitialize(); // 컴포넌트 연결
        if (player.GetCurChoice(PlayerChoice.P1)) 
        { 
            GameObject obj = GameObject.Find("P1_ManaGauge");
            if (obj) mpBar = obj.GetComponent<Image>();
        }
        else if (player.GetCurChoice(PlayerChoice.P2))
        {
            GameObject obj = GameObject.Find("P2_ManaGauge");
            if (obj) mpBar = obj.GetComponent<Image>();
        }
        else { print(gameObject + "가 Player할당이 되어있지 않습니다."); }
    }

    private void PlayerInitialize()
    {
        player = gameObject.GetComponent<PlayerBase>();
    }

    private void Update()  // 플레이어의 마나량에 접근해 마나량에 따라 마나UI의 채우기값 조정
    {
        if (mpBar != null) // 만약 mpBar 이미지가 할당되었다면
            mpBar.fillAmount = ((float)curMana / (float)maxMana); // hpBar fillAmount값 변경
        
        if (curMana < 0) { curMana = 0; }
        else if (curMana > maxMana) { curMana = maxMana; }

    }
    public void MpCharge(int chargeMana)
    {
        Debug.Log("MP 충전, 현재 마나: " + curMana);
        curMana += chargeMana; // 플레이어 스크립트에 마나량 추가
    }
    public int GetCurMana() { return curMana; }
    public int GetMaxMana() { return maxMana; }
}
