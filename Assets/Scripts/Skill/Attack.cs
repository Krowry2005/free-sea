using UnityEngine;


public class Attack : MonoBehaviour
{
	public void OnAttack(GameObject[] damageUnit ,int damage)
	{
		foreach (GameObject unit in damageUnit)
		{
			unit.GetComponent<Unit>().Damage(damage);
		}
	}
}
