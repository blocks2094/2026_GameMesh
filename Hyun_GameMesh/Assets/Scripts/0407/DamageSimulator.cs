using TMPro;
using UnityEngine;

public class DamageSimulator : MonoBehaviour
{
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangeDisplay;

    private int level = 1;
    private float totalDamage = 0, baseDamage = 20f;
    private int attackCount = 0;
    private int critCount = 0;
    private int missCount = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;

    void Start()
    {
        SetWeapon(0);
    }

    private void ResetData()
    {
        totalDamage = 0;
        attackCount = 0;
        level = 1;
        baseDamage = 20f;
    }

    

    public void SetWeapon(int id)
    {
        ResetData();
        if(id == 0)
        {
            SetStats("단검", 0.1f, 0.4f, 1.5f);
        }
        else if(id == 1)
        {
            SetStats("장검", 0.2f, 0.3f, 2.5f);
        }
        else
        {
            SetStats("도끼", 0.3f, 0.2f, 3.0f);
        }

        logDisplay.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();
    }

    private void SetStats(string _name, float _stdDev, float _criRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _criRate;
        critMult = _critMult;
    }

    public void LevelUp()   // 레벨업 버튼
    {
        totalDamage = 0;
        attackCount = 0;
        level++;
        baseDamage = level * 20f;
        logDisplay.text = string.Format("레벨업! 현재 레벨: {0}", level);
        UpdateUI();
    }

    public void OnAttack()  // 한번 공격 버튼
    {
        // 정규 분포 데미지 계산
        float sd = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, sd);

        // 치명타 판정
        bool isCrit = Random.value < critRate;
        float finalDamage = isCrit ? normalDamage * critMult : normalDamage;

        // 통계 누적
        attackCount++;
        totalDamage += finalDamage;

        // 로그 및 UI 업데이트
        string critMark = isCrit ? "<color=red>[치명타!]</color> " : "";
        logDisplay.text = string.Format("{0}데미지: {1:F1}", critMark, finalDamage);
        UpdateUI();
    }

    private void UpdateUI()
    {
        statusDisplay.text = string.Format("Level: {0} / 무기: {1} 기본 데미지: {2} / 치명타: {3}% (x{4})",
            level, weaponName, baseDamage, critRate * 100, critMult);

        rangeDisplay.text = string.Format("예상 일반 데미지 범위 : [{0:F1} ~ {1:F1}]",
            baseDamage - (3 * baseDamage * stdDevMult),
            baseDamage + (3 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = string.Format("누적 데미지: {0:F1} 공격 횟수: {1} 평균 DPA: {2:F2}",
            totalDamage, attackCount, dpa);
    }

    public void attackCountUp()   
    {
        for (int i = 0; i < 1000; i++)
        {

            // 정규 분포 데미지 계산
            float sd = baseDamage * stdDevMult;
            float normalDamage = GetNormalStdDevDamage(baseDamage, sd);
            
            if(normalDamage > baseDamage + 2 * sd)
            {
                      
            }

            // 치명타 판정
            bool isCrit = Random.value < critRate;
            float finalDamage = isCrit ? normalDamage * critMult : normalDamage;

            // 통계 누적
            attackCount++;
            totalDamage += finalDamage;

            // 로그 및 UI 업데이트
            string critMark = isCrit ? "<color=red>[치명타!]</color> " : "";
            logDisplay.text = string.Format("{0}데미지: {1:F1}", critMark, finalDamage);
            UpdateUI();
        }
    }

    private float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value; 
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}
