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

	UnitManager m_turnManager;
	GridMass m_gridmass;

	GameObject m_turnUnit;
	GameObject m_targetMass;
	Vector3 m_targetPos;

	Vector3[] m_possibleMass;
	bool m_move;

	List<GameObject> m_moveList = new();

	private void Awake()
	{
		m_turnManager = GetComponent<UnitManager>();
		m_gridmass = m_mapBlock.GetComponent<GridMass>();
	}

	private void Start()
	{
		m_move = false;
	}

	private void Update()
	{
		//現在のターンのUnitを更新
		m_turnUnit = m_turnManager.TurnUnit;
		switch (m_turnManager.GetPhase)
		{
			case UnitManager.Phase.Select:
				OnSelect();
				if(Input.GetMouseButtonDown(0))
				{
					OnPick();
				}
				break;

			case UnitManager.Phase.Action:
				m_move = false;
				
				m_turnManager.NextPhase(UnitManager.Phase.End);
				break;
		}
	}

	public void OnPick()
	{
		// タップした場所にカメラからRayを飛ばす
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			//マスの選択
			m_targetMass = hit.collider.gameObject;
			m_targetMass.TryGetComponent(out Choice choice);
			if (choice.Possible)
			{
				m_move = true;
				m_targetPos = m_targetMass.transform.position;
				m_turnUnit.transform.position = m_targetPos;
				m_turnManager.NextPhase(UnitManager.Phase.Action);
				OnRemove();
			}
		}
	}

	public void OnSelect()
	{
		if (m_move) return;
		//移動可能マスを表示する
		Unit unit = m_turnUnit.GetComponent<Unit>();
		foreach (GameObject list in m_gridmass.GridList)
		{
		Choice choice = list.GetComponent<Choice>();
			for (int i = 0; i < unit.Destination.Length; i++)
			{
				if (list.transform.position.x == unit.Destination[i].x + m_turnUnit.transform.position.x
					&& list.transform.position.z == unit.Destination[i].z + m_turnUnit.transform.position.z)
				{
					choice.OnChoice();
					m_moveList.Add(list);
				}
			}
		}
	}

	public void OnRemove()
	{
		foreach(GameObject list in m_moveList)
		{
			Choice choice = list.GetComponent<Choice>();
			choice.OnCancell();
		}
		m_moveList.Clear();
	}

	//プレイヤーのポジションとマップのブロック座標比較をいちいち書くのめんどいから必要な奴だけ抽出
	public Vector3 Extraction(Vector3 posWidth , Vector3 posHeight)
	{
		return new Vector3(posWidth.x ,posHeight.y, posWidth.z);
	}

}
