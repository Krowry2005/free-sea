using System;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public enum Phase
	{
		Start,
		Select,
		Action,
		End,
		Length,
	}

	Phase m_phase;

	public List<GameObject> UnitList = new List<GameObject>();
	public Phase GetPhase => m_phase;
	
	private void Start()
	{
		
	}

	private void Update()
	{

	}

	public Phase NextPhase(Phase phase)
	{
		return m_phase = phase;
	}

	public void SetList(GameObject list)
	{
		//リストに加えて速度順に並べ替える
		UnitList.Add(list);
		SortList();

		foreach (GameObject unit in UnitList)
		{
			Unit m_unit;
			unit.TryGetComponent(out m_unit);
			Debug.Log(m_unit.UnitName());
		}
	}

	public void DeleteList(GameObject list)
	{
		UnitList.Remove(list);
		SortList();
	}

	public void SortList()
	{
		//	//GameObject型のリストで、リスト内のゲームオブジェクトが持ってるUnitっていうスクリプトのAgility順にソートしたい
		//	//わからん事、↑を実行するためにUnitList.Sort()の中に何をほりこめばいいのか、GameObject型リストなのだが、この場合何の数値を基準にソートされるのか

		// GameObjectとSampleScriptのペアを先に作っておく（GetComponentは1回だけ）
		var objectScriptPairs = UnitList
			.Select(unit => new { unit, script = unit.GetComponent<Unit>() })
			.ToList();

		// SampleScript.hp を使って降順ソート（値が大きい順）
		objectScriptPairs.Sort((a, b) => b.script.Agility.CompareTo(a.script.Agility));

		// ソート結果からGameObjectのリストだけに再構成
		UnitList = objectScriptPairs.Select(pair => pair.unit).ToList();
	}
}
