using UnityEngine;

public class ActionSwitch : MonoBehaviour
{
	[SerializeField]
	UnitAction.Action m_action;

	UnitAction m_unitAction;
	UnitManager m_unitManager;

	private void Awake()
	{
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		m_unitAction = gameController.GetComponent<UnitAction>();
		m_unitManager = gameController.GetComponent<UnitManager>();
	}

	public void OnAction() 
	{
		//�t�F�[�Y���Z���N�g�܂Ŗ߂�
		m_unitManager.SetPhase(UnitManager.Phase.Select);

		//�ړ��\�\�����I�t
		m_unitAction.OnRemove();

		//�A�N�V�����̕ύX
		m_unitAction.SetAction(m_action);
	}
}

