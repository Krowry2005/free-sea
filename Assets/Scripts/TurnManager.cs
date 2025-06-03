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
		Enemy,
		Select,
		Action,
		End,
		Length,
	}

	Phase m_phase;
	GameObject m_turnUnit;
	bool m_turnStart;
	bool m_buttleEnd;
	int m_round;

	public GameObject TurnUnit => m_turnUnit;
	public List<GameObject> UnitList = new List<GameObject>();
	public List<GameObject> ReserveUnit;
	public Phase GetPhase => m_phase;
	public int Rount => m_round; 
	
	private void Start()
	{
		m_buttleEnd = false;
		m_phase = Phase.Start;
		m_round = 0;
	}

	private void Update()
	{
		switch (m_phase)
		{
			case Phase.Start:
				m_turnUnit = UnitList.First();
				if(m_turnUnit.TryGetComponent(out Unit unit))
				{
					if(unit.FriendLevel == UnitsSetting.UnitData.FriendLevel.Enemy) NextPhase(Phase.Enemy);
					else NextPhase(Phase.Select);
				}
				break;

			case Phase.Enemy:
				//今は仮置きとしてターン終了
				NextPhase(Phase.End);
				break;

			case Phase.End:
				//リストの先頭要素を控えのリストにコピーする
				ReserveUnit.Add(UnitList.First());

				//リストの最初の要素を削除
				UnitList.RemoveAt(0);

				//バフの消費

				//フェーズを最初に戻す
				NextPhase(Phase.Start);

				if(UnitList.Count == 0)
				{
					//行動順をリセットし、ラウンドを消費
					UnitList.AddRange(ReserveUnit);
					ReserveUnit.Clear();
					SortList();
					m_round++;
				}
				break;
		}
	}

	public Phase NextPhase(Phase phase)
	{
		//別スクリプト下でもフェーズをいじれるようになる
		return m_phase = phase;
	}

	public void SetList(GameObject list)
	{
		//リストに加えて速度順に並べ替える
		UnitList.Add(list);
		SortList();
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