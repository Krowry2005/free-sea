using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
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

	[SerializeField]
	Image[] m_image;

	[SerializeField]
	GameObject[] m_actionBar;

	Phase m_phase;
	GameObject m_turnUnit;
	int m_round;

	List<GameObject> m_unitList = new();            // Unitリスト
	List<GameObject> m_speedList = new();			// 速度順に並べてあるリスト					
	List<GameObject> m_reserveTurnList = new();		// 速度管理のために保管するリスト

	public GameObject TurnUnit => m_turnUnit;
	public Phase GetPhase => m_phase;
	public int Rount => m_round; 
	public List<GameObject> UnitList => m_unitList;
	public List<GameObject> SpeedList => m_speedList;

	private void Awake()
	{
		for (int i = 0; i < m_image.Length; i++)
		{
			m_image[i] = m_image[i].GetComponent<Image>();
		}
	}

	private void Start()
	{
		m_phase = Phase.Start;
		m_round = 0;
	}

	private void Update()
	{
		switch (m_phase)
		{
			case Phase.Start:
				//ターンユニットの更新
				m_turnUnit = m_speedList.First();

				//アクションバーの見た目を新しくする
				for (int i = 0; i < m_image.Length; i++)
				{	if(i >= m_speedList.Count())
					{
						m_actionBar[i].SetActive(false);
					}
					else m_image[i].sprite = GetSprite(i);
				}

				//ターンユニットが敵ならAIが動かす
				if (m_turnUnit.TryGetComponent(out Unit unit))
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
				m_reserveTurnList.Add(m_speedList.First());

				//リストの最初の要素を削除
				m_speedList.RemoveAt(0);

				//行動終了時、全員が行動済みであれば
				if(m_speedList.Count == 0)
				{
					//行動順をリセットし、ラウンドを消費
					m_speedList.AddRange(m_reserveTurnList);
					m_reserveTurnList.Clear();
					SortList();
					m_round++;

					foreach(GameObject actionBar in m_actionBar)
					{
						actionBar.SetActive(true);
					}
				}

				//フェーズを最初に戻す
				NextPhase(Phase.Start);
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
		//リストに加える
		UnitList.Add(list);

		//すべてのユニットはUnitコンポーネントを持っている,,,はず
		Unit unit;
		if(list.TryGetComponent(out unit))
		{
			//速度０のユニットは行動順から外れる
			if (unit.Agility != 0)
			{
				//ターン途中で召喚されたユニットは最後
				m_speedList.Add(list);
			}
		}
	}
	
	public void DeleteList(GameObject list)
	{
		UnitList.Remove(list);
		m_speedList.Remove(list);
		m_reserveTurnList.Remove(list);
		SortList();
	}

	public void SortList()
	{
		// GameObjectとSampleScriptのペアを先に作っておく（GetComponentは1回だけ）
		var objectScriptPairs = m_speedList
			.Select(unit => new { unit, script = unit.GetComponent<Unit>() })
			.ToList();

		// SampleScript.hp を使って降順ソート（値が大きい順）
		objectScriptPairs.Sort((a, b) => b.script.Agility.CompareTo(a.script.Agility));

		// ソート結果からGameObjectのリストだけに再構成
		m_speedList = objectScriptPairs.Select(pair => pair.unit).ToList();
	}

	public Sprite GetSprite(int num)
	{
		Unit unit;
		m_speedList[num].TryGetComponent(out unit);
		return unit.Sprite;
	}
}