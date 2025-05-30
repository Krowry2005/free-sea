using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	Vector3 m_position;
	Vector3 m_targetPosition;
	Animator m_animator;
	Rigidbody m_rigidbody;
	TurnManager m_turnManager;

	private void Awake()
	{
		m_animator = GetComponent<Animator>();
		m_rigidbody = GetComponent<Rigidbody>();
		m_turnManager = GetComponent<TurnManager>();
	}

	private void Update()
	{
		switch (m_turnManager.GetPhase)
		{
			case TurnManager.Phase.Select:
				if(Input.GetMouseButton(0))
				{
					OnSelect();
				}
				break;

			case TurnManager.Phase.Action:
				
				break;
		}
	}

	public void OnSelect()
	{
		// タップした方向にカメラからRayを飛ばす
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			m_position = hit.collider.gameObject.transform.position;
		}
	}

	public void OnMove(Vector3 position)
	{
		m_animator.SetBool("Move", true);
	}

}
