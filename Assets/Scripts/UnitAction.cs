using NUnit.Framework;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevelPhysics;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

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

	[SerializeField]
	GameObject m_uiController;

	Action m_action;

	UnitManager m_unitManager;
	GridMass m_gridmass;
	GameObject m_turnUnit;
	Skill m_usedSkill;
	Information m_information;

	List<GameObject> m_rangeViewList = new();
	List<GameObject> m_extentViewList = new();
	bool m_move;
	bool m_select;
	bool m_actionAproval;

	public Action GetAction => m_action;
	
	private void Awake()
	{
		m_unitManager = GetComponent<UnitManager>();
		m_gridmass = m_mapBlock.GetComponent<GridMass>();
		m_information = m_uiController.GetComponent<Information>();
	}

	private void Start()
	{
		m_move = false;
		m_action = Action.Choice;
		m_select = false;
		m_actionAproval = true;
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
					case Action.Choice:
						m_actionAproval = true;
						break;

					case Action.Move:
						//移動範囲の表示1回目
						OnDisplay(m_turnUnit.GetComponent<Unit>().GetSkill()[0].GetRange(), false, false);
						m_unitManager.SetPhase(UnitManager.Phase.Action);
						break;

					case Action.Attack:
						//攻撃範囲の表示
						OnDisplay(m_turnUnit.GetComponent<Unit>().GetSkill()[1].GetRange(), true, true);
						m_unitManager.SetPhase(UnitManager.Phase.Action);
						break;

					case Action.Information:
						//情報の取得
						foreach(GameObject unitList in m_unitManager.UnitList)
						{
							foreach(GameObject posList in m_gridmass.GridList)
							{
								if(SameGridPosition(unitList.transform.position,posList.transform.position))
								{
									Choice choice = posList.GetComponent<Choice>();
									choice.OnChoice(true);
									m_actionAproval = false;
									m_rangeViewList.Add(posList);
									m_unitManager.SetPhase(UnitManager.Phase.Action);
									break;
								}
							}
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
						OnAction( new Vector3Int(Mathf.RoundToInt(targetMass.transform.position.x),
													Mathf.RoundToInt(targetMass.transform.position.y),
														Mathf.RoundToInt(targetMass.transform.position.z)));
					}
					else
					{
						if (!m_actionAproval) return;
						//意味わからんとこ指定されたらセレクトフェーズに戻る
						m_unitManager.SetPhase(UnitManager.Phase.Select);
						m_action = Action.Choice;
						m_information.DefText();
						OnRemove();
					}
				}
			break;
		}
	}

	private void OnAction(Vector3Int targetPos)
	{
		// コピーを使って安全にループ
		// 効果選択範囲としていされたマスが同じであるかを走査
		foreach (GameObject viewList in m_rangeViewList.ToList())
		{
			//同じならそのマスを起点に効果範囲を表示、セレクトフェーズに返す
			if (SameGridPosition(viewList.transform.position, targetPos))
			{
				switch (m_action)
				{
					case Action.Move:
						if (m_select)
						{
							//一回目の選択では効果範囲の表示だけ
							OnExtent(m_turnUnit.GetComponent<Unit>().GetSkill()[0].GetExtent(), targetPos);
						}
						else
						{
							//2回目で実行
							OnMove(targetPos);
						}
						break;

					case Action.Attack:
						if (m_select)
						{
							//一回目の選択では効果範囲の表示だけ
							OnExtent(m_turnUnit.GetComponent<Unit>().GetAttackSkill()[0].GetExtent(), targetPos);
						}
						else
						{
							//2回目で実行
							OnAttack(m_turnUnit.GetComponent<Unit>().GetAttackSkill()[0], targetPos);
						}
						break;

					case Action.Information:
						//情報取得、ターン消費なし
						for (int i = 0; i <  m_unitManager.UnitList.Count; i++)
						{
							if (SameGridPosition(targetPos, m_unitManager.UnitList[i].transform.position))
							{
								m_information.OnInformation(m_unitManager.UnitList[i].GetComponent<Unit>());
							}
						}
						break;
				}
				//選択が終わったのでループから抜ける
				break;
			}
		}
	}

	public void OnDisplay(Vector3Int[] massArray, bool possible, bool fly)
	//選択できるマスの表示 (選択可能範囲、移動不可マスを選択可能か)
	{
		foreach (GameObject mapList in m_gridmass.GridList)
		{
			for (int i = 0; i < massArray.Length; i++)
			{
				// 整数座標で比較 
				int destX = Mathf.RoundToInt(m_turnUnit.transform.position.x + massArray[i].x);
				int destZ = Mathf.RoundToInt(m_turnUnit.transform.position.z + massArray[i].z);
				if (SameGridPosition(mapList.transform.position, new Vector3(destX, mapList.transform.position.y, destZ)))
				{
					Choice choice = mapList.GetComponent<Choice>();
					//選択不可マスかつ、選択不可表示ならあきらめる
					if (!choice.Possible && !possible) continue;
					choice.OnChoice(true);
					m_select = true;
					m_rangeViewList.Add(mapList);
					m_unitManager.SetPhase(UnitManager.Phase.Action);
					break;
				}
			}
		}
	}

	public void OnExtent(Vector3Int[] extent, Vector3 ActivationPos)
	//スキル、アイテム等の効果範囲の表示
	{
		for (int i = 0; i < extent.Length; i++)
		{
			foreach (GameObject mapList in m_gridmass.GridList)
			{
				// 整数座標で比較 
				int destX = Mathf.RoundToInt(ActivationPos.x + extent[i].x);
				int destZ = Mathf.RoundToInt(ActivationPos.z + extent[i].z);
				if (SameGridPosition(mapList.transform.position, new Vector3(destX, mapList.transform.position.y, destZ)))
				{
					Choice choice = mapList.GetComponent<Choice>();
					choice.OnExtent(true);
					m_select = false;
					m_extentViewList.Add(mapList);

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
			if (hit.collider.gameObject.tag == "Grid")
			{
				// マスの選択
				return hit.collider.gameObject;
			}
			return null;
		}
		else return null;
	}

	private void OnMove(Vector3 targetPos)
	{
		foreach (GameObject list in m_extentViewList)
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

	private void OnAttack(Skill skill,Vector3Int targetPos)
	{
		List<Vector3Int> extentList = new();
		foreach (Vector3Int extent in skill.GetExtent())
		{
			extentList.Add(extent + targetPos);
		}

		//選択したマスにいるキャラをすべて抽出
		for (int i = 0; i < extentList.Count(); i++)
		{
			var DamageUnit = m_unitManager.UnitList.Where(unit => SameGridPosition(unit.transform.position,extentList[i]));
			if (DamageUnit.Count() > 0)
			{
				Unit attackUnit = m_turnUnit.GetComponent<Unit>();
				foreach (GameObject list in DamageUnit)
				{
					//ダメージ
					Unit hitUnit = list.GetComponent<Unit>();

					//フレンドリーファイアの禁止
					if (hitUnit.FriendLevel != attackUnit.FriendLevel)
					{
						Transform unit = m_turnUnit.transform;
						unit.LookAt(list.transform);
						hitUnit.Damage(attackUnit.AttackValue * attackUnit.GetAttackSkill()[0].GetMagnification());
						Animator animator = m_turnUnit.GetComponent<Animator>();
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
	}

	public void OnSkill(Skill usedSkill)
	{
		m_usedSkill = usedSkill;
		OnDisplay(usedSkill.GetRange(), true, true);
	}

	public void OnRemove()
	{
		//効果使用範囲マスの表示を外す
		foreach (GameObject list in m_rangeViewList)
		{
			Choice choice = list.GetComponent<Choice>();
			choice.OnChoice(false);
			choice.OnExtent(false);
		}

		//効果発動範囲マスの表示を外す
		foreach (GameObject list in m_extentViewList)
		{
			Choice choice = list.GetComponent<Choice>();
			choice.OnChoice(false);
			choice.OnExtent(false);
		}
		m_rangeViewList.Clear();
		m_extentViewList.Clear();
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
