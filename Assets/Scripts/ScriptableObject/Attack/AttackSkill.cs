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

	//�U���̎��
	[SerializeField]
	AttackType attackType;

	//�Q�ƃX�e�[�^�X
	[SerializeField]
	AttackWay attackWay;

	//�U�����͂�����
	[SerializeField]
	Vector3[] skillRange = { new(-1,0,1),new(0,0,1),new(1,0,1),new(-1,0,0),
											new(1,0,0),new(-1,0,-1),new(0,0,-1),new(1,0,-1)};

	//�U���͈̔�
	[SerializeField]
	Vector3[] extent;

	//�U���{��
	[SerializeField]
	int magnification;

	//�U����
	[SerializeField]
	int attackNumTime;

	//�t�����h���t�@�C�A
	[SerializeField]
	bool friendlyFire;
}
