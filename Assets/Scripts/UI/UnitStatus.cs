using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UnitStatus : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_unitStatus;

    [SerializeField]
    TextMeshProUGUI m_HP;

    [SerializeField] 
    Slider m_HPSlider;

    [SerializeField]
    TextMeshProUGUI m_SP;

    [SerializeField]
    Slider m_SPSlider;

    public void TextUnitStatus(Unit unit)
    {
        m_unitStatus.text = " " + unit.Name;
        m_HP.text = "HP : " + unit.HealthValue;
        m_SP.text = "SP : " + unit.SP;

        m_HPSlider.maxValue = unit.MaxHP;
        m_SPSlider.maxValue = unit.MaxSP;
        m_HPSlider.value = unit.HealthValue;
        m_SPSlider.value = unit.SP;
    }
}
