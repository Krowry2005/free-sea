using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitAction : MonoBehaviour
{
	Animator m_animator;
	TurnManager m_turnManager;

	GameObject m_turnUnit;
	GameObject m_targetMass;
	Vector3 m_targetPos;

	private void Awake()
	{
		m_turnManager = GetComponent<TurnManager>();
	}

	private void Start()
	{
		
	}

	private void Update()
	{
		//���݂̃^�[����Unit���X�V
		m_turnUnit = m_turnManager.TurnUnit;
		switch (m_turnManager.GetPhase)
		{
			case TurnManager.Phase.Select:





				if (Input.GetMouseButtonDown(0))
				{
					OnPick();
				}
				break;

			case TurnManager.Phase.Action:
				
				
				m_turnManager.NextPhase(TurnManager.Phase.End);
				break;
		}
	}

	public void OnPick()
	{
		// �^�b�v�����ꏊ�ɃJ��������Ray���΂�
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			//�}�X�̑I��
			m_targetMass = hit.collider.gameObject;
			m_targetMass.TryGetComponent(out Choice choice);
			if(choice.Possible)
			{
				m_targetPos = m_targetMass.transform.position;
				m_turnManager.NextPhase(TurnManager.Phase.Action);
			}
		}
	}
}
