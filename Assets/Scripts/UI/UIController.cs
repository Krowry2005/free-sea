using UnityEngine;

public class UIController : MonoBehaviour
{
	[SerializeField]
	GameObject[] m_actionButton;

	[SerializeField]
	GameObject m_cameBackButton;

	[SerializeField]
	GameObject m_gameController;

	UnitManager m_unitManager;


	private void Awake()
	{
		m_unitManager = m_gameController.GetComponent<UnitManager>();
	}

	private void Update()
	{
		if(m_unitManager.GetPhase != UnitManager.Phase.Select)
		{
			m_cameBackButton.SetActive(true);
			foreach(GameObject actionButton in m_actionButton)
			{
				actionButton.SetActive(false);
			}
		}
		else
		{
			m_cameBackButton.SetActive(false);
			foreach(GameObject actionButton in m_actionButton)
			{
				actionButton.SetActive(true);
			}
		}
	}
}
