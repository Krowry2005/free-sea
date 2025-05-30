using UnityEngine;
using static UnitsSetting.UnitData;

public class Generator : MonoBehaviour
{
	[Header("ユニットが召喚されるトランスフォーム"),SerializeField]
	GameObject m_allyParent;
	[SerializeField]
	GameObject m_neutralParent;
	[SerializeField]
	GameObject m_enemyParent;

	public void Awake()
	{
	}

	public void OnGenerate(GameObject unit, Vector3 initial, Quaternion quaternion, FriendLevel friendLevel)
	{
		Transform parent = m_neutralParent.transform;
		switch (friendLevel)
		{
			case FriendLevel.Ally:
				parent = m_allyParent.transform;
				break;

			case FriendLevel.Neutral:
				parent = m_neutralParent.transform;
				break;

			case FriendLevel.Enemy:
				parent = m_enemyParent.transform;
				break;
		}
		Instantiate(unit, initial, quaternion, parent);
	}
}
