
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Skill", menuName = "CreateSkill")]
public class Skills : ScriptableObject
{
	public enum Type
	{
		Attack,	
		Move,
		Guard,
		Item,
		Buff,
		DeBuff,
		Damage,
		Heal,
	}

	//�X�L���̃^�C�v
	[SerializeField]
	private Type skillType;

	//�X�L����ID
	[SerializeField]
	private int skillId;

	//�����̃X�L����
	[SerializeField]
	private string kanjiName = "";

	//�Ђ炪�Ȃ̃X�L����
	[SerializeField]
	private string hiraganaName = "";

	//����MP 
	[SerializeField]
	int mp;

	//���
	[SerializeField]
	private string information = "";

	//�͈�
	[SerializeField]
	Vector3Int[] Range = { new (-1,0,1),new (0,0,1),new (1,0,1),new (-1,0,0),new (1,0,0),new (-1,0,-1),new (0,0,-1),new (1,0,-1)};

	//�@�g�p�҂̃G�t�F�N�g
	[SerializeField]
	private GameObject skillUserEffect = null;

	//�@�X�L���̎�ނ�Ԃ�
	public Type GetSkillType()
	{
		return skillType;
	}

	//�X�L����ID
	public int GetID()
	{
		return skillId;
	}

	//�@�X�L���̖��O��Ԃ�
	public string GetKanjiName()
	{
		return kanjiName;
	}

	//�@�X�L���̕������̖��O��Ԃ�
	public string GetHiraganaName()
	{
		return hiraganaName;
	}

	//����MP
	public int GetMP()
	{
		return mp;
	}

	//�@�X�L������Ԃ�
	public string GetInformation()
	{
		return information;
	}

	//�͈͂�Ԃ�
	public Vector3Int[] GetRange()
	{
		return Range;
	}

	//�@�g�p�҂̃G�t�F�N�g��Ԃ�
	public GameObject GetSkillUserEffect()
	{
		return skillUserEffect;
	}
}