using UnityEngine;

public class Unit : MonoBehaviour
{
	GameObject m_gameController;
	GameObject m_target;

	Grid m_grid;

	void Start()
    {
		m_gameController = GameObject.FindGameObjectWithTag("GameController");
		m_grid = m_gameController.GetComponent<Grid>();
		m_grid.UpdateGrid(transform);
    }

    void Update()
    {
		//if (Input.GetMouseButtonDown(0))
		//{
		//	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//	RaycastHit hit = new RaycastHit();
		//	if(Physics.Raycast(ray,out hit))
		//	{
		//		m_target = hit.collider.gameObject;
		//	}
		//}
		//Debug.Log(m_target);
    }
}
