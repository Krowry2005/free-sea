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

	//UŒ‚”{—¦
	[SerializeField]
	int magnification;

	//UŒ‚‰ñ”
	[SerializeField]
	int attackNumTime;

	//ƒtƒŒƒ“ƒhƒŠƒtƒ@ƒCƒA
	[SerializeField]
	bool friendlyFire;

	public AttackType GetAttackType()
		{ return attackType; }

	public AttackWay GetAttackWay()
		{ return attackWay; }

	public int GetMagnification()
		{ return magnification; }

	public int GetAttackNumTime() {
		return attackNumTime; }

	public bool GetFriendlyFire() {
		return friendlyFire; }
}
