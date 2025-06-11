using UnityEngine;

public class UnitFactory : MonoBehaviour
{
	[SerializeField]
	GameObject m_gameController;

	[SerializeField]
	GameObject[] m_enemyUnit;
	[SerializeField]
	Vector3[] m_initEnemyPos;

	Generator m_generator;

	private void Awake()
	{
		m_generator = m_gameController.GetComponent<Generator>();
	}

	private void Start()
	{
		m_generator.OnGenerate(m_enemyUnit[0], m_initEnemyPos[0], Quaternion.identity, UnitsSetting.UnitData.FriendLevel.Enemy);
	}
}
