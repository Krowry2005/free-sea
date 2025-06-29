using System.Collections.Generic;
using System.Linq;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using System;

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

	[SerializeField]
	GameObject m_uiController;

	Action m_action;

	UnitManager m_unitManager;
	GridMass m_gridmass;
	GameObject m_turnUnit;
	Skill m_usedSkill;
	Information m_information;

	List<GameObject> m_rangeViewList = new();
	List<GameObject> m_extentViewList = new();

	Vector3Int m_activationPos;
	Quaternion m_turnUnitRotation;

	bool m_move;
	bool m_select;
	bool m_actionAproval;

	public Action GetAction => m_action;
	
	private void Awake()
	{
		m_unitManager = GetComponent<UnitManager>();
		m_gridmass = m_mapBlock.GetComponent<GridMass>();
		m_information = m_uiController.GetComponent<Information>();
	}

	private void Start()
	{
		m_move = false;
		m_action = Action.Choice;
		m_select = true;
		m_actionAproval = true;
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
					case Action.Choice:
						m_actionAproval = true;
						m_select = true;
						break;

					case Action.Move:
						//�ړ��͈͂̕\��1���
						OnDisplay(m_turnUnit.GetComponent<Unit>().GetSkill()[0].GetRange(), false);
						m_unitManager.SetPhase(UnitManager.Phase.Action);
						break;

					case Action.Attack:
						//�U���͈͂̕\��
						OnDisplay(m_turnUnit.GetComponent<Unit>().GetSkill()[1].GetRange(), true);
						m_unitManager.SetPhase(UnitManager.Phase.Action);
						break;

					case Action.Information:
						//���̎擾
						foreach(GameObject unitList in m_unitManager.UnitList)
						{
							foreach(GameObject posList in m_gridmass.GridList)
							{
								if(SameGridPosition(unitList.transform.position,posList.transform.position))
								{
									Choice choice = posList.GetComponent<Choice>();
									choice.OnChoice(true);
									m_actionAproval = false;
									m_rangeViewList.Add(posList);
									m_unitManager.SetPhase(UnitManager.Phase.Action);
									break;
								}
							}
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
						OnAction( new Vector3Int(Mathf.RoundToInt(targetMass.transform.position.x),
													Mathf.RoundToInt(targetMass.transform.position.y),
														Mathf.RoundToInt(targetMass.transform.position.z)));
					}
					else
					{
						if (!m_actionAproval) return;
						//�Ӗ��킩���Ƃ��w�肳�ꂽ��Z���N�g�t�F�[�Y�ɖ߂�
						m_unitManager.SetPhase(UnitManager.Phase.Select);
						m_action = Action.Choice;
						m_information.DefText();
						OnRemove();
					}
				}
			break;
		}
	}

	public void OnAction(Vector3Int targetPos)
	{
		List<GameObject> viewList = new();
		if(m_select)
		{
			viewList.AddRange(m_rangeViewList);
		}
		else
		{
			viewList.AddRange(m_extentViewList);
		}

		// �R�s�[���g���Ĉ��S�Ƀ��[�v
		// ���ʑI��͈͂Ǝw�肳�ꂽ�}�X�������ł��邩�𑖍�
		foreach (GameObject viewer in viewList)
		{
			//�����Ȃ炻�̃}�X���N�_�Ɍ��ʔ͈͂�\���A�Z���N�g�t�F�[�Y�ɕԂ�
			if (SameGridPosition(viewer.transform.position, targetPos))
			{
				switch (m_action)
				{
					case Action.Move:
						if (m_select)
						{
							//���ڂ̑I���ł͌��ʔ͈͂̕\������
							OnExtent(m_turnUnit.GetComponent<Unit>().GetSkill()[0].GetExtent(), targetPos);
						}
						else
						{
							//2��ڂŎ��s
							OnMove(targetPos);
						}
						break;

					case Action.Attack:
						if (m_select)
						{
							//���ڂ̑I���ł͌��ʔ͈͂̕\������
							OnExtent(m_turnUnit.GetComponent<Unit>().GetAttackSkill()[0].GetExtent(), targetPos);
						}
						else
						{
							//2��ڂŎ��s
							OnAttack(m_turnUnit.GetComponent<Unit>().GetAttackSkill()[0], targetPos);
						}
						break;

					case Action.Skill:
						if (m_select)
						{
							//���ʔ͈͑I��
							OnExtent(m_usedSkill.GetExtent(), targetPos);
						}
						else
						{
							//���s
							Debug.Log(m_usedSkill + "�����I�I");
							SkillExecution(m_usedSkill, m_activationPos);
						}
						break;

					case Action.Information:
						//���擾�A�^�[������Ȃ�
						for (int i = 0; i <  m_unitManager.UnitList.Count; i++)
						{
							if (SameGridPosition(targetPos, m_unitManager.UnitList[i].transform.position))
							{
								m_information.OnInformation(m_unitManager.UnitList[i].GetComponent<Unit>());
							}
						}
						break;
				}
			}
		}
	}

	public void OnDisplay(Vector3Int[] massArray, bool possible)
	//�I���ł���}�X�̕\�� (�I���\�͈́A�ړ��s�}�X��I���\��)
	{
		foreach (GameObject mapList in m_gridmass.GridList)
		{
			for (int i = 0; i < massArray.Length; i++)
			{
				// �������W�Ŕ�r 
				int destX = Mathf.RoundToInt(m_turnUnit.transform.position.x + massArray[i].x);
				int destZ = Mathf.RoundToInt(m_turnUnit.transform.position.z + massArray[i].z);
				if (SameGridPosition(mapList.transform.position, new Vector3(destX, mapList.transform.position.y, destZ)))
				{
					Choice choice = mapList.GetComponent<Choice>();
					//�I��s�}�X���A�I��s�\���Ȃ炠����߂�
					if (!choice.Possible && !possible) continue;
					choice.OnChoice(true);
					m_select = true;
					m_rangeViewList.Add(mapList);
					break;
				}
			}
		}
	}

	public void OnExtent(Vector3Int[] extent, Vector3 ActivationPos)
	//�X�L���A�A�C�e�����̌��ʔ͈͂̕\��
	{
		//Range�͈͂̌����ڍ폜
		foreach(GameObject viewList in m_rangeViewList)
		{
			if(!SameGridPosition(viewList.transform.position,ActivationPos))
			{
				viewList.GetComponent<Choice>().OnChoice(false);
			}
		}

		m_activationPos = new Vector3Int(
			Mathf.RoundToInt(ActivationPos.x),
			Mathf.RoundToInt(ActivationPos.y),
			Mathf.RoundToInt(ActivationPos.z));

		//���ʔ͈͂̌����ڂ𐶐�
		for (int i = 0; i < extent.Length; i++)
		{
			foreach (GameObject mapList in m_gridmass.GridList)
			{
				int destX = Mathf.RoundToInt(ActivationPos.x + extent[i].x);
				int destZ = Mathf.RoundToInt(ActivationPos.z + extent[i].z);
				// �������W�Ŕ�r 
				if (SameGridPosition(mapList.transform.position, new Vector3(destX, mapList.transform.position.y, destZ)))
				{
					Choice choice = mapList.GetComponent<Choice>();
					choice.OnExtent(true);
					m_select = false;
					m_extentViewList.Add(mapList);
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
			if (hit.collider.gameObject.tag == "Grid")
			{
				// �}�X�̑I��
				return hit.collider.gameObject;
			}
			return null;
		}
		else return null;
	}

	private void OnMove(Vector3 targetPos)
	{
		foreach (GameObject list in m_extentViewList)
		{
			// �������W�Ŕ�r
			if (SameGridPosition(list.transform.position, targetPos))
			{
				//�ړ��̉s���Ē�`�A�L���}�X�\�����O�X
				GridInvalid(targetPos);
				m_turnUnit.GetComponent<Unit>().MoveTo(targetPos);
				m_unitManager.SetPhase(UnitManager.Phase.End);
				m_action = Action.Choice;
				return;
			}
		}
	}

	private void OnAttack(SkillAttack skill,Vector3Int targetPos)
	{
        Quaternion originalRotation = m_turnUnit.transform.rotation;
		List<Vector3Int> extentList = new();
		foreach (Vector3Int extent in skill.GetExtent())
		{
			extentList.Add(extent + targetPos);
		}

        //�I�������}�X�ɂ���L���������ׂĒ��o
        for (int i = 0; i < extentList.Count(); i++)
		{
			//�U���͈͓��̃L�����𒊏o
			var DamageUnitList = m_unitManager.UnitList.Where(unit => SameGridPosition(unit.transform.position,extentList[i]));
			if (DamageUnitList.Count() > 0)
			{
				Unit attackUnit = m_turnUnit.GetComponent<Unit>();
				for (int num = 0; num < skill.GetAttackNumTime();num++)
				{
					foreach (GameObject damageUnit in DamageUnitList)
					{
						Unit hitUnit = damageUnit.GetComponent<Unit>();
						//�t�����h���[�t�@�C�A�̋֎~
						if (hitUnit.FriendLevel != attackUnit.FriendLevel)
						{
                            Animator animator = m_turnUnit.GetComponent<Animator>();
							//�P�̍U���Ȃ�^�[�Q�b�g�̂ق�������
							if (skill.GetSingleTarget()) m_turnUnit.transform.LookAt(damageUnit.transform);

							//�G�t�F�N�g����
							if (skill.GetSkillUserEffect() != null) Instantiate(skill.GetSkillUserEffect(), m_turnUnit.transform);

							//SE���Đ�����
							if (skill.GetAudioClip() != null) SoundEffect.Play2D(skill.GetAudioClip(), 0.2f);

							//�_���[�W����
							m_uiController.GetComponent<Explanation>().TakeDamage(hitUnit,attackUnit, hitUnit.Damage(attackUnit.AttackValue * skill.GetMagnification()));

                            //�A�j���[�V�����Đ�
                            animator.SetTrigger(skill.GetKanjiName());

							//�A�j���[�V�����Đ����I������A������߂��Ď��̃t�F�[�Y��
							ObservableStateMachineTrigger trigger = animator.GetBehaviour<ObservableStateMachineTrigger>();

                            trigger.OnStateExitAsObservable()
                             .Subscribe((ObservableStateMachineTrigger.OnStateInfo onStateInfo) =>
                             {
                                 Observable.Timer(TimeSpan.FromMilliseconds(100))
                                 .Subscribe(__ =>
                                 {
                                     m_turnUnit.transform.rotation = originalRotation;
                                     m_unitManager.SetPhase(UnitManager.Phase.End);
                                 }).AddTo(this);

                             }).AddTo(this);
							break;
                        }
						//�����U���������̓~�X�\���o���Ď��̃^�[����
                        else
						{
							m_uiController.GetComponent<Explanation>().MissDamage();
                            m_unitManager.SetPhase(UnitManager.Phase.End);
                        }
					}
				}
			}

            else
            {
                m_uiController.GetComponent<Explanation>().MissDamage();
                m_unitManager.SetPhase(UnitManager.Phase.End);
            }
            OnRemove();
			m_action = Action.Choice;
		}
	}

	public void OnSkill(Skill usedSkill)
	{
		m_usedSkill = usedSkill;
		OnDisplay(usedSkill.GetRange(), usedSkill.GetImpossibleMass());
		m_unitManager.SetPhase(UnitManager.Phase.Action);
	}

	public void SkillExecution(Skill usedSkill,Vector3Int targetPos)
	{
		SkillAttack usedSkillAttack = m_turnUnit.GetComponent<Unit>().GetAttackSkill().FirstOrDefault(attackSkill => attackSkill.GetID() == usedSkill.GetID());
		if(usedSkill.GetSP() > m_turnUnit.GetComponent<Unit>().SP)
		{
			//choice�ɖ߂�
			m_select = true;
			m_action = Action.Choice;
			m_unitManager.SetPhase(UnitManager.Phase.Select);
			m_uiController.GetComponent<Explanation>().NotEnoughSP(usedSkill);
			return;
		}
		else
		{
			m_turnUnit.GetComponent<Unit>().usedSP(usedSkill.GetSP());
		}

		switch (usedSkill.GetSkillType())
		{ 
			case Skill.Type.Attack:
				OnAttack(usedSkillAttack, targetPos);
				break;

			case Skill.Type.Move:
				OnMove(targetPos);
				break;

			case Skill.Type.Guard:

				break;

			case Skill.Type.Buff:

				break;

			case Skill.Type.DeBuff:

				break;

			case Skill.Type.Heal:

				break;

			case Skill.Type.Item:

				break;
		}
	}

	public void OnRemove()
	{
		//���ʎg�p�͈̓}�X�̕\�����O��
		foreach (GameObject list in m_rangeViewList)
		{
			Choice choice = list.GetComponent<Choice>();
			choice.OnChoice(false);
			choice.OnExtent(false);
		}

		//���ʔ����͈̓}�X�̕\�����O��
		foreach (GameObject list in m_extentViewList)
		{
			Choice choice = list.GetComponent<Choice>();
			choice.OnChoice(false);
			choice.OnExtent(false);
		}
		m_rangeViewList.Clear();
		m_extentViewList.Clear();
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
