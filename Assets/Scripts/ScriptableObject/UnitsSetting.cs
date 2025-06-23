using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObject/UnitData")]
public class UnitsSetting : ScriptableObject
{
	public List<UnitData> data = new();

	[Serializable]
	public class UnitData
	{
		public enum FriendLevel
		{
			Enemy,
			Neutral,
			Ally,
		}

		//�G�A�����A����
		[SerializeField]
		FriendLevel friendLevel;

		//�����ڂ̉摜,�A�C�R�����ێ�
		[SerializeField]
		Sprite sprite;

		//�X�e�[�^�X
		[SerializeField]
		string name;

		[SerializeField, TextArea(1, 4)]
		string information = "1\n2\n3\n4";

		[SerializeField]
		int id;

		[SerializeField]
		int health;

		[SerializeField]
		int sp;

		[SerializeField]
		int attack;

		[SerializeField]
		int defense;

		[SerializeField]
		int agility;

		//�����Ă���X�L��
		[SerializeField]
		private List<Skill> skillList ;

		//�U���X�L��
		[SerializeField]
		private List<SkillAttack> attackSkillList;

		//�����Ă���L�����N�^�[��
		[SerializeField]
		bool fly;

		public FriendLevel GetFriendLevel()
		{ return friendLevel; }

		public String GetName()
		{ return name;}

		public String GetInformation()
		{ return information; }

		public Sprite GetSprite()
		{ return sprite;}

		public int GetID()
		{ return id;}

		public int GetHealth()
		{ return health;}

		public int GetSkillPoint()
		{ return sp; }

		public int GetAttack()
		{ return attack;}

		public int GetDefense()
		{ return defense;}

		public int GetAgillity()
		{ return agility;}

		public List<Skill> GetSkill()
		{ return skillList; }

		public List<SkillAttack> GetAttackSkill()
		{return attackSkillList;}

		public bool GetFly()
		{return fly;}
	}
}