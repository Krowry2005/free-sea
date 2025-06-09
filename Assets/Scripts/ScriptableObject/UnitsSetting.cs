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

		public enum AttackWay
		{
			Attack,
			MaxHP,
			Defense,
			Agility,
		}

		//�G�A�����A����
		public FriendLevel friendLevel;

		//�_���[�W�̎Q�Ɣ{��
		public AttackWay attackWay;

		//�����ڂ̉摜,�A�C�R�����ێ�
		public Sprite sprite;

		//�ړ��\�͈�(�������Ƃ��Ď��͈�}�X�݈̂ړ��\)
		public Vector3Int[] destination = {new(-1,0,1),new(0,0,1),new(1,0,1),new(-1,0,0),
											new(1,0,0),new(-1,0,-1),new(0,0,-1),new(1,0,-1)};

		//�ʏ�U���͈�
		public Vector3Int[] attackPos = { new(-1,0,1),new(0,0,1),new(1,0,1),new(-1,0,0),
											new(1,0,0),new(-1,0,-1),new(0,0,-1),new(1,0,-1)};

		//�X�e�[�^�X
		public string name;
		public int id;
		public int health;
		public int attack;
		public int defense;
		public int agility;
		public int magnification;
	}
}