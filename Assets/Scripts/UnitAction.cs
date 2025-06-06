using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevelPhysics;

public class UnitAction : MonoBehaviour
{
	[SerializeField]
	GameObject m_mapBlock;

	UnitManager m_unitManager;
	GridMass m_gridmass;
	GameObject m_turnUnit;
	GameObject m_targetMass;
	bool m_move;
	List<GameObject> m_moveList = new();

	private void Awake()
	{
		m_unitManager = GetComponent<UnitManager>();
		m_gridmass = m_mapBlock.GetComponent<GridMass>();
	}

	private void Start()
	{
		m_move = false;
	}

	private void Update()
	{
		//現在のターンのUnitを更新
		m_turnUnit = m_unitManager.TurnUnit;
		switch (m_unitManager.GetPhase)
		{
			case UnitManager.Phase.Select:
				RoundVector3(m_turnUnit.transform.position);
				OnSelect();
				if(Input.GetMouseButtonDown(0))
				{
					OnPick();
				}
				break;

			case UnitManager.Phase.Action:
				m_move = false;
				
				m_unitManager.NextPhase(UnitManager.Phase.End);
				break;
		}
	}

	private void OnPick()
	{
		// タップした場所にカメラからRayを飛ばす
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			//マスの選択
			m_targetMass = hit.collider.gameObject;
			foreach (GameObject list in m_moveList)
			{
				//選択されたマスが移動可能なマスの時、移動する
				if (Mathf.Approximately(list.transform.position.x, m_targetMass.transform.position.x)
					&& Mathf.Approximately(list.transform.position.z, m_targetMass.transform.position.z))
				{
					m_move = true;

					//今いるマスを移動可能マスに戻し、移動後マスを移動不可マスにする
					foreach (GameObject grid in m_gridmass.GridList)
					{
						//現在のマスを移動不可にする
						if (Mathf.Approximately(m_turnUnit.transform.position.x, grid.transform.position.x)
							&& Mathf.Approximately(m_turnUnit.transform.position.z, grid.transform.position.z))
						{
							grid.GetComponent<Choice>().SetPossible(true);
						}

						//移動予定マスを移動不可マスにする
						if (Mathf.Approximately(m_targetMass.transform.position.x, grid.transform.position.x)
							&& Mathf.Approximately(m_targetMass.transform.position.z, grid.transform.position.z))	
						{
							grid.GetComponent<Choice>().SetPossible(false);
						}
					}
					m_turnUnit.transform.position = m_targetMass.transform.position;
					m_unitManager.NextPhase(UnitManager.Phase.Action);
					OnRemove();
					return;
				}
			}
		}
	}

	private void OnSelect()
	{
		if (m_move) return;

		Unit unit = m_turnUnit.GetComponent<Unit>();
		foreach (GameObject mapList in m_gridmass.GridList)
		{
			Choice choice = mapList.GetComponent<Choice>();
			for (int i = 0; i < unit.Destination.Length; i++)
			{
				if (Mathf.Approximately( mapList.transform.position.x, m_turnUnit.transform.position.x + unit.Destination[i].x)
					&&Mathf.Approximately( mapList.transform.position.z, m_turnUnit.transform.position.z + unit.Destination[i].z))
				{
					if (choice.Possible)
					{
						choice.OnChoice();
						m_moveList.Add(mapList);
						break;
					}
				}
			}
		}
	}

	private void OnRemove()
	{
		//移動可能マスの表示を外す
		foreach (GameObject list in m_moveList)
		{
			Choice choice = list.GetComponent<Choice>();
			choice.OnCancell();
		}
		m_moveList.Clear();
	}

	public Vector3 RoundVector3(Vector3 v)
	{
		return new Vector3(Mathf.Round(v.x),Mathf.Round(v.y), Mathf.Round(v.z));
	}

	//public void OnPick()
	//{
	//	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	//	if (Physics.Raycast(ray, out RaycastHit hit))
	//	{
	//		m_targetMass = hit.collider.gameObject;
	//		Vector3 targetPos = m_targetMass.transform.position;

	//		foreach (GameObject list in m_moveList)
	//		{
	//			Vector3 listPos = list.transform.position;
	//			if (Mathf.Approximately(listPos.x, targetPos.x) && Mathf.Approximately(listPos.z, targetPos.z))
	//			{
	//				m_move = true;
	//				Vector3 currentPos = m_turnUnit.transform.position;

	//				foreach (GameObject grid in m_gridmass.GridList)
	//				{
	//					Vector3 gridPos = grid.transform.position;
	//					Choice choice = grid.GetComponent<Choice>();

	//					if (Mathf.Approximately(gridPos.x, currentPos.x) && Mathf.Approximately(gridPos.z, currentPos.z))
	//						choice.SetPossible(true);

	//					if (Mathf.Approximately(gridPos.x, targetPos.x) && Mathf.Approximately(gridPos.z, targetPos.z))
	//						choice.SetPossible(false);
	//				}

	//				m_turnUnit.transform.position = new Vector3(targetPos.x, m_turnUnit.transform.position.y, targetPos.z);
	//				m_unitManager.NextPhase(UnitManager.Phase.Action);
	//				OnRemove();
	//				return;
	//			}
	//		}
	//	}
	//}

	//public void OnSelect()
	//{
	//	if (m_move) return;

	//	Unit unit = m_turnUnit.GetComponent<Unit>();
	//	Vector3 unitPos = m_turnUnit.transform.position;

	//	foreach (GameObject mapList in m_gridmass.GridList)
	//	{
	//		Vector3 mapPos = mapList.transform.position;
	//		Choice choice = mapList.GetComponent<Choice>();

	//		foreach (Vector2 offset in unit.Destination)
	//		{
	//			float targetX = unitPos.x + offset.x;
	//			float targetZ = unitPos.z + offset.y;

	//			if (Mathf.Approximately(mapPos.x, targetX) && Mathf.Approximately(mapPos.z, targetZ) && choice.Possible)
	//			{
	//				choice.OnChoice();
	//				m_moveList.Add(mapList);
	//				break;
	//			}
	//		}
	//	}
	//}

	//public void OnRemove()
	//{
	//	foreach (GameObject list in m_moveList)
	//	{
	//		list.GetComponent<Choice>().OnCancell();
	//	}
	//	m_moveList.Clear();
	//}

}
