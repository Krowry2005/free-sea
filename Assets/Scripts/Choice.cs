using UnityEngine;

public class Choice : MonoBehaviour
{
	public enum MassAttribute
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
	MassAttribute m_mass;

	GridMass m_gridMass;

	public bool Possible => m_possible;
	public MassAttribute Attribute => m_mass;

	private void Awake()
	{
		GameObject grid;
		grid = GameObject.FindGameObjectWithTag("Grid");
		m_gridMass = grid.GetComponent<GridMass>();
		m_gridMass.SetGrid(gameObject);
	}

	public void OnChoice()
	{
		m_choiceBlock.SetActive(true);
	}

	public void OnCancell()
	{
		m_choiceBlock.SetActive(false);
	}
}
