using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "MoveAttack", menuName = "CreateMoveAttack")]
public class MoveAttack : AttackSkill
{
	//ˆÚ“®”ÍˆÍ
	[SerializeField]
	Vector3[] MoveRange;

	public Vector3[] GetMoveRange()
	{
		return MoveRange;
	}
}
