using UnityEngine;

public class Choice : MonoBehaviour
{
	GameObject m_mass;


	public void OnChoice()
	{
		// タップした方向にカメラからRayを飛ばす
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			m_mass = hit.collider.gameObject;
		}
	}	
}
