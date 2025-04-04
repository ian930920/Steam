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
        //�� �� ������ ����
        if(this.m_queueAction.Count == 0) return;

        //���� �ٷ� ����
        this.m_queueAction.Dequeue().Invoke();
    }
}