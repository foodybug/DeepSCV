using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCV_AI : MonoBehaviour
{
    [SerializeField] SCV_Agent agent;

    [SerializeField] int cntMineral = 5;
    [SerializeField] SCV_Object scv;
    [SerializeField] GameObject[] mineral;
    [SerializeField] GameObject CC;

    [SerializeField] float genCenter_Mineral;
    [SerializeField] float genCenter_CC;

    [SerializeField] float speed = 2f;

    public SCV_Agent scvAgent { get; private set; }

    public Vector3 posSCV { get { return scv.transform.localPosition; } }
    public Vector3[] posMineral { get {
            Vector3[] ar = new Vector3[mineral.Length];
            for(int i=0; i<mineral.Length; ++i)
            {
                ar[i] = mineral[i].transform.localPosition;
            }
            return ar; } }
    public Vector3 posCC { get { return CC.transform.localPosition; } }

    //public bool carryingMineral { get; private set; } = false;

    public int cntCarryingMineral { get; private set; } = 0;

    public static int countWin { get; private set; } = 0;
    public static int countLose { get; private set; } = 0;
    public static int totalStack { get; private set; } = 0;

    Vector3 initPos_SCV;
    Vector3[] initPos_Mineral;
    Vector3 initPos_CC;

    Vector3 dir;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public void Init()
    {
        GameObject obj = mineral[0];
        mineral = new GameObject[cntMineral];

        mineral[0] = obj;
        for (int i = 1; i < cntMineral; ++i)
        {
            mineral[i] = Instantiate(obj);
            mineral[i].transform.SetParent(transform);
        }

        initPos_Mineral = new Vector3[cntMineral];

        initPos_SCV = scv.transform.localPosition;
        for (int i = 0; i < cntMineral; ++i)
        {
            initPos_Mineral[i] = mineral[i].transform.localPosition;
        }

        initPos_CC = CC.transform.localPosition;

        agent.Init(OnReceive_AgentAction);
        scv.Init(OnReceive_AgentCollision);
    }

    private void Start()
    {
        Begin();
    }

    private void Update()
    {
        scv.transform.Translate(dir * Time.deltaTime * speed);
    }

    public void Begin()
    {
        scv.transform.localPosition = initPos_SCV;
        //for (int i = 0; i < mineral.Length; ++i)
        //{
        //    mineral[i].SetActive(true);
        //    mineral[i].transform.localPosition = initPos_Mineral[i];
        //}   
        //CC.transform.localPosition = initPos_CC;

        float radius = genCenter_Mineral - 2f;
        Vector3 v;
        for (int i = 0; i < mineral.Length; ++i)
        {
            mineral[i].SetActive(true);
            v = Random.onUnitSphere;
            v.y = 0f;
            mineral[i].transform.localPosition = v.normalized * radius;

            radius += 1f;
        }
        v = Random.onUnitSphere;
        v.y = 0f;
        CC.transform.localPosition = v.normalized * genCenter_CC;

        cntCarryingMineral = 0;
    }

    void OnReceive_AgentAction(Vector3 dir)
    {
        this.dir = dir.normalized;
    }

    void OnReceive_AgentCollision(Collider other)
    {
        switch (other.tag)
        {
            case "CC":
                if (cntCarryingMineral == 5)
                {
                    float reward = 1f;// 0.01f * Mathf.Pow(cntCarryingMineral - 1, 3f) - 0.01f;
                    Debug.Log("Collision. other.tag = " + other.tag + ", mineral = " + cntCarryingMineral + ", reward = " + reward);

                    agent.SetReward(1f);
                }
                else
                    agent.SetReward(-0.2f);

                //Begin();
                agent.EndEpisode();
                break;
            default:
            case "Wall":
                Debug.Log("Collision. other.tag = " + other.tag);

                agent.SetReward(-0.2f);

                //Begin();
                agent.EndEpisode();
                break;
            case "Mineral":
                //if (carryingMineral == false)
                //{
                    Debug.Log("Collision. other.tag = " + other.tag);

                    //agent.AddReward(0.1f);

                    other.gameObject.SetActive(false);
                    ++cntCarryingMineral;
                //}
                break;
        }
    }

    void FixedUpdate()
    {
        agent.AddReward(-0.00001f);
    }
}

//public enum eActionCode { NONE = -1, Fold = 0, SmallBlind = 1, BigBlind = 2,

//}

public delegate void DLT_VOID_VOID();
public delegate void DLT_VOID_VECTOR3(Vector3 v);
public delegate void DLT_VOID_COLLIDER(Collider other);
public delegate void DLT_VOID_COLLISIONTYPE(eCollisionType e);
public delegate void DLT_VOID_INT(int n);
public delegate float DLT_FLOAT_VOID();