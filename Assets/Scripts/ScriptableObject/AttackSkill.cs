using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Attack", menuName = "CreateAttackSkill")]
public class AttackSkill : Skills
{
	public enum AttackType
	{
		Nomal,		//���̕ϓN���Ȃ��ʏ�U��
		Continuou,  //�A���U��
		Extent,     //�͈͍U��

		Other,
	}

	public enum AttackWay
	{
		Attack,
		MaxHP,
		hp,
		Defense,
	}

	//�U���̎��
	[SerializeField]
	AttackType attackType;

	//�Q�ƃX�e�[�^�X
	[SerializeField]
	AttackWay attackWay;

	//�U���{��
	[SerializeField]
	int magnification;

	//�U����
	[SerializeField]
	int attackNumTime;

	//�t�����h���t�@�C�A
	[SerializeField]
	bool friendlyFire;

	public AttackType GetAttackType()
		{ return attackType; }

	public AttackWay GetAttackWay()
		{ return attackWay; }

	public int GetMagnification()
		//���\�L
		{ return magnification / 100; }

	public int GetAttackNumTime() {
		return attackNumTime; }

	public bool GetFriendlyFire() {
		return friendlyFire; }
}
