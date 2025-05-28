using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using static UnitsData.UnitData;

public class Unit : MonoBehaviour
{
	[SerializeField]
	UnitsData unitsData;


	FriendLevel m_friendLevel;
	string m_name;
	int m_id;
	int m_health;
	int m_attack;
	int m_defense;


	private void Start()
	{
		int id = 0;
		Debug.Log(unitsData.m_data[id].m_name);
	}

}

