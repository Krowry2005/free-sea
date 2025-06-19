using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillAction : MonoBehaviour
{
	[SerializeField]
	UnitManager m_unitManager;

	[SerializeField]
	UnitAction m_unitAction;

	[SerializeField]
	GameObject[] m_actionButton;



	public void OnSelect()
	{
		Unit unitStatus = m_unitManager.TurnUnit.GetComponent<Unit>();
		for (int i = 0; i < m_actionButton.Length; i++)
		{
			if (i >= unitStatus.GetSkill().Count)
			{
				m_actionButton[i].SetActive(false);
			}
			else
			{
				m_actionButton[i].GetComponentInChildren<TextMeshProUGUI>().text = unitStatus.GetSkill()[i].GetKanjiName();
				//�����_���̒��� i ���g���Ă��邽�߁A���ׂẴ{�^�����Ō�� i �̒l���g���Ă��܂�
				int index = i;
				m_actionButton[i].GetComponent<Button>().onClick.RemoveAllListeners();
				m_actionButton[i].GetComponent<Button>().onClick.AddListener(() => m_unitAction.OnDisplay(unitStatus.GetSkill()[index].GetRange(), true, true));
			}
		}
	}
}
