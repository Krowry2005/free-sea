using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private UnitManager m_unitManager;
    [SerializeField] private UnitAction m_unitAction;

    /// <summary>
    /// 敵ユニットの行動を実行するコルーチン
    /// </summary>
    public IEnumerator ExecuteEnemyTurn(Unit unit)
    {
        yield return new WaitForSeconds(0.5f); // 少し待機して演出を整える

        // 攻撃スキルを取得（仮に最初のスキルを使用）
        SkillAttack attackSkill = unit.GetAttackSkill()[0];

        // 攻撃範囲の表示（必要なら）
        m_unitAction.SetAction(UnitAction.Action.Attack);
        m_unitAction.OnDisplay(attackSkill.GetRange(), true);
        yield return new WaitForSeconds(0.5f);

        // 攻撃対象を探す
        GameObject target = FindAttackTarget(unit, attackSkill);
        if (target != null)
        {
            Vector3Int targetPos = Vector3Int.RoundToInt(target.transform.position);

            // 効果範囲の表示
            m_unitAction.OnExtent(attackSkill.GetExtent(), targetPos);
            yield return new WaitForSeconds(0.5f);

            // スキル発動（OnAttackを内部で呼ぶ）
            m_unitAction.SkillExecution(attackSkill, targetPos);
        }
        else
        {
            // 攻撃できない → 移動処理へ
            m_unitAction.SetAction(UnitAction.Action.Move);
            m_unitAction.OnDisplay(unit.GetSkill()[0].GetRange(), false);
            yield return new WaitForSeconds(0.5f);

            Vector3Int movePos = FindClosestMovePosition(unit);
            m_unitAction.OnExtent(unit.GetSkill()[0].GetExtent(), movePos);
            yield return new WaitForSeconds(0.5f);

            m_unitAction.OnAction(movePos);
        }

        // 行動終了後、フェーズを End に移行
        yield return new WaitForSeconds(1.0f);
        m_unitManager.SetPhase(UnitManager.Phase.End);
    }

    /// 攻撃可能なプレイヤーユニットを探す
    private GameObject FindAttackTarget(Unit attacker, SkillAttack skill)
    {
        foreach (GameObject other in m_unitManager.UnitList)
        {
            Unit target = other.GetComponent<Unit>();
            if (target.FriendLevel != UnitsSetting.UnitData.FriendLevel.Enemy)
            {
                float dest = Vector3.Distance(attacker.transform.position, target.transform.position);
                foreach (Vector3Int skillRange in skill.GetRange())
                { 
                    if(dest <= Vector3.Distance(attacker.transform.position,skillRange))
                    {
                        return other;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 最も近いプレイヤーユニットに向かって移動する位置を決定
    /// </summary>
    private Vector3Int FindClosestMovePosition(Unit unit)
    {
        GameObject closest = m_unitManager.UnitList
            .Where(u => u.GetComponent<Unit>().FriendLevel != UnitsSetting.UnitData.FriendLevel.Enemy )
            .OrderBy(u => Vector3.Distance(unit.transform.position, u.transform.position))
            .FirstOrDefault();

        if (closest != null)
        {

            Vector3 dir = (closest.transform.position - unit.transform.position).normalized;
            Vector3Int movePos = Vector3Int.RoundToInt(unit.transform.position + dir);
            return movePos;
        }
        return Vector3Int.RoundToInt(unit.transform.position);
    }
}
