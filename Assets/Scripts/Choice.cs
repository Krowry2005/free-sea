using UnityEngine;

public class Choice : MonoBehaviour
{
	GameObject m_mass;


	public void OnChoice()
	{
		// �^�b�v���������ɃJ��������Ray���΂�
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			m_mass = hit.collider.gameObject;
		}
	}	
}
