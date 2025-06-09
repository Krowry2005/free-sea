using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevelPhysics;

public class UnitAction : MonoBehaviour
{
	public enum Action
	{
		Choice,
		Move,
		Attack,
		Item,
		Skill,

		Length,
	}

	[SerializeField]
	GameObject m_mapBlock;

	Action m_action;

	UnitManager m_unitManager;
	GridMass m_gridmass;

	GameObject m_turnUnit;

	List<GameObject> m_viewList = new();
	Vector3 m_targetPos;
	bool m_move;
	int count;
	
	private void Awake()
	{
		m_unitManager = GetComponent<UnitManager>();
		m_gridmass = m_mapBlock.GetComponent<GridMass>();
	}

	private void Start()
	{
		m_move = false;
		m_action = Action.Choice;
	}

	private void Update()
	{
		//現在のターンのUnitを更新
		m_turnUnit = m_unitManager.TurnUnit;
		switch (m_unitManager.GetPhase)
		{
			case UnitManager.Phase.Select:
				if(!m_move)switch (m_action)
				{
					case Action.Move:
						OnMoveSelect(m_turnUnit.GetComponent<Unit>().Destination);
							//アクション選択後にマスのピックが可能
							if (Input.GetMouseButtonDown(0)) OnPick();
							break;
					
					case Action.Attack:
						OnAttackSelect(m_turnUnit.GetComponent<Unit>().AttackPos);
							//アクション選択後にマスのピックが可能
							if (Input.GetMouseButtonDown(0)) OnPick();
							break;

					case Action.Item:
							//アイテムの使用
							//アクション選択後にマスのピックが可能
							if (Input.GetMouseButtonDown(0)) OnPick();
							break;

					case Action.Skill:
							//スキルの使用
							//アクション選択後にマスのピックが可能
							if (Input.GetMouseButtonDown(0)) OnPick();
							break;
				}
				break;

			case UnitManager.Phase.Action:
				switch (m_action)
				{
					case Action.Move:
						OnMove();
						break;

					case Action.Attack:
						OnAttack();
						break;

					case Action.Item:
						OnItem();
						break;

					case Action.Skill:
						OnSkill();
						break;
				}
				m_move = false;
				break;
		}
	}

	private void OnMoveSelect(Vector3Int[] massArray)
	//選択できるマスの表示
	{
		foreach (GameObject mapList in m_gridmass.GridList)
		{
			Choice choice = mapList.GetComponent<Choice>();
			for (int i = 0; i < massArray.Length; i++)
			{
				// 整数座標で比較 
				int destX = Mathf.RoundToInt(m_turnUnit.transform.position.x + massArray[i].x);
				int destZ = Mathf.RoundToInt(m_turnUnit.transform.position.z + massArray[i].z);
				if (SameGridPosition(mapList.transform.position, new Vector3(destX, mapList.transform.position.y, destZ)))
				{
					//移動アクション中かつ、移動可能マスであればリストに加える
					if (choice.Possible && m_action == Action.Move)
					{
						choice.OnChoice();
						m_viewList.Add(mapList);
						break;
					}
				}
			}
		}
		m_move = true;
	}
	private void OnAttackSelect(Vector3Int[] massArray)
	//選択できるマスの表示
	{
	
	}

	private void OnPick()
	//移動先の選択
	{
		// タップした場所にカメラからRayを飛ばす
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			// マスの選択
			GameObject targetMass = hit.collider.gameObject;
			foreach(GameObject list in m_viewList)
			{
				//押されたマスが有効マスである場合
				if(SameGridPosition(targetMass.transform.position,list.transform.position))
				{
					//選択マスを更新し、次のフェーズへ
					m_targetPos = targetMass.transform.position;
					m_unitManager.NextPhase(UnitManager.Phase.Action);
				}
			}
		}
	}

	private void OnMove()
	{
		foreach (GameObject list in m_viewList)
		{
			// 整数座標で比較
			Vector3 listPos = list.transform.position;
			if (SameGridPosition(listPos, m_targetPos))
			{
				// 移動時も整数座標にスナップ 
				m_turnUnit.transform.position = new Vector3(
					Mathf.Round(m_targetPos.x),
					Mathf.Round(m_turnUnit.transform.position.y),
					Mathf.Round(m_targetPos.z)
				);

				//移動の可不可を再定義、有効マス表示を外して次のフェーズへ
				GridInvalid();
				return;
			}
		}
	}

	private void OnAttack()
	{
		
	}

	private void OnItem()
	{

	}

	private void OnSkill()
	{

	}

	private void OnRemove()
	{
		//有効マスの表示を外す
		foreach (GameObject list in m_viewList)
		{
			Choice choice = list.GetComponent<Choice>();
			choice.OnCancell();
		}
		m_viewList.Clear();
	}

	public bool SameGridPosition(Vector3 compare1, Vector3 compare2)
	{
		//二つの座標が同じマスか比較
		//変数名終わってるけど気にしないでね
		return Mathf.RoundToInt(compare1.x) == Mathf.RoundToInt(compare2.x)
			&& Mathf.RoundToInt(compare1.z) == Mathf.RoundToInt(compare2.z);
	}

	public void SetAction(Action action)
	{
		m_action = action;
	}

	public void GridInvalid()
	{
	// 今いるマスを移動可能マスに戻し、移動後マスを移動不可マスにする
		foreach (GameObject grid in m_gridmass.GridList)
		{
			// 整数座標で比較 
			if (SameGridPosition(grid.transform.position, m_turnUnit.transform.position))
			{
				grid.GetComponent<Choice>().SetPossible(true);
			}

			if (SameGridPosition(grid.transform.position, m_targetPos))
			{
				grid.GetComponent<Choice>().SetPossible(false);
			}
		}

		//有効マスの表示を外して次のフェーズへ
		OnRemove();
		m_unitManager.NextPhase(UnitManager.Phase.End);
	}
}
