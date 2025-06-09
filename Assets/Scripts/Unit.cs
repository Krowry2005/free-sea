using System.Linq;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;
using static UnitsSetting;
using static UnitsSetting.UnitData;

public class Unit : MonoBehaviour
{
	[SerializeField]
	UnitsSetting unitsData;

	[SerializeField]
	int m_dataId;

	[SerializeField]
	FriendLevel m_friendLevel;

	UnitManager m_turnManager;
	GameController m_gameController;
	Animator m_animator;

	string m_name;
	Sprite m_sprite;
	Vector3Int[] m_destination;
	Vector3Int[] m_attackPos;
	AttackWay m_attackWay;
	int MaxHealth;
	int m_health;
	int m_attack;
	int m_defense;
	int m_agility;
	int m_magnification;

	public FriendLevel FriendLevel => m_friendLevel;
	public Sprite Sprite => m_sprite; 
	public Vector3Int[] Destination => m_destination;
	public Vector3Int[] AttackPos => m_attackPos;

	public int AttackValue => m_attack;
	public int HealthValue => m_health;
	public int Agility => m_agility;

	private void Awake()
	{
		GameObject gameController;
		gameController = GameObject.FindGameObjectWithTag("GameController");
		m_gameController = gameController.GetComponent<GameController>();
		m_turnManager = gameController. GetComponent<UnitManager>();
		m_animator = GetComponent<Animator>();

		//ユニットのデータを引き継ぐ
		UnitData unitData = unitsData.data.FirstOrDefault(unitSetting => unitSetting.id == m_dataId && unitSetting.friendLevel == m_friendLevel);
		m_name = unitData.name;
		m_sprite = unitData.sprite;
		m_attackWay = unitData.attackWay;
		m_destination = unitData.destination;
		MaxHealth = unitData.health;
		m_attackPos = unitData.attackPos;
		m_attack = unitData.attack;
		m_defense = unitData.defense;
		m_agility = unitData.agility;
		m_magnification = unitData.magnification;
	}

	private void Start()
	{
		//HPを最大値に
		m_health = MaxHealth;

		//ユニットリストに加える
		m_turnManager.SetList(gameObject);

		//召喚時、足元を移動不可に
		GridMass gridmass = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMass>();
		foreach(GameObject grid in gridmass.GridList)
		{
			//ユニットの足元には移動不可
			if (Mathf.RoundToInt(grid.transform.position.x) == Mathf.RoundToInt(transform.position.x)
				&& Mathf.RoundToInt(grid.transform.position.z) == Mathf.RoundToInt(transform.position.z))
			{
				grid.GetComponent<Choice>().SetPossible(false);
			}
		}
	}

	public string UnitName()
	{
		return m_name;
	}

	public int Magnification()
	{
		switch (m_attackWay)
		{
			case AttackWay.Attack:
				return Mathf.CeilToInt(m_attack * m_magnification / 100);

			case AttackWay.Defense:
				return Mathf.CeilToInt(m_defense * m_magnification / 100);

			case AttackWay.MaxHP:
				return Mathf.CeilToInt(MaxHealth * m_magnification / 100);

			case AttackWay.Agility:
				return Mathf.CeilToInt(m_agility * m_magnification / 100);
		}
		return 0;
	}

	public void Damage(int damage)
	{
		if (m_health <= 0) return;
		m_health -= Calculation(damage);
	}

	public void OnDeath()
	{
		m_turnManager.DeleteList(gameObject);
		m_animator.SetTrigger("Death");

		//死亡メッセージ
		Debug.Log(m_name + "は倒れた");
	}

	public int Calculation(int damage)
	{
		//防御力以下のダメージは1にする
		if (damage <= m_defense) damage = 1;
		return damage -= m_defense;
	}

	public void ResetStatus()
	{
		//ステータスリセット
		UnitData unitData = unitsData.data.FirstOrDefault(unitSetting => unitSetting.id == m_dataId && unitSetting.friendLevel == m_friendLevel);
		m_name = unitData.name;
		m_health = unitData.health;
		m_attack = unitData.attack;
		m_defense = unitData.defense;
		m_agility = unitData.agility;
	}
}

