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
		public FriendLevel friendLevel;

		//�����ڂ̉摜,�A�C�R�����ێ�
		public Sprite sprite;

		//�����Ă���X�L��
		[SerializeField]
		private List<Skills> skillList ;

		//�U���X�L��
		[SerializeField]
		private List<AttackSkill> attackSkillList;

		//�����Ă���L�����N�^�[��
		public bool fly;

		//�X�e�[�^�X
		public string name;
		public int id;
		public int health;
		public int sp;
		public int attack;
		public int defense;
		public int agility;

		public List<Skills> GetSkill()
		{ return skillList; }

		public List<AttackSkill> GetAttackSkill()
		{return attackSkillList;}
	}
}