using UnityEngine;

public class ActionSwitch : MonoBehaviour
{
	[SerializeField]
	UnitAction.Action m_action;

	UnitAction m_unitAction;

	private void Awake()
	{
		m_unitAction = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitAction>();
	}

	public void OnAction() 
	{
		m_unitAction.SetAction(m_action);
	}
}

