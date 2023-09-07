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

    //에피소드(학습단위)가 시작할때마다 호출
    public override void OnEpisodeBegin()
    {
        //Debug.Log("OnEpisodeBegin");

        scvAI.Begin();
    }

    //환경 정보를 관측 및 수집해 정책 결정을 위해 브레인에 전달하는 메소드
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

    //브레인(정책)으로 부터 전달 받은 행동을 실행하는 메소드
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 dir = new Vector3(vectorAction[0], 0f, vectorAction[1]);
        
        if (dltAction != null)
            dltAction(dir);
    }

    //개발자(사용자)가 직접 명령을 내릴때 호출하는 메소드(주로 테스트용도 또는 모방학습에 사용)
    public override void Heuristic(float[] actionsOut)
    {
        //int action = (int)actionsOut[0];
        //board.ClickAction(this, action);
    }
}
