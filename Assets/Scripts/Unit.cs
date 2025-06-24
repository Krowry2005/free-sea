using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
	string m_information;
	Sprite m_sprite;
	List<SkillAttack> m_attackSkillList = new();
	List<Skill> m_skillList = new();
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
	public int Id => m_dataId;
	public string Information => m_information;
	public bool Fly => m_fly;
	public int SP => m_sp;
	public int HealthValue => m_health;
	public int DefeseValue => m_defense;
	public int AttackValue => m_attack;
	public int Agility => m_agility;

	private void Awake()
	{
		GameObject gameController;
		gameController = GameObject.FindGameObjectWithTag("GameController");
		m_turnManager = gameController. GetComponent<UnitManager>();
		m_animator = GetComponent<Animator>();

		//���j�b�g�̃f�[�^�������p��
		UnitData unitData = unitsData.data.FirstOrDefault(unitSetting => unitSetting.GetID() == m_dataId && unitSetting.GetFriendLevel() == m_friendLevel);
		m_name = unitData.GetName();
		m_information = unitData.GetInformation();
		m_sprite = unitData.GetSprite();
		m_skillList.AddRange(unitData.GetSkill());
		m_attackSkillList.AddRange(unitData.GetAttackSkill());
		m_fly = unitData.GetFly();
		MaxHealth = unitData.GetHealth();
		MaxSp = unitData.GetSkillPoint();
		m_attack = unitData.GetAttack();
		m_defense = unitData.GetDefense();
		m_agility = unitData.GetAgillity();
	}

	private void Start()
	{
		//HP���ő�l��
		m_health = MaxHealth;

		//MP
		m_sp = MaxSp;

		//���j�b�g���X�g�ɉ�����
		m_turnManager.SetList(gameObject);

		//�������A�������ړ��s��
		GridMass gridmass = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMass>();
		foreach(GameObject grid in gridmass.GridList)
		{
			//���j�b�g�̑����ɂ͈ړ��s��
			if (Mathf.RoundToInt(grid.transform.position.x) == Mathf.RoundToInt(transform.position.x)
				&& Mathf.RoundToInt(grid.transform.position.z) == Mathf.RoundToInt(transform.position.z))
			{
				grid.GetComponent<Choice>().SetPossible(false);
			}
		}
	}

	public List<SkillAttack> GetAttackSkill()
	{
		return m_attackSkillList;
	}

	public List<Skill> GetSkill()
	{
		return m_skillList; 
	}

	public void Damage(int damage)
	{
		if (m_health <= 0) return;
		m_health -= Calculation(damage);
		Debug.Log(gameObject.name+"��"+Calculation(damage)+"�_���[�W���󂯂�" +"�c���"+m_health);
	}

	public void OnDeath()
	{
		m_turnManager.DeleteList(gameObject);
		//m_animator.SetTrigger("Death");
		//���S���b�Z�[�W
		Debug.Log(m_name + "�͓|�ꂽ");
		Destroy(gameObject,5);
	}

	public void MoveTo(Vector3 targetPos)
	{
		// �ړ������������W�ɃX�i�b�v 
		transform.position = new Vector3(
			Mathf.RoundToInt(targetPos.x),
			transform.position.y,    //�����͂܂Ƃ߂��炾��
			Mathf.RoundToInt(targetPos.z)
		);
		return;
	}

	public int Calculation(int damage)
	{
		//�h��͈ȉ��̃_���[�W��1�ɂ���
		if (damage <= m_defense) return 1;
		return damage -= m_defense;
	}

	public void DirectionReset()
	{
		gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
	}
}

