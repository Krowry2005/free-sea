using System.Diagnostics.Contracts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Explanation : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_explanation;

    [SerializeField]
    GameObject m_explanationPanel;

    private void Start()
    {
        m_explanationPanel.SetActive(false);
    }

    public void TakeDamage(Unit takeUnit, Unit giveUnit,int damage)
    //ダメージの表記
    {
        m_explanationPanel.SetActive(true);
        m_explanation.text = giveUnit.Name + "が " + takeUnit.Name + "に" + damage + "のダメージを与えた。";
    }

    public void MissDamage()
    {
        m_explanationPanel.SetActive(true);
        m_explanation.text = "ミス！\n" + "ダメージを与えられなかった";
    }
    
    public void DeathUnit(Unit unit)
    {
        m_explanation.text = unit.Name + "は倒れた";
    }

    public void NotEnoughSP(Skill usedSkill)
    {
        m_explanation.text = usedSkill.GetKanjiName() + "に必要なSPが足りない";
    }

    public void EndExplanation()
    {
        m_explanationPanel.SetActive(false);
    }

}
