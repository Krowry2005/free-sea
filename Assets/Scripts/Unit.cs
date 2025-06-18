using System.Linq;
using UnityEngine;
using System.Collections.Generic;
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
	Animator m_animator;

	string m_name;
	Sprite m_sprite;
	List<AttackSkill> m_attackSkillList = new();
	List<Skills> m_skillList = new();
	bool m_fly;
	int MaxHealth;
	int m_health;
	int MaxSp;
	int m_sp;
	int m_attack;
	int m_defense;
	int m_agility;

	public FriendLevel FriendLevel => m_friendLevel;
	public Sprite Sprite => m_sprite; 
	public string Name => m_name;
	public bool Fly => m_fly;
	public int SP => m_sp;
	public int HealthValue => m_health;
	public int AttackValue => m_attack;
	public int Agility => m_agility;

	private void Awake()
	{
		GameObject gameController;
		gameController = GameObject.FindGameObjectWithTag("GameController");
		m_turnManager = gameController. GetComponent<UnitManager>();
		m_animator = GetComponent<Animator>();

		//ユニットのデータを引き継ぐ
		UnitData unitData = unitsData.data.FirstOrDefault(unitSetting => unitSetting.id == m_dataId && unitSetting.friendLevel == m_friendLevel);
		m_name = unitData.name;
		Debug.Log(m_name);
		m_sprite = unitData.sprite;
		m_skillList.AddRange(unitData.GetSkill());
		m_attackSkillList.AddRange(unitData.GetAttackSkill());
		m_fly = unitData.fly;
		MaxHealth = unitData.health;
		MaxSp = unitData.sp;
		m_attack = unitData.attack;
		m_defense = unitData.defense;
		m_agility = unitData.agility;
	}

	private void Start()
	{
		//HPを最大値に
		m_health = MaxHealth;

		//MP
		m_sp = MaxSp;

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

	public List<AttackSkill> GetAttackSkill()
	{
		return m_attackSkillList;
	}

	public List<Skills> GetSkill()
	{
		return m_skillList; 
	}

	public void Damage(int damage)
	{
		if (m_health <= 0) return;
		m_health -= Calculation(damage);
		Debug.Log(gameObject.name+"は"+Calculation(damage)+"ダメージを受けた" +"残りは"+m_health);
	}

	public void OnDeath()
	{
		m_turnManager.DeleteList(gameObject);
		//m_animator.SetTrigger("Death");
		//死亡メッセージ
		Debug.Log(m_name + "は倒れた");
		Destroy(gameObject,5);
	}

	public int Calculation(int damage)
	{
		//防御力以下のダメージは1にする
		if (damage <= m_defense) return 1;
		return damage -= m_defense;
	}

	public void DirectionReset()
	{
		gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
	}
}

