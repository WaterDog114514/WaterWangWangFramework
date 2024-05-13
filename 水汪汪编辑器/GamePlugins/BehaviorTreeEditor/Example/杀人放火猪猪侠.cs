using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//测试用的垃圾AI脚本
public class 杀人放火猪猪侠 : MonoBehaviour
{
    public GameObject Player;
    public GameObject BulletPrefab;
    public Vector3[] pos;
    private NavMeshAgent agent;
    private MeshRenderer render;
    private void Awake()
    {
        Player = GameObject.Find("Player"); 
        agent = GetComponent<NavMeshAgent>();
        render = GetComponent<MeshRenderer>();
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] += transform.position;
        }
    }
    public bool b_PlayerInChaseRange()
    {
        return (Player.transform.position - transform.position).magnitude <= 19;
    }
    public bool b_PlayerInATKRange()
    {
        return (Player.transform.position - transform.position).magnitude <= 5;
    }
    public void Atk()
    {
        Instantiate(BulletPrefab, transform.position, Quaternion.LookRotation(Player.transform.position - transform.position));
    }
    int step;
    public void Patrol()
    {
        agent.isStopped = false;
        render.material.color = Color.red;
        agent.SetDestination(pos[step]); ;
    }
    private float nextTime;
    private void UpdateStep()
    {
        if (Time.time <= nextTime) return;
        nextTime = Time.time + 1F;
        step++;
        if (step >= pos.Length) step = 0;
    }
    public void 发呆流鼻涕般的追赶()
    {
        render.material.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
        agent.isStopped = false;
        agent.SetDestination(Player.transform.position);
    }

    private void Update()
    {
        UpdateStep();
    }
}
