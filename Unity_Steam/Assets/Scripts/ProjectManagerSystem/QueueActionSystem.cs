using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QueueActionSystem : MonoBehaviour
{
    private Queue<UnityAction> m_queueAction = new Queue<UnityAction>();

    public void ResetQueueAction()
    {
        this.m_queueAction.Clear();
    }

    public void AddQueueAction(UnityAction action)
    {
        this.m_queueAction.Enqueue(action);
    }

    public void DoQueueAction()
    {
        //할 거 없으면 ㄴㄴ
        if(this.m_queueAction.Count == 0) return;

        //빼서 바로 실행
        this.m_queueAction.Dequeue().Invoke();
    }
}