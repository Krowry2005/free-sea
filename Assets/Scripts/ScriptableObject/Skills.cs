
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Skill", menuName = "CreateSkill")]
public class Skills : ScriptableObject
{
	public enum Type
	{
		Attack,	
		Move,
		Guard,
		Item,
		Buff,
		DeBuff,
		Damage,
		Heal,
	}

	//スキルのタイプ
	[SerializeField]
	private Type skillType;

	//スキルのID
	[SerializeField]
	private int skillId = 0;

	//漢字のスキル名
	[SerializeField]
	private string kanjiName = "";

	//ひらがなのスキル名
	[SerializeField]
	private string hiraganaName = "";

	//消費SP
	[SerializeField]
	int sp = 0;

	[SerializeField]
	bool extentAttack = false;

	//情報
	[SerializeField]
	private string information = "";

	//距離
	[SerializeField]
	Vector3Int[] range = { new (-1,0,1),new (0,0,1),new (1,0,1),new (-1,0,0),new (1,0,0),new (-1,0,-1),new (0,0,-1),new (1,0,-1)};

	//範囲
	[SerializeField]
	Vector3Int[] extent = { new(0, 0, 0),};

	//　使用者のエフェクト
	[SerializeField]
	private GameObject skillUserEffect = null;

	//　スキルの種類を返す
	public Type GetSkillType()
	{
		return skillType;
	}

	//スキルのID
	public int GetID()
	{
		return skillId;
	}

	//　スキルの名前を返す
	public string GetKanjiName()
	{
		return kanjiName;
	}

	//　スキルの平仮名の名前を返す
	public string GetHiraganaName()
	{
		return hiraganaName;
	}

	//消費MP
	public int GetMP()
	{
		return sp;
	}

	public bool GetExtentAttack()
	{
		return extentAttack;
	}

	//　スキル情報を返す
	public string GetInformation()
	{
		return information;
	}

	//距離を返す
	public Vector3Int[] GetRange()
	{
		return range;
	}

	//範囲を返す
	public Vector3Int[] GetExtent()
	{
		return extent;
	}

	//　使用者のエフェクトを返す
	public GameObject GetSkillUserEffect()
	{
		return skillUserEffect;
	}
}