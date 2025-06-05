using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using static UnitsSetting.UnitData;

public class GameController : MonoBehaviour
{
	[Header("初期生成のオブジェクト")]
	[Header("※必ずm_allyとm_intialPositionの配列の長さを合わせること"),SerializeField]
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
		 	m_generator.OnGenerate(m_ally[i], m_initialPosition[i], Quaternion.identity, FriendLevel.Ally);
		}
	}
}
