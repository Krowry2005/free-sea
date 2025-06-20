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
	GameObject m_extentBlock;

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

	public void OnChoice(bool choice)
	{
		m_choiceBlock.SetActive(choice);
	}

	public void OnExtent(bool display)
	{
		m_extentBlock.SetActive(display);
	}

	public void SetPossible(bool possible)
	{
		m_possible = possible;
	}
}
