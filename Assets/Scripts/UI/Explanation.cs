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
    //�_���[�W�̕\�L
    {
        m_explanationPanel.SetActive(true);
        m_explanation.text = giveUnit.Name + "�� " + takeUnit.Name + "��" + damage + "�̃_���[�W��^�����B";
    }

    public void MissDamage()
    {
        m_explanationPanel.SetActive(true);
        m_explanation.text = "�~�X�I\n" + "�_���[�W��^�����Ȃ�����";
    }
    
    public void DeathUnit(Unit unit)
    {
        m_explanation.text = unit.Name + "�͓|�ꂽ";
    }

    public void NotEnoughSP(Skill usedSkill)
    {
        m_explanation.text = usedSkill.GetKanjiName() + "�ɕK�v��SP������Ȃ�";
    }

    public void EndExplanation()
    {
        m_explanationPanel.SetActive(false);
    }

}
