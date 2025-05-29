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

	[Header("�s�\�}�X(����,���s�̔����܂�)"),SerializeField]
	Vector3[] m_impossibleMass;

	Vector3 m_gridMass;

	public Vector3 GridMass => m_gridMass;

	private void Start()
	{
		Vector3 defaultPos = Vector3.zero;
		defaultPos.x = -(m_width / 2);	//X���W�̊�_
		defaultPos.z = -(m_height / 2);	//Y���W�̊�_

		m_gridMass = new Vector3 (m_width ,0 , m_height);

		for (int x = 1; x < m_width; x++)
			for(int z = 1; z < m_height; z++)
			{
				Vector3 pos = defaultPos;
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
			}
	}
}

