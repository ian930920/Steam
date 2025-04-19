using UnityEngine;
using System.Collections;

public class Status
{
    public TableData.TableStatus.eID eStatusID { get; private set; }

    public ulong RemainTurn { get; private set; } = 0;

    public Status(uint statusID, ulong turn)
    {
        this.eStatusID = (TableData.TableStatus.eID)statusID;
        this.RemainTurn = turn;
    }

    public void AddTurn(ulong turn)
    {
        this.RemainTurn += turn;
    }

    public void UpdateTurn()
    {
        if(this.RemainTurn < 1) return;

        this.RemainTurn--;

        //TODO 상태이상 실행
        ProjectManager.Instance.Log($"{eStatusID} 실행!");

        //TODO UI 갱신
    }

    public void DoStatus(BaseCharacter character)
    {
        switch(eStatusID)
        {
            case TableData.TableStatus.eID.Burn:
            {
                //매 턴 시작 시 최대 체력의 10%의 피해를 받음
                character.Damaged(new stDamage((ulong)(character.Stat.HP * 0.1f), false));
            }
            break;
            case TableData.TableStatus.eID.Bleeding:
            {
                //매 턴 시작 시 최대 체력의 10%의 피해를 받음
                character.Damaged(new stDamage((ulong)(character.Stat.HP * 0.1f), false));
            }
            break;
            case TableData.TableStatus.eID.Fainting:
            {
                //행동 불가
                character.TurnFinish();
            }
            break;
            case TableData.TableStatus.eID.Dark:
            {
                //다음 공격 1회는 반드시 빗나감
            }
            break;
            case TableData.TableStatus.eID.Curse:
            {
                //매 턴 시작 시 회복되는 마나 1 감소
            }
            break;
            case TableData.TableStatus.eID.Weakened_Def:
            {
                //받는 피해 50% 증가
            }
            break;
            case TableData.TableStatus.eID.Weakened_Atk:
            {
                //최종 피해 50% 감소
            }
            break;
            case TableData.TableStatus.eID.Weakened_Hit:
            {
                //명중률 50% 감소
            }
            break;
        }
    }
}