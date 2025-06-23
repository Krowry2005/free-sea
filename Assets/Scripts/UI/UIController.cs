using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
	[SerializeField]
	GameObject[] m_actionButton;

	[SerializeField]
	GameObject m_cameBackButton;

	[SerializeField]
	GameObject m_gameController;

	[SerializeField]
	GameObject m_skillWindow;

	UnitManager m_unitManager;
	UnitAction m_unitAction;

	private void Awake()
	{
		m_unitManager = m_gameController.GetComponent<UnitManager>();
		m_unitAction = m_gameController.GetComponent <UnitAction>();
	}

	private void Update()
	{
		if(m_unitManager.GetPhase != UnitManager.Phase.Select)
		{
			m_skillWindow.SetActive(false);
			m_cameBackButton.SetActive(true);
			foreach(GameObject actionButton in m_actionButton)
			{
				actionButton.SetActive(false);
			}
		}
		else
		{
			if (m_unitAction.GetAction == UnitAction.Action.Skill)
			{
				m_skillWindow.SetActive(true);
			}
			m_cameBackButton.SetActive(false);
			foreach(GameObject actionButton in m_actionButton)
			{
				actionButton.SetActive(true);
			}
		}
	}
}
