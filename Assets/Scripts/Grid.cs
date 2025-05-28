using Unity.Mathematics;
using UnityEngine;

public class Grid : MonoBehaviour
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

	[SerializeField]
	Vector3[] m_impossibleMass;

	private void Start()
	{
		for (int x = 1; x < m_width; x++)
			for(int z = 1; z < m_height; z++)
			{
				Vector3 pos = new Vector3(x,0,z);

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
			}
	}
}

