using System;
using UnityEngine;
using UnityEngine.Analytics;
using static UnitsData.UnitData;

public class GameController : MonoBehaviour
{
	[Header("¦•K‚¸m_ally‚Æm_intialPosition‚Ì”z—ñ‚Ì’·‚³‚ğ‡‚í‚¹‚é‚±‚Æ"),SerializeField]
	GameObject[] m_ally;
	[SerializeField]
	Vector3[] m_initialPosition;

	Generator m_generator;

	private void Awake()
	{
		m_generator = GetComponent<Generator>();
	}

	private void Start()
	{
		for(int i = 0; i < m_ally.Length; i++)
		{
		 	m_generator.OnGenerate(m_ally[i], m_initialPosition[i], Quaternion.identity,FriendLevel.Ally);
		}
	}
}
