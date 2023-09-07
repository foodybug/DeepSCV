using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Policies;

public class SCV_Agent : Agent
{
    [SerializeField] SCV_AI scvAI;

    DLT_VOID_VECTOR3 dltAction;

    private void Awake()
    {
        scvAI.Init();

        BehaviorParameters param = GetComponent<BehaviorParameters>();
        param.BrainParameters.VectorObservationSize = 3 + scvAI.posMineral.Length * 2 + 2;
        MaxStep = 9999 * 5;
    }

    public void Init(DLT_VOID_VECTOR3 dltAction)
    {
        this.dltAction = dltAction;

        //BehaviorParameters param = GetComponent<BehaviorParameters>();
        //param.BrainParameters.VectorObservationSize = 4 + scvAI.posMineral.Length * 3 + 3;
    }

    //���Ǽҵ�(�н�����)�� �����Ҷ����� ȣ��
    public override void OnEpisodeBegin()
    {
        //Debug.Log("OnEpisodeBegin");

        scvAI.Begin();
    }

    //ȯ�� ������ ���� �� ������ ��å ������ ���� �극�ο� �����ϴ� �޼ҵ�
    public override void CollectObservations(Unity.MLAgents.Sensors.VectorSensor sensor)
    {
        sensor.AddObservation(scvAI.cntCarryingMineral == 5);

        sensor.AddObservation(scvAI.posSCV.x);
        //sensor.AddObservation(scvAI.posSCV.y);
        sensor.AddObservation(scvAI.posSCV.z);

        foreach(Vector3 node in scvAI.posMineral)
        {
            sensor.AddObservation(node.x);
            //sensor.AddObservation(node.y);
            sensor.AddObservation(node.z);
        }
        
        sensor.AddObservation(scvAI.posCC.x);
        //sensor.AddObservation(scvAI.posCC.y);
        sensor.AddObservation(scvAI.posCC.z);
    }

    //�극��(��å)���� ���� ���� ���� �ൿ�� �����ϴ� �޼ҵ�
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 dir = new Vector3(vectorAction[0], 0f, vectorAction[1]);
        
        if (dltAction != null)
            dltAction(dir);
    }

    //������(�����)�� ���� ����� ������ ȣ���ϴ� �޼ҵ�(�ַ� �׽�Ʈ�뵵 �Ǵ� ����н��� ���)
    public override void Heuristic(float[] actionsOut)
    {
        //int action = (int)actionsOut[0];
        //board.ClickAction(this, action);
    }
}
