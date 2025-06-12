using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Attack", menuName = "CreateAttackSkill")]
public class AttackSkill : Skills
{
	public enum AttackType
	{
		Nomal,		//‰½‚Ì•Ï“N‚à‚È‚¢’ÊíUŒ‚
		Continuou,  //˜A‘±UŒ‚
		Extent,     //”ÍˆÍUŒ‚
		MoveAttack,

		Other,
	}

	public enum AttackWay
	{
		Attack,
		MaxHP,
		hp,
		Defense,
	}

	//UŒ‚‚Ìí—Ş
	[SerializeField]
	AttackType attackType;

	//QÆƒXƒe[ƒ^ƒX
	[SerializeField]
	AttackWay attackWay;

	//UŒ‚‚ª“Í‚­‹——£
	[SerializeField]
	Vector3[] skillRange = { new(-1,0,1),new(0,0,1),new(1,0,1),new(-1,0,0),
											new(1,0,0),new(-1,0,-1),new(0,0,-1),new(1,0,-1)};

	//UŒ‚‚Ì”ÍˆÍ
	[SerializeField]
	Vector3[] extent;

	//UŒ‚”{—¦
	[SerializeField]
	int magnification;

	//UŒ‚‰ñ”
	[SerializeField]
	int attackNumTime;

	//ƒtƒŒƒ“ƒhƒŠƒtƒ@ƒCƒA
	[SerializeField]
	bool friendlyFire;
}
