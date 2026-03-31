using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    [Header("Combat Settings")]
    public int playerAttack = 30;
    public float targetCritRate = 0.30f; // 목표 치명타 확률 30%
    public int maxEnemyHp = 300;
    private int currentEnemyHp;

    [Header("Stats & Tracking")]
    private int totalHits = 0;
    private int critHits = 0;

    [Header("Loot Probabilities (Current)")]
    private float probNormal = 0.50f;
    private float probAdvanced = 0.30f;
    private float probRare = 0.15f;
    private float probLegendary = 0.05f;

    [Header("Inventory (Dropped Items)")]
    private int countNormal = 0;
    private int countAdvanced = 0;
    private int countRare = 0;
    private int countLegendary = 0;

    [Header("UI References")]
    public TMP_Text statsText;       // 전체 공격/치명타 횟수, 확률 정보 텍스트
    public TMP_Text enemyHpText;     // 적 체력 텍스트
    public TMP_Text probText;        // 현재 아이템 획득 확률 텍스트
    public TMP_Text inventoryText;   // 획득한 아이템 현황 텍스트

    void Start()
    {
        // 적 체력 초기화 및 UI 업데이트
        currentEnemyHp = maxEnemyHp;
        UpdateAllUI();
    }

    // [공격] 버튼의 OnClick 이벤트에 연결할 함수
    public void OnAttackButtonClicked()
    {
        bool isCrit = RollCrit();
        int damage = isCrit ? (playerAttack * 2) : playerAttack;

        currentEnemyHp -= damage;

        if (currentEnemyHp <= 0)
        {
            currentEnemyHp = 0;
            OnEnemyDeath();
        }

        UpdateAllUI();
    }

    // --- [실습 3] 치명타 보정 로직 (Image 2 참조) ---
    private bool RollCrit()
    {
        totalHits++;
        float currentRate = 0f;

        if (critHits > 0)
        {
            currentRate = (float)critHits / totalHits;
        }

        // 1. 치명타가 너무 안 뜰 경우 강제 발생
        if (currentRate < targetCritRate && (float)(critHits + 1) / totalHits <= targetCritRate)
        {
            Debug.Log("Critical Hit! (Forced)");
            critHits++;
            return true;
        }

        // 2. 치명타가 너무 많이 발생할 경우 강제로 일반 공격
        if (currentRate > targetCritRate && (float)critHits / totalHits >= targetCritRate)
        {
            Debug.Log("Normal Hit! (Forced)");
            return false;
        }

        // 3. 기본 확률 처리
        if (Random.value < targetCritRate)
        {
            Debug.Log("Critical Hit! (Base)");
            critHits++;
            return true;
        }

        Debug.Log("Normal Hit! (Base)");
        return false;
    }

    // --- 적 사망 및 전리품 획득 로직 ---
    private void OnEnemyDeath()
    {
        Debug.Log("Enemy Defeated! Rolling for loot...");
        RollLoot();

        // 적 부활 (체력 초기화)
        currentEnemyHp = maxEnemyHp;
    }

    private void RollLoot()
    {
        float roll = Random.value; // 0.0 ~ 1.0

        // 누적 확률로 아이템 등급 결정
        if (roll < probLegendary)
        {
            // 전설 획득 성공
            countLegendary++;
            ResetLootProbabilities();
        }
        else if (roll < probLegendary + probRare)
        {
            countRare++;
            ApplyPitySystem();
        }
        else if (roll < probLegendary + probRare + probAdvanced)
        {
            countAdvanced++;
            ApplyPitySystem();
        }
        else
        {
            countNormal++;
            ApplyPitySystem();
        }
    }

    // 전설 획득 실패 시 확률 보정 로직
    private void ApplyPitySystem()
    {
        probLegendary += 0.015f; // 전설 1.5% 증가
        probRare -= 0.005f;      // 희귀 0.5% 감소
        probAdvanced -= 0.005f;  // 고급 0.5% 감소
        probNormal -= 0.005f;    // 일반 0.5% 감소

        // 확률이 0 이하로 떨어지는 것을 방지 (안전 장치)
        probRare = Mathf.Max(0, probRare);
        probAdvanced = Mathf.Max(0, probAdvanced);
        probNormal = Mathf.Max(0, probNormal);
    }

    // 전설 획득 시 확률 초기화
    private void ResetLootProbabilities()
    {
        probNormal = 0.50f;
        probAdvanced = 0.30f;
        probRare = 0.15f;
        probLegendary = 0.05f;
    }

    // --- UI 업데이트 ---
    private void UpdateAllUI()
    {
        // 1. 치명타 통계 UI
        float actualCritRate = totalHits == 0 ? 0 : ((float)critHits / totalHits) * 100f;
        statsText.text = $"전체 공격 횟수 : {totalHits}\n" +
                         $"발생한 치명타 횟수 : {critHits}\n" +
                         $"설정된 치명타 확률 : {targetCritRate * 100}%\n" +
                         $"실제 치명타 확률 : {actualCritRate:F2}%";

        // 2. 적 체력 UI
        enemyHpText.text = $"체력 : {currentEnemyHp}/{maxEnemyHp}";

        // 3. 현재 아이템 확률 UI
        probText.text = $"<b>현재 아이템 확률</b>\n" +
                        $"일반 : {probNormal * 100:F1}%\n" +
                        $"고급 : {probAdvanced * 100:F1}%\n" +
                        $"희귀 : {probRare * 100:F1}%\n" +
                        $"전설 : {probLegendary * 100:F1}%";

        // 4. 획득한 아이템 UI
        inventoryText.text = $"<b>현재 드롭된 아이템</b>\n" +
                             $"일반 : {countNormal}\n" +
                             $"고급 : {countAdvanced}\n" +
                             $"희귀 : {countRare}\n" +
                             $"전설 : {countLegendary}";
    }
}