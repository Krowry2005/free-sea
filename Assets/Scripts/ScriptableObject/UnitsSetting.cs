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

		//敵、味方、中立
		public FriendLevel friendLevel;

		//見た目の画像,アイコンも保持
		public Sprite sprite;

		//持っているスキル
		[SerializeField]
		Skills[] skill;

		[SerializeField]
		AttackSkill[] attacks;

		//浮いているキャラクターか
		public bool fly;

		//ステータス
		public string name;
		public int id;
		public int health;
		public int mp;
		public int attack;
		public int defense;
		public int agility;

		public Skills[] GetSkill()
			{ return skill; }

		public AttackSkill[] GetAttackSkill()
			{ return attacks; }
	}
}