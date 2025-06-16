using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevelPhysics;
using UnityEngine.Rendering.Universal;

public class UnitAction : MonoBehaviour
{
	public enum Action
	{
		Choice,
		Move,
		Attack,
		Item,
		Skill,
		Information,

		Length,
	}

	[SerializeField]
	GameObject m_mapBlock;

	Action m_action;

	UnitManager m_unitManager;
	GridMass m_gridmass;

	GameObject m_turnUnit;

	List<GameObject> m_viewList = new();
	bool m_move;
	
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
				if (m_move) return;
				switch (m_action)
				{
					case Action.Move:
						
						
						break;

					case Action.Attack:
						
						
						break;

					case Action.Item:

						break;

					case Action.Skill:

						break;

					case Action.Information:
						//すべてのマスが対象
						foreach(GameObject posList in m_gridmass.GridList)
						{
							Choice choice = posList.GetComponent<Choice>();
							choice.OnChoice();
							m_viewList.Add(posList);
							m_unitManager.SetPhase(UnitManager.Phase.Action);
						}
						break;
				}
				break;

			case UnitManager.Phase.Action:
				if (Input.GetMouseButtonDown(0))
				{
					if (OnPick() != null)
					{
						GameObject targetMass = OnPick();
						OnAction(targetMass.transform.position);
					}
					else
					{
						//意味わからんとこ指定されたらセレクトフェーズに戻る
						m_unitManager.SetPhase(UnitManager.Phase.Select);
						m_action = Action.Choice;
						OnRemove();
					}
				}
				break;
		}
	}

	private void OnDisplay(Vector3Int[] massArray, bool possible, bool fly) 
	//選択できるマスの表示 (選択可能範囲、移動不可マスを選択可能か)
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
					//選択不可マスかつ、選択不可表示ならあきらめる
					if (!choice.Possible && !possible) continue;
					choice.OnChoice();
					m_viewList.Add(mapList);
					m_unitManager.SetPhase(UnitManager.Phase.Action);
					break;
				}
			}
		}
	}

	private GameObject OnPick()
	{
		// タップした場所にカメラからRayを飛ばす
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			//今のところマス以外に当たり判定つけないけど一応
			if(hit.collider.gameObject.tag == "Grid")
			{
				// マスの選択
				return hit.collider.gameObject;
			}
			return null;
		}
		else return null;
	}

	private void OnAction(Vector3 targetPos)
	{
		foreach (GameObject viewList in m_viewList)
		{
			//押されたマスが有効マスである場合
			if (SameGridPosition(targetPos, viewList.transform.position))
			{
				switch (m_action)
				{
					case Action.Move:
						OnMove(targetPos);
						break;

					case Action.Attack:
						OnAttack(targetPos);
						break;

					case Action.Item:

						break;

					case Action.Skill:

						break;

					case Action.Information:
						foreach (GameObject unitList in m_unitManager.UnitList)
						{
							if(SameGridPosition(targetPos,unitList.transform.position))
							{
								Debug.Log(unitList.transform.position);
							}
						}
						break;
				}
				break;
			}
		}
	}

	private void OnMove(Vector3 targetPos)
	{
		foreach (GameObject list in m_viewList)
		{
			// 整数座標で比較
			if (SameGridPosition(list.transform.position, targetPos))
			{
				//移動の可不可を再定義、有効マス表示を外ス
				GridInvalid(targetPos);

				// 移動時も整数座標にスナップ 
				m_turnUnit.transform.position = new Vector3(
					Mathf.RoundToInt(targetPos.x),
					m_turnUnit.transform.position.y,	//ここはまとめたらだめ
					Mathf.RoundToInt(targetPos.z)
				);

				m_unitManager.SetPhase(UnitManager.Phase.End);
				m_action = Action.Choice;
				return;
			}
		}
	}

	private void OnAttack(Vector3 targetPos)
	{
		//選択したマスにいるキャラをすべて抽出
		var DamageUnit = m_unitManager.UnitList.Where(unit => SameGridPosition(unit.transform.position, targetPos));
		if (DamageUnit.Count() > 0)
		{
			foreach (GameObject list in DamageUnit)
			{
				//ダメージ
				Unit unit = list.GetComponent<Unit>();

				//フレンドリーファイアの禁止
				if (unit.FriendLevel != m_turnUnit.GetComponent<Unit>().FriendLevel)
				{
					Debug.Log("HIt?");
					unit.Damage(m_turnUnit.GetComponent<Unit>().AttackValue * m_turnUnit.GetComponent<Unit>().GetAttackSkills()[0].GetMagnification() / 100);
				}
				else
				{
					Debug.Log("miss");

				}
			}
		}

		//選択したマスに何もいなかったらどうにかする
		else
		{
			Debug.Log("miss");
		}

		OnRemove();
		m_unitManager.SetPhase(UnitManager.Phase.End);
		m_action = Action.Choice;
	}

	private void OnItem()
	{

	}

	private void OnSkill()
	{

	}

	public void OnRemove()
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

	public void GridInvalid(Vector3 targetPos)
	{
	// 今いるマスを移動可能マスに戻し、移動後マスを移動不可マスにする
		foreach (GameObject grid in m_gridmass.GridList)
		{
			// 整数座標で比較 
			//もといた座標のマスを移動可能にする
			if (SameGridPosition(grid.transform.position, m_turnUnit.transform.position))
			{
				grid.GetComponent<Choice>().SetPossible(true);
			}

			//移動先を移動不可にする
			if (SameGridPosition(grid.transform.position,targetPos))
			{
				grid.GetComponent<Choice>().SetPossible(false);
			}
		}

		//有効マスの表示を外して次のフェーズへ
		OnRemove();

	}
}
