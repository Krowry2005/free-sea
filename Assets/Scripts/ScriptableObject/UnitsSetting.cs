using System;
using System.Collections.Generic;
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

		//�ړ��\�͈�


		//�X�e�[�^�X
		public string name;
		public int id;
		public int health;
		public int attack;
		public int defense;
		public int agility;
	}
}