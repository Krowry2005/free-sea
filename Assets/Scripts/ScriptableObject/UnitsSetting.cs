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

		//移動可能範囲(初期化として周囲一マスのみ移動可能)
		public Vector3[] destination = {new(-1,0,1),new(0,0,1),new(1,0,1),new(-1,0,0),
											new(1,0,0),new(-1,0,-1),new(0,0,-1),new(1,0,-1)};
		//ステータス
		public string name;
		public int id;
		public int health;
		public int attack;
		public int defense;
		public int agility;
	}
}