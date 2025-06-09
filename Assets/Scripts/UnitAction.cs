using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevelPhysics;

public class UnitAction : MonoBehaviour
{
	public enum Action
	{
		Choice,
		Move,
		Attack,
		Item,
		Skill,

		Length,
	}

	[SerializeField]
	GameObject m_mapBlock;

	Action m_action;

	UnitManager m_unitManager;
	GridMass m_gridmass;

	GameObject m_turnUnit;

	List<GameObject> m_viewList = new();
	Vector3 m_targetPos;
	bool m_move;
	int count;
	
	private void Awake()
	{
		m_unitManager = GetComponent<UnitManager>();
		m_gridmass = m_mapBlock.GetComponent<GridMass>();
	}

	private void Start()
	{
		m_move = false;
		m_action = Action.Choice;
	}

	private void Update()
	{
		//���݂̃^�[����Unit���X�V
		m_turnUnit = m_unitManager.TurnUnit;
		switch (m_unitManager.GetPhase)
		{
			case UnitManager.Phase.Select:
				if(!m_move)switch (m_action)
				{
					case Action.Move:
						OnMoveSelect(m_turnUnit.GetComponent<Unit>().Destination);
							//�A�N�V�����I����Ƀ}�X�̃s�b�N���\
							if (Input.GetMouseButtonDown(0)) OnPick();
							break;
					
					case Action.Attack:
						OnAttackSelect(m_turnUnit.GetComponent<Unit>().AttackPos);
							//�A�N�V�����I����Ƀ}�X�̃s�b�N���\
							if (Input.GetMouseButtonDown(0)) OnPick();
							break;

					case Action.Item:
							//�A�C�e���̎g�p
							//�A�N�V�����I����Ƀ}�X�̃s�b�N���\
							if (Input.GetMouseButtonDown(0)) OnPick();
							break;

					case Action.Skill:
							//�X�L���̎g�p
							//�A�N�V�����I����Ƀ}�X�̃s�b�N���\
							if (Input.GetMouseButtonDown(0)) OnPick();
							break;
				}
				break;

			case UnitManager.Phase.Action:
				switch (m_action)
				{
					case Action.Move:
						OnMove();
						break;

					case Action.Attack:
						OnAttack();
						break;

					case Action.Item:
						OnItem();
						break;

					case Action.Skill:
						OnSkill();
						break;
				}
				m_move = false;
				break;
		}
	}

	private void OnMoveSelect(Vector3Int[] massArray)
	//�I���ł���}�X�̕\��
	{
		foreach (GameObject mapList in m_gridmass.GridList)
		{
			Choice choice = mapList.GetComponent<Choice>();
			for (int i = 0; i < massArray.Length; i++)
			{
				// �������W�Ŕ�r 
				int destX = Mathf.RoundToInt(m_turnUnit.transform.position.x + massArray[i].x);
				int destZ = Mathf.RoundToInt(m_turnUnit.transform.position.z + massArray[i].z);
				if (SameGridPosition(mapList.transform.position, new Vector3(destX, mapList.transform.position.y, destZ)))
				{
					//�ړ��A�N�V���������A�ړ��\�}�X�ł���΃��X�g�ɉ�����
					if (choice.Possible && m_action == Action.Move)
					{
						choice.OnChoice();
						m_viewList.Add(mapList);
						break;
					}
				}
			}
		}
		m_move = true;
	}
	private void OnAttackSelect(Vector3Int[] massArray)
	//�I���ł���}�X�̕\��
	{
	
	}

	private void OnPick()
	//�ړ���̑I��
	{
		// �^�b�v�����ꏊ�ɃJ��������Ray���΂�
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			// �}�X�̑I��
			GameObject targetMass = hit.collider.gameObject;
			foreach(GameObject list in m_viewList)
			{
				//�����ꂽ�}�X���L���}�X�ł���ꍇ
				if(SameGridPosition(targetMass.transform.position,list.transform.position))
				{
					//�I���}�X���X�V���A���̃t�F�[�Y��
					m_targetPos = targetMass.transform.position;
					m_unitManager.NextPhase(UnitManager.Phase.Action);
				}
			}
		}
	}

	private void OnMove()
	{
		foreach (GameObject list in m_viewList)
		{
			// �������W�Ŕ�r
			Vector3 listPos = list.transform.position;
			if (SameGridPosition(listPos, m_targetPos))
			{
				// �ړ������������W�ɃX�i�b�v 
				m_turnUnit.transform.position = new Vector3(
					Mathf.Round(m_targetPos.x),
					Mathf.Round(m_turnUnit.transform.position.y),
					Mathf.Round(m_targetPos.z)
				);

				//�ړ��̉s���Ē�`�A�L���}�X�\�����O���Ď��̃t�F�[�Y��
				GridInvalid();
				return;
			}
		}
	}

	private void OnAttack()
	{
		
	}

	private void OnItem()
	{

	}

	private void OnSkill()
	{

	}

	private void OnRemove()
	{
		//�L���}�X�̕\�����O��
		foreach (GameObject list in m_viewList)
		{
			Choice choice = list.GetComponent<Choice>();
			choice.OnCancell();
		}
		m_viewList.Clear();
	}

	public bool SameGridPosition(Vector3 compare1, Vector3 compare2)
	{
		//��̍��W�������}�X����r
		//�ϐ����I����Ă邯�ǋC�ɂ��Ȃ��ł�
		return Mathf.RoundToInt(compare1.x) == Mathf.RoundToInt(compare2.x)
			&& Mathf.RoundToInt(compare1.z) == Mathf.RoundToInt(compare2.z);
	}

	public void SetAction(Action action)
	{
		m_action = action;
	}

	public void GridInvalid()
	{
	// ������}�X���ړ��\�}�X�ɖ߂��A�ړ���}�X���ړ��s�}�X�ɂ���
		foreach (GameObject grid in m_gridmass.GridList)
		{
			// �������W�Ŕ�r 
			if (SameGridPosition(grid.transform.position, m_turnUnit.transform.position))
			{
				grid.GetComponent<Choice>().SetPossible(true);
			}

			if (SameGridPosition(grid.transform.position, m_targetPos))
			{
				grid.GetComponent<Choice>().SetPossible(false);
			}
		}

		//�L���}�X�̕\�����O���Ď��̃t�F�[�Y��
		OnRemove();
		m_unitManager.NextPhase(UnitManager.Phase.End);
	}
}
