using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private UnitManager m_unitManager;
    [SerializeField] private UnitAction m_unitAction;

    /// <summary>
    /// �G���j�b�g�̍s�������s����R���[�`��
    /// </summary>
    public IEnumerator ExecuteEnemyTurn(Unit unit)
    {
        yield return new WaitForSeconds(0.5f); // �����ҋ@���ĉ��o�𐮂���

        // �U���X�L�����擾�i���ɍŏ��̃X�L�����g�p�j
        SkillAttack attackSkill = unit.GetAttackSkill()[0];

        // �U���͈͂̕\���i�K�v�Ȃ�j
        m_unitAction.SetAction(UnitAction.Action.Attack);
        m_unitAction.OnDisplay(attackSkill.GetRange(), true);
        yield return new WaitForSeconds(0.5f);

        // �U���Ώۂ�T��
        GameObject target = FindAttackTarget(unit, attackSkill);
        if (target != null)
        {
            Vector3Int targetPos = Vector3Int.RoundToInt(target.transform.position);

            // ���ʔ͈͂̕\��
            m_unitAction.OnExtent(attackSkill.GetExtent(), targetPos);
            yield return new WaitForSeconds(0.5f);

            // �X�L�������iOnAttack������ŌĂԁj
            m_unitAction.SkillExecution(attackSkill, targetPos);
        }
        else
        {
            // �U���ł��Ȃ� �� �ړ�������
            m_unitAction.SetAction(UnitAction.Action.Move);
            m_unitAction.OnDisplay(unit.GetSkill()[0].GetRange(), false);
            yield return new WaitForSeconds(0.5f);

            Vector3Int movePos = FindClosestMovePosition(unit);
            m_unitAction.OnExtent(unit.GetSkill()[0].GetExtent(), movePos);
            yield return new WaitForSeconds(0.5f);

            m_unitAction.OnAction(movePos);
        }

        // �s���I����A�t�F�[�Y�� End �Ɉڍs
        yield return new WaitForSeconds(1.0f);
        m_unitManager.SetPhase(UnitManager.Phase.End);
    }

    /// �U���\�ȃv���C���[���j�b�g��T��
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
    /// �ł��߂��v���C���[���j�b�g�Ɍ������Ĉړ�����ʒu������
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
