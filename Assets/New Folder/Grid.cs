using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	static Grid Instance;

	//�}�X�̕��Ɖ��s��
	int Width = 20;
	int Height = 2;
	int Depth = 20;

	//�}�X�ڂ��i�[����3�����z��

	static Transform[,,] m_grid;

	private void Awake()
	{
		//�}�X�ڂ����ɑ��݂���ꍇ�͂��̃I�u�W�F�N�g��j��
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
			m_grid = new Transform[Width,Height,Depth];
		}
	}

	public Vector3 ScreenToGrid(Vector3 screen)
	{
		//�X�N���[�����W���}�X�ڂ̍��W�ɂ܂Ƃ߂�
		return new Vector3(
				Mathf.Round(screen.x),
				Mathf.Round(screen.y),
				Mathf.Round(screen.z));
	}

	//�w�肳�ꂽ�͈͂��O���b�h�͈͓̔����̃`�F�b�N
	public bool InsideBorder(Vector3 pos)
	{
		return	0 < (int)pos.x && (int)pos.x <= Width &&
					0 < (int)pos.y && (int)pos.y <= Height&&
						0 < (int)pos.z && (int)pos.z <= Depth;
	}

	//�w��̃I�u�W�F�N�g�̍폜
	public void OnDelete(Vector3Int pos)
	{
		Destroy(m_grid[pos.x,pos.y,pos.z].gameObject);
		m_grid[pos.x,pos.y,pos.z] = null;
	}

	//�O���b�h�̍X�V
	public void UpdateGrid(Transform transform)
	{
		for(int y = 0; y < Height; ++y)
			for(int x = 0; x < Width; ++x)
				for(int z = 0; z < Depth; ++z)
					if (m_grid[x,y,z] != null)
					{
						if (m_grid[x,y,z].parent == transform)
						{
							m_grid[x,y,z] = null;
						}
					}
		foreach (Transform child in transform)
		{
			Vector3 pos = ScreenToGrid(child.position);
			m_grid[(int)pos.x,(int)pos.y,(int)pos.z] = child;
		}
	}


}
