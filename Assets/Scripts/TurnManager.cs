using System;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public enum Phase
	{
		Start,
		Enemy,
		Select,
		Action,
		End,
		Length,
	}

	Phase m_phase;
	GameObject m_turnUnit;
	bool m_turnStart;
	bool m_buttleEnd;
	int m_round;

	public GameObject TurnUnit => m_turnUnit;
	public List<GameObject> UnitList = new List<GameObject>();
	public List<GameObject> ReserveUnit;
	public Phase GetPhase => m_phase;
	public int Rount => m_round; 
	
	private void Start()
	{
		m_buttleEnd = false;
		m_turnUnit = UnitList.First();
		m_phase = Phase.Start;
		m_round = 0;
	}

	private void Update()
	{
		switch (m_phase)
		{
			case Phase.Start:
				Debug.Log(m_turnUnit.name);
				break;

			case Phase.Enemy:
				//���͉��u���Ƃ��ă^�[���I��
				NextPhase(Phase.End);
				break;

			case Phase.End:
				//���X�g�̐擪�v�f���T���̃��X�g�ɃR�s�[����
				ReserveUnit.Add(UnitList.First());

				//���X�g�̍ŏ��̗v�f���폜
				UnitList.RemoveAt(0);

				//���X�g�̐V�����ŏ��̗v�f�����̃^�[���ɂ���
				m_turnUnit = UnitList.First();

				//�o�t�̏���

				NextPhase(Phase.Start);
				break;
		}
		//�s���������Z�b�g���A���E���h������
		UnitList.AddRange(UnitList);
		SortList();
		m_round++;
	}

	public Phase NextPhase(Phase phase)
	{
		//�ʃX�N���v�g���ł��t�F�[�Y���������悤�ɂȂ�
		return m_phase = phase;
	}

	public void SetList(GameObject list)
	{
		//���X�g�ɉ����đ��x���ɕ��בւ���
		UnitList.Add(list);
		SortList();
	}

	public void DeleteList(GameObject list)
	{
		UnitList.Remove(list);
		SortList();
	}

	public void SortList()
	{
		//	//GameObject�^�̃��X�g�ŁA���X�g���̃Q�[���I�u�W�F�N�g�������Ă�Unit���Ă����X�N���v�g��Agility���Ƀ\�[�g������
		//	//�킩��񎖁A�������s���邽�߂�UnitList.Sort()�̒��ɉ����ق肱�߂΂����̂��AGameObject�^���X�g�Ȃ̂����A���̏ꍇ���̐��l����Ƀ\�[�g�����̂�

		// GameObject��SampleScript�̃y�A���ɍ���Ă����iGetComponent��1�񂾂��j
		var objectScriptPairs = UnitList
			.Select(unit => new { unit, script = unit.GetComponent<Unit>() })
			.ToList();

		// SampleScript.hp ���g���č~���\�[�g�i�l���傫�����j
		objectScriptPairs.Sort((a, b) => b.script.Agility.CompareTo(a.script.Agility));

		// �\�[�g���ʂ���GameObject�̃��X�g�����ɍč\��
		UnitList = objectScriptPairs.Select(pair => pair.unit).ToList();
	}
}
