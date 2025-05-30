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
		Select,
		Action,
		End,
		Length,
	}

	Phase m_phase;

	public List<GameObject> UnitList = new List<GameObject>();
	public Phase GetPhase => m_phase;
	
	private void Start()
	{
		
	}

	private void Update()
	{

	}

	public Phase NextPhase(Phase phase)
	{
		return m_phase = phase;
	}

	public void SetList(GameObject list)
	{
		//���X�g�ɉ����đ��x���ɕ��בւ���
		UnitList.Add(list);
		SortList();

		foreach (GameObject unit in UnitList)
		{
			Unit m_unit;
			unit.TryGetComponent(out m_unit);
			Debug.Log(m_unit.UnitName());
		}
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
