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

	List<GameObject> m_unitList = new();            // Unit���X�g
	List<GameObject> m_speedList = new();			// ���x���ɕ��ׂĂ��郊�X�g					
	List<GameObject> m_reserveTurnList = new();		// ���x�Ǘ��̂��߂ɕۊǂ��郊�X�g

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
				//�^�[�����j�b�g�̍X�V
				m_turnUnit = m_speedList.First();

				//�A�N�V�����o�[�̌����ڂ�V��������
				for (int i = 0; i < m_image.Length; i++)
				{	if(i >= m_speedList.Count())
					{
						m_actionBar[i].SetActive(false);
					}
					else m_image[i].sprite = GetSprite(i);
				}

				//�^�[�����j�b�g���G�Ȃ�AI��������
				if (m_turnUnit.TryGetComponent(out Unit unit))
				{
					if(unit.FriendLevel == UnitsSetting.UnitData.FriendLevel.Enemy) NextPhase(Phase.Enemy);
					else NextPhase(Phase.Select);
				}
				break;

			case Phase.Enemy:
				//���͉��u���Ƃ��ă^�[���I��
				NextPhase(Phase.End);
				break;

			case Phase.End:
				//���X�g�̐擪�v�f���T���̃��X�g�ɃR�s�[����
				m_reserveTurnList.Add(m_speedList.First());

				//���X�g�̍ŏ��̗v�f���폜
				m_speedList.RemoveAt(0);

				//�s���I�����A�S�����s���ς݂ł����
				if(m_speedList.Count == 0)
				{
					//�s���������Z�b�g���A���E���h������
					m_speedList.AddRange(m_reserveTurnList);
					m_reserveTurnList.Clear();
					SortList();
					m_round++;

					foreach(GameObject actionBar in m_actionBar)
					{
						actionBar.SetActive(true);
					}
				}

				//�t�F�[�Y���ŏ��ɖ߂�
				NextPhase(Phase.Start);
				break;
		}
	}

	public Phase NextPhase(Phase phase)
	{
		//�ʃX�N���v�g���ł��t�F�[�Y���������悤�ɂȂ�
		return m_phase = phase;
	}

	public void SetList(GameObject list)
	{
		//���X�g�ɉ�����
		UnitList.Add(list);

		//���ׂẴ��j�b�g��Unit�R���|�[�l���g�������Ă���,,,�͂�
		Unit unit;
		if(list.TryGetComponent(out unit))
		{
			//���x�O�̃��j�b�g�͍s��������O���
			if (unit.Agility != 0)
			{
				//�^�[���r���ŏ������ꂽ���j�b�g�͍Ō�
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
		// GameObject��SampleScript�̃y�A���ɍ���Ă����iGetComponent��1�񂾂��j
		var objectScriptPairs = m_speedList
			.Select(unit => new { unit, script = unit.GetComponent<Unit>() })
			.ToList();

		// SampleScript.hp ���g���č~���\�[�g�i�l���傫�����j
		objectScriptPairs.Sort((a, b) => b.script.Agility.CompareTo(a.script.Agility));

		// �\�[�g���ʂ���GameObject�̃��X�g�����ɍč\��
		m_speedList = objectScriptPairs.Select(pair => pair.unit).ToList();
	}

	public Sprite GetSprite(int num)
	{
		Unit unit;
		m_speedList[num].TryGetComponent(out unit);
		return unit.Sprite;
	}
}