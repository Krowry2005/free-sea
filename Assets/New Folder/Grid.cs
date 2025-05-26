using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	static Grid Instance;

	//マスの幅と奥行き
	int Width = 20;
	int Height = 2;
	int Depth = 20;

	//マス目を格納する3次元配列

	static Transform[,,] m_grid;

	private void Awake()
	{
		//マス目が既に存在する場合はこのオブジェクトを破壊
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
		//スクリーン座標をマス目の座標にまとめる
		return new Vector3(
				Mathf.Round(screen.x),
				Mathf.Round(screen.y),
				Mathf.Round(screen.z));
	}

	//指定された範囲がグリッドの範囲内かのチェック
	public bool InsideBorder(Vector3 pos)
	{
		return	0 < (int)pos.x && (int)pos.x <= Width &&
					0 < (int)pos.y && (int)pos.y <= Height&&
						0 < (int)pos.z && (int)pos.z <= Depth;
	}

	//指定のオブジェクトの削除
	public void OnDelete(Vector3Int pos)
	{
		Destroy(m_grid[pos.x,pos.y,pos.z].gameObject);
		m_grid[pos.x,pos.y,pos.z] = null;
	}

	//グリッドの更新
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
