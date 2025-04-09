using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    private CharecterStatus m_status = null;
    private TableData.TableData_Skill m_data = null;
    public TableData.TableSkill.eTARGET_TYPE TargetType => (TableData.TableSkill.eTARGET_TYPE)this.m_data.target;
    public int TargetCount
    {
        get
        {
            switch((TableData.TableSkill.eTARGET_TYPE)this.m_data.target)
            {
                case TableData.TableSkill.eTARGET_TYPE.Self:
                case TableData.TableSkill.eTARGET_TYPE.Enemy_Select_1:
                case TableData.TableSkill.eTARGET_TYPE.Friendly_Select_1:
                return 1;
                
                case TableData.TableSkill.eTARGET_TYPE.Enemy_Random_2:
                case TableData.TableSkill.eTARGET_TYPE.Friendly_Random_1:
                return 2;

                case TableData.TableSkill.eTARGET_TYPE.Enemy_All:
                case TableData.TableSkill.eTARGET_TYPE.Friendly_All:
                return 3;
            }

            return 1;
        }
    }

    public Skill(uint skillID, CharecterStatus status)
    {
        this.m_status = status;
        this.m_data = ProjectManager.Instance.Table.Skill.GetData(skillID);
    }

    private ulong getDamage()
    {
        switch((TableData.TableSkill.eTYPE)this.m_data.type)
        {
            case TableData.TableSkill.eTYPE.Attack:
            case TableData.TableSkill.eTYPE.Heal:
            {
                return this.m_data.coe * this.m_status.Strength;
            }

            case TableData.TableSkill.eTYPE.Status:
            break;
            case TableData.TableSkill.eTYPE.Summon:
            break;
        }

        return this.m_data.coe * this.m_status.Strength;
    }

    public bool isValiedTatget(BaseCharacter charTarget)
    {
        //TODO 아군 적군 확인

        return true;
    }

    public void UseSkill(List<BaseCharacter> listTarget)
    {
        switch((TableData.TableSkill.eTYPE)this.m_data.type)
        {
            case TableData.TableSkill.eTYPE.Attack:
            {
                for(int i = 0, nMax = listTarget.Count; i < nMax; ++i)
                {
                    listTarget[i].Damaged(this.getDamage());
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Heal:
            {
                for(int i = 0, nMax = listTarget.Count; i < nMax; ++i)
                {
                    listTarget[i].Heal(this.getDamage());
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Status:
            break;
            case TableData.TableSkill.eTYPE.Summon:
            break;
        }
    }
}