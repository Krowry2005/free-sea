using UnityEngine;

public class Grid : MonoBehaviour
{
	static Grid Instance;

	const int Width = 10;
	const int Depth = 10;
	const int Height = 1;

	static Transform[,,] m_grid;

	[SerializeField]
	GameObject m_mass;

	private void Awake()
	{
		if(Instance == null && Instance != this)
		{
			Destroy(Instance);
		}
		else
		{
			Instance = this;
			m_grid = new Transform[Width ,Height, Depth];
			DontDestroyOnLoad(Instance);
		}
		for (int x = 0; x < Width; ++x)
			for (int y = 0; y < Height; ++y)
				for (int z = 0; z < Depth; ++z)
				{
					Instantiate(m_mass, new Vector3(x, y, z), Quaternion.identity);
				}
	}

	private void Start()
	{

	}

	public Vector3Int ScreentoGrid(Vector3 pos)
	{
		return new Vector3Int((int)pos.x,(int)pos.y,(int)pos.z);
	}

	public bool Placeable(Vector3 pos)
	{
		return (0 < (int)pos.x && (int)pos.x <= Width
				&& 0 < (int)pos.y && (int)pos.y <= Height
					&& 0 < (int)pos.z && (int)pos.z <= Depth)
						&& m_grid[(int)pos.x, (int)pos.y, (int)pos.z] == null;
	}

	public void DeleteGrid(Vector3Int pos)
	{
		Destroy(m_grid[pos.x,pos.y,pos.z].gameObject);
		m_grid[pos.x, pos.y, pos.z] = null;
	}

	public void UpdateGrid(Transform transform)
	{
		Debug.Log("ha?");
		for(int y = 0; y < Height; ++y)
			for(int x = 0; x < Width; ++x)
				for(int z = 0; z < Depth; ++z)
					if (m_grid[x, y, z] == null)
					{
						if (m_grid[x, y, z].parent == transform)
						{
							m_grid[x, y, z] = null;
						}
					}
		//foreach (Transform child in transform)
		//{
		//	Vector3 pos = ScreentoGrid(child.position);
		//	m_grid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
		//}
	}
}

