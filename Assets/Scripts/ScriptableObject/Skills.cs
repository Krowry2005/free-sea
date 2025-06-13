
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
	private int skillId;

	//漢字のスキル名
	[SerializeField]
	private string kanjiName = "";

	//ひらがなのスキル名
	[SerializeField]
	private string hiraganaName = "";

	//消費MP 
	[SerializeField]
	int mp;

	//情報
	[SerializeField]
	private string information = "";

	//範囲
	[SerializeField]
	Vector3Int[] Range = { new (-1,0,1),new (0,0,1),new (1,0,1),new (-1,0,0),new (1,0,0),new (-1,0,-1),new (0,0,-1),new (1,0,-1)};

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
		return mp;
	}

	//　スキル情報を返す
	public string GetInformation()
	{
		return information;
	}

	//範囲を返す
	public Vector3Int[] GetRange()
	{
		return Range;
	}

	//　使用者のエフェクトを返す
	public GameObject GetSkillUserEffect()
	{
		return skillUserEffect;
	}
}