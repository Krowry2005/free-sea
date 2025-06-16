using System.Collections.Generic;
using UnityEngine;

public class GridMass : MonoBehaviour
{
	[SerializeField]
	Transform m_parent;

	[SerializeField]
	GameObject m_possibleGrid;

	[SerializeField]
	GameObject m_impossibleGrid;

	[Header("��"),SerializeField] 
	int m_width;

	[Header("���s"),SerializeField]
	int m_height;

	[Header("�s�\�}�X(����,���s�̔����܂�)"),SerializeField]
	Vector3[] m_impossibleMass;

	Vector3Int m_gridMass;

	List<GameObject> m_gridList = new();
	List<Vector3Int> m_posList = new();

	public Vector3 Grid => m_gridMass;

	public List<GameObject> GridList => m_gridList;
	public List<Vector3Int> PosList => m_posList;

	private void Awake()
	{
		Vector3Int defaultPos = Vector3Int.zero;
		//defaultPos.x = -(m_width / 2);  //X���W�̊�_
		//defaultPos.z = -(m_height / 2); //Y���W�̊�_

		m_gridMass = new Vector3Int (m_width ,0 , m_height);
		for (int x = 0; x < m_width; x++)
			for(int z = 0; z < m_height; z++)
			{
				Vector3Int pos = defaultPos;
				pos.x += x;
				pos.z += z;

				//���̃}�X���z�u�\�}�X������
				bool possible = true;
				for (int i = 0; i < m_impossibleMass.Length; i++)
				{
					if (pos == m_impossibleMass[i])
					{
						possible = false;
					}
				}

				//�I�u�W�F�N�g�̐����ƍ��W�̊m��
				GameObject grid;
				if (possible)
				{
					grid = Instantiate(m_possibleGrid, m_parent);
				}
				else
				{
					grid = Instantiate(m_impossibleGrid, m_parent);
				}
				grid.transform.position = pos;
				m_posList.Add(pos);
			}
	}

	public void SetGrid(GameObject grid)
	{
		m_gridList.Add(grid);
	}
}

