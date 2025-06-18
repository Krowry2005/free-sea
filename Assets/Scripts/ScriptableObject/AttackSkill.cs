using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Attack", menuName = "CreateAttackSkill")]
public class AttackSkill : Skills
{
	public enum AttackType
	{
		Nomal,		//何の変哲もない通常攻撃
		Continuou,  //連続攻撃
		Extent,     //範囲攻撃

		Other,
	}

	public enum AttackWay
	{
		Attack,
		MaxHP,
		hp,
		Defense,
	}

	//攻撃の種類
	[SerializeField]
	AttackType attackType;

	//参照ステータス
	[SerializeField]
	AttackWay attackWay;

	//攻撃倍率
	[SerializeField]
	int magnification;

	//攻撃回数
	[SerializeField]
	int attackNumTime;

	//フレンドリファイア
	[SerializeField]
	bool friendlyFire;

	public AttackType GetAttackType()
		{ return attackType; }

	public AttackWay GetAttackWay()
		{ return attackWay; }

	public int GetMagnification()
		//％表記
		{ return magnification / 100; }

	public int GetAttackNumTime() {
		return attackNumTime; }

	public bool GetFriendlyFire() {
		return friendlyFire; }
}
