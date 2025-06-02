using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitAction : MonoBehaviour
{
	Vector3 m_targetPos;
	Animator m_animator;
	Rigidbody m_rigidbody;
	TurnManager m_turnManager;

	GameObject m_turnUnit;

	private void Awake()
	{
		m_turnManager = GetComponent<TurnManager>();
		m_rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		//現在のターンのUnitを更新
		m_turnUnit = m_turnManager.TurnUnit;
		switch (m_turnManager.GetPhase)
		{
			case TurnManager.Phase.Select:
				if(Input.GetMouseButton(0))
				{
					OnSelect(m_turnUnit.transform.position);
				}
				break;

			case TurnManager.Phase.Action:
				


				break;
		}
	}

	public void OnPick()
	{
		// タップした方向にカメラからRayを飛ばす
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			m_targetPos = hit.collider.gameObject.transform.position;
			Debug.Log(m_targetPos);
		}
	}

	public void OnSelect(Vector3 pos)
	{
		
	}

	public void OnMove(Vector3 position)
	{
		m_animator.SetBool("Move", true);
	}

}
