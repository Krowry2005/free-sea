using UnityEngine;

public class ActionSwitch : MonoBehaviour
{
	[SerializeField]
	UnitAction.Action m_action;

	UnitAction m_unitAction;
	UnitManager m_unitManager;

	private void Awake()
	{
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		m_unitAction = gameController.GetComponent<UnitAction>();
		m_unitManager = gameController.GetComponent<UnitManager>();
	}

	public void OnAction() 
	{
		//フェーズをセレクトまで戻す
		m_unitManager.SetPhase(UnitManager.Phase.Select);

		//移動可能表示をオフ
		m_unitAction.OnRemove();

		//アクションの変更
		m_unitAction.SetAction(m_action);
	}
}

