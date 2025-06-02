using UnityEngine;

public class Choice : MonoBehaviour
{
	public enum Mass
	{
		Possible,
		ImPossible,
		Event,
	}

	[SerializeField]
	GameObject m_choiceBlock;

	[SerializeField]
	bool m_possible;

	[SerializeField]
	Mass m_mass;

	public bool Possible => m_possible;
	public Mass Attribute => m_mass;

	public void OnChoice()
	{
		m_choiceBlock.SetActive(true);
	}

	public void OnCancell()
	{
		m_choiceBlock.SetActive(false);
	}

}
