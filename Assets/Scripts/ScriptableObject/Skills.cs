using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillsData")]
public class Skills : ScriptableObject
{
	public List<SkillData> skill = new();

	public class SkillData
	{
		public int magnification;
	}


}

