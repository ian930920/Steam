using UnityEngine;
using System.Collections;

public class Status
{
    public TableData.TableStatus.eID eStatusID { get; private set; }

    public int RemainTurn { get; private set; } = 0;

    public Status(uint statusID, int turn)
    {
        this.eStatusID = (TableData.TableStatus.eID)statusID;
        this.RemainTurn = turn;
    }

    public void AddTurn(int turn)
    {
        this.RemainTurn += turn;
    }

    public void UpdateTurn()
    {
        if(this.RemainTurn < 1) return;

        this.RemainTurn--;

        //TODO 상태이상 실행
        ProjectManager.Instance.Log($"{eStatusID} 실행!");
    }

    public void AddStatus(BaseUnit character)
    {
        switch(eStatusID)
        {
            case TableData.TableStatus.eID.Shield:
            {
                //받는 피해 흡수
                //턴끝났으면 지우기
                //character.AddShield(value);
            }
            break;

            case TableData.TableStatus.eID.Transcendence:
            {
                //이번 턴 동안 소환수 소환에 필요한 모든 마나 비용 1 감소. 비용은 최소 1까지만 감소할 수 있음
            }
            break;

            case TableData.TableStatus.eID.Add_Mana:
            {
                //사용 즉시 마나 2 회복
                character.AddMana(2);
            }
            break;
        }
    }

    public void DoStatus(BaseUnit character)
    {
        float fValue = TableManager.Instance.Status.GetValue(this.eStatusID);
        switch(eStatusID)
        {
            case TableData.TableStatus.eID.Burn:
            {
                //매 턴 시작 시 최대 체력의 10%의 피해를 받음
                character.Damaged(new stDamage((int)(character.DefaultStat.GetStat(Stat_Character.eTYPE.HP) * fValue)));
            }
            break;

            case TableData.TableStatus.eID.Bleeding:
            {
                //매 턴 시작 시 최대 체력의 10%의 피해를 받음
                character.Damaged(new stDamage((int)(character.DefaultStat.GetStat(Stat_Character.eTYPE.HP) * fValue)));
            }
            break;

            case TableData.TableStatus.eID.Fainting:
            case TableData.TableStatus.eID.Freezing:
            {
                //공격 할때 대상자에서 적용
            }
            break;

            case TableData.TableStatus.eID.Dark:
            {
                //다음 공격 1회는 반드시 빗나감
                //캐릭터가 스킬 사용할 때 적용
            }
            break;

            case TableData.TableStatus.eID.Curse:
            {
                //매 턴 시작 시 회복되는 마나 1 감소
                character.UseMana((int)fValue);
            }
            break;

            case TableData.TableStatus.eID.Weakened_Def:
            {
                //받는 피해 50% 증가
                //공격 받을 때 적용
            }
            break;

            case TableData.TableStatus.eID.Weakened_Atk:
            {
                //최종 피해 50% 감소
                //공격 받을 때 적용
            }
            break;

            case TableData.TableStatus.eID.Weakened_Hit:
            {
                //명중률 50% 감소
                //공격 할 때 적용
            }
            break;

            case TableData.TableStatus.eID.Regeneration:
            {
                //매 턴 시작 시 최대 체력의 10% 회복
                character.Heal(new stDamage((int)(character.DefaultStat.GetStat(Stat_Character.eTYPE.HP) * fValue)));
            }
            break;

            case TableData.TableStatus.eID.Shield:
            {
                //받는 피해 흡수
                //턴끝났으면 지우기
                if(this.RemainTurn == 0) character.ResetShield();
            }
            break;

            case TableData.TableStatus.eID.Transcendence:
            {
                //이번 턴 동안 소환수 소환에 필요한 모든 마나 비용 1 감소. 비용은 최소 1까지만 감소할 수 있음
            }
            break;

            case TableData.TableStatus.eID.Rage:
            {
                //다음 공격 1회는 반드시 치명타
                //공격 할 때 적용
            }
            break;

            case TableData.TableStatus.eID.Blessing:
            {
                //매 턴 시작 시 회복되는 마나 1 증가
                character.AddMana((int)fValue);
            }
            break;

            case TableData.TableStatus.eID.Defense_Enhancement:
            {
                //받는 피해 50% 감소
            }
            break;

            case TableData.TableStatus.eID.Attack_Enhancement:
            {
                //최종 피해 50% 증가
            }
            break;

            case TableData.TableStatus.eID.Summon_Protection:
            {
                //다른 아군에 의해 보호받는 상태. 받는 피해를 보호 시전자에게 이전
            }
            break;
        }
    }
}