using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObject")]
public class UnitsData : ScriptableObject
{
	public List<UnitData> data = new List<UnitData>();

	[Serializable]
	public class UnitData
	{
		public enum FriendLevel
		{
			Enemy,
			Neutral,
			Ally,
		}

		public FriendLevel friendLevel;
		public Sprite sprite;
		public string name;
		public int id;
		public int health;
		public int attack;
		public int defense;
	}
}