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

	TurnManager m_turnManager;
	GameController m_gameController;
	Animator m_animator;

	string m_name;
	int m_id;
	int m_health;
	int m_attack;
	int m_defense;
	int m_agility;

	public int Agility => m_agility;

	public int HealthValue => m_health;

	private void Awake()
	{
		GameObject gameController;
		gameController = GameObject.FindGameObjectWithTag("GameController");
		m_gameController = gameController.GetComponent<GameController>();
		m_turnManager = gameController. GetComponent<TurnManager>();
		m_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		UnitData unitData = unitsData.data.FirstOrDefault(unitSetting => unitSetting.id == m_dataId && unitSetting.friendLevel == m_friendLevel);
		m_name = unitData.name;
		m_health = unitData.health;
		m_attack = unitData.attack;
		m_defense = unitData.defense;
		m_agility = unitData.agility;

		//���X�g�ɉ�����
		m_turnManager.SetList(gameObject);
	}

	public string UnitName()
	{
		return m_name;
	}

	public void OnDamage(int damage)
	{
		if (m_health <= 0) return;
		m_health -= Calculation(damage);
	}

	public void OnDeath()
	{
		m_turnManager.DeleteList(gameObject);
		m_animator.SetTrigger("Death");

		//���S���b�Z�[�W
		Debug.Log(m_name + "�͓|�ꂽ");
	}

	public int Calculation(int damage)
	{
		//�h��͈ȉ��̃_���[�W��1�ɂ���
		if (damage <= m_defense) damage = 1;
		return damage -= m_defense;
	}

	public void ResetStatus()
	{
		//�X�e�[�^�X���Z�b�g
		UnitData unitData = unitsData.data.FirstOrDefault(unitSetting => unitSetting.id == m_dataId && unitSetting.friendLevel == m_friendLevel);
		m_name = unitData.name;
		m_health = unitData.health;
		m_attack = unitData.attack;
		m_defense = unitData.defense;
		m_agility = unitData.agility;
	}
}

