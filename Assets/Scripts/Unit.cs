using System.Linq;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using static UnitsData.UnitData;

public class Unit : MonoBehaviour
{
	[SerializeField]
	UnitsData unitsData;

	[SerializeField]
	int m_dataId;

	[SerializeField]
	FriendLevel m_friendLevel;

	string m_name;
	int m_id;
	int m_health;
	int m_attack;
	int m_defense;

	public int HPValue => m_health;

	private void Start()
	{
		var slilmeData = unitsData.data.FirstOrDefault(unitData => unitData.id == m_dataId && unitData.friendLevel == m_friendLevel);
		m_name = slilmeData.name;
		m_health = slilmeData.health;
		m_attack = slilmeData.attack;
		m_defense = slilmeData.defense;
		print(m_friendLevel);
		print(m_name);
		print(m_id);
		print(m_health);
		print(m_attack);
		print(m_defense);
	}

	public void OnDamage(int damage)
	{
		if (m_health <= 0) return;
		m_health -= Calculation(damage);
	}

	public int Calculation(int damage)
	{
		//–hŒä—ÍˆÈ‰º‚Ìƒ_ƒ[ƒW‚Í1‚É‚·‚é
		if (damage < m_defense) damage = 1;
		return damage -= m_defense;
	}
}

