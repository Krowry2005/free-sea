using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevelPhysics;
using UnityEngine.Rendering.Universal;

public class UnitAction : MonoBehaviour
{
	public enum Action
	{
		Choice,
		Move,
		Attack,
		Item,
		Skill,
		Information,

		Length,
	}

	[SerializeField]
	GameObject m_mapBlock;

	Action m_action;

	UnitManager m_unitManager;
	GridMass m_gridmass;

	GameObject m_turnUnit;

	List<GameObject> m_viewList = new();
	bool m_move;
	
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
				if (m_move) return;
				switch (m_action)
				{
					case Action.Move:
						
						
						break;

					case Action.Attack:
						
						
						break;

					case Action.Item:

						break;

					case Action.Skill:

						break;

					case Action.Information:
						//���ׂẴ}�X���Ώ�
						foreach(GameObject posList in m_gridmass.GridList)
						{
							Choice choice = posList.GetComponent<Choice>();
							choice.OnChoice();
							m_viewList.Add(posList);
							m_unitManager.SetPhase(UnitManager.Phase.Action);
						}
						break;
				}
				break;

			case UnitManager.Phase.Action:
				if (Input.GetMouseButtonDown(0))
				{
					if (OnPick() != null)
					{
						GameObject targetMass = OnPick();
						OnAction(targetMass.transform.position);
					}
					else
					{
						//�Ӗ��킩���Ƃ��w�肳�ꂽ��Z���N�g�t�F�[�Y�ɖ߂�
						m_unitManager.SetPhase(UnitManager.Phase.Select);
						m_action = Action.Choice;
						OnRemove();
					}
				}
				break;
		}
	}

	private void OnDisplay(Vector3Int[] massArray, bool possible, bool fly) 
	//�I���ł���}�X�̕\�� (�I���\�͈́A�ړ��s�}�X��I���\��)
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
					//�I��s�}�X���A�I��s�\���Ȃ炠����߂�
					if (!choice.Possible && !possible) continue;
					choice.OnChoice();
					m_viewList.Add(mapList);
					m_unitManager.SetPhase(UnitManager.Phase.Action);
					break;
				}
			}
		}
	}

	private GameObject OnPick()
	{
		// �^�b�v�����ꏊ�ɃJ��������Ray���΂�
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			//���̂Ƃ���}�X�ȊO�ɓ����蔻����Ȃ����ǈꉞ
			if(hit.collider.gameObject.tag == "Grid")
			{
				// �}�X�̑I��
				return hit.collider.gameObject;
			}
			return null;
		}
		else return null;
	}

	private void OnAction(Vector3 targetPos)
	{
		foreach (GameObject viewList in m_viewList)
		{
			//�����ꂽ�}�X���L���}�X�ł���ꍇ
			if (SameGridPosition(targetPos, viewList.transform.position))
			{
				switch (m_action)
				{
					case Action.Move:
						OnMove(targetPos);
						break;

					case Action.Attack:
						OnAttack(targetPos);
						break;

					case Action.Item:

						break;

					case Action.Skill:

						break;

					case Action.Information:
						foreach (GameObject unitList in m_unitManager.UnitList)
						{
							if(SameGridPosition(targetPos,unitList.transform.position))
							{
								Debug.Log(unitList.transform.position);
							}
						}
						break;
				}
				break;
			}
		}
	}

	private void OnMove(Vector3 targetPos)
	{
		foreach (GameObject list in m_viewList)
		{
			// �������W�Ŕ�r
			if (SameGridPosition(list.transform.position, targetPos))
			{
				//�ړ��̉s���Ē�`�A�L���}�X�\�����O�X
				GridInvalid(targetPos);

				// �ړ������������W�ɃX�i�b�v 
				m_turnUnit.transform.position = new Vector3(
					Mathf.RoundToInt(targetPos.x),
					m_turnUnit.transform.position.y,	//�����͂܂Ƃ߂��炾��
					Mathf.RoundToInt(targetPos.z)
				);

				m_unitManager.SetPhase(UnitManager.Phase.End);
				m_action = Action.Choice;
				return;
			}
		}
	}

	private void OnAttack(Vector3 targetPos)
	{
		//�I�������}�X�ɂ���L���������ׂĒ��o
		var DamageUnit = m_unitManager.UnitList.Where(unit => SameGridPosition(unit.transform.position, targetPos));
		if (DamageUnit.Count() > 0)
		{
			foreach (GameObject list in DamageUnit)
			{
				//�_���[�W
				Unit unit = list.GetComponent<Unit>();

				//�t�����h���[�t�@�C�A�̋֎~
				if (unit.FriendLevel != m_turnUnit.GetComponent<Unit>().FriendLevel)
				{
					Debug.Log("HIt?");
					unit.Damage(m_turnUnit.GetComponent<Unit>().AttackValue * m_turnUnit.GetComponent<Unit>().GetAttackSkills()[0].GetMagnification() / 100);
				}
				else
				{
					Debug.Log("miss");

				}
			}
		}

		//�I�������}�X�ɉ������Ȃ�������ǂ��ɂ�����
		else
		{
			Debug.Log("miss");
		}

		OnRemove();
		m_unitManager.SetPhase(UnitManager.Phase.End);
		m_action = Action.Choice;
	}

	private void OnItem()
	{

	}

	private void OnSkill()
	{

	}

	public void OnRemove()
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

	public void GridInvalid(Vector3 targetPos)
	{
	// ������}�X���ړ��\�}�X�ɖ߂��A�ړ���}�X���ړ��s�}�X�ɂ���
		foreach (GameObject grid in m_gridmass.GridList)
		{
			// �������W�Ŕ�r 
			//���Ƃ������W�̃}�X���ړ��\�ɂ���
			if (SameGridPosition(grid.transform.position, m_turnUnit.transform.position))
			{
				grid.GetComponent<Choice>().SetPossible(true);
			}

			//�ړ�����ړ��s�ɂ���
			if (SameGridPosition(grid.transform.position,targetPos))
			{
				grid.GetComponent<Choice>().SetPossible(false);
			}
		}

		//�L���}�X�̕\�����O���Ď��̃t�F�[�Y��
		OnRemove();

	}
}
