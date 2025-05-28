using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObject")]
public class UnitsData : ScriptableObject
{
	public List<UnitData> m_data = new List<UnitData>();

	[Serializable]
	public class UnitData
	{
		public enum FriendLevel
		{
			Enemy,
			Neutral,
			Ally,
		}

		public FriendLevel m_friendLevel;
		public string m_name;
		public int m_id;
		public int m_health;
		public int m_attack;
		public int m_defense;
	}
}