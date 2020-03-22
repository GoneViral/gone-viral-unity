using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPCHust : MonoBehaviour
{
    private People m_Base;
    private NavMeshAgent agent;
    
    public float m_fRadius = 5;
    private float m_fTimer;

    public ParticleSystem particle;
    
    void Start()
    {
        m_Base = GetComponent<People>();
        agent = GetComponent<NavMeshAgent>();
        m_fTimer = Time.realtimeSinceStartup + Random.Range(2.0F, 10.0F);
    }

    void Update()
    {
        if (m_fTimer < Time.realtimeSinceStartup)
        {
            if (m_Base.getInfected())
            {
                float fAngle = Vector3.Angle(agent.nextPosition,agent.transform.position);
                OnHustArea(gameObject, fAngle, agent.transform.position,  m_fRadius);
            }
            
            m_fTimer = Time.realtimeSinceStartup + Random.Range(2.0F, 10.0F);
        }
    }
    
    
    //TODO implement showsSymptoms Boolean
    // Wird aufgerufen von infizierten Spieler wenn er hustet
    void OnHustArea(GameObject obj, float fDirection, Vector3 center, float fRadius)
    {
        particle.Play();
        Collider[] hitColliders = Physics.OverlapSphere(center, fRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject != obj && hitColliders[i].gameObject.GetComponent<People>() != null)
            {
                Vector3 position = hitColliders[i].gameObject.transform.position;
                float angle = Vector3.Angle(center, position);
                if (Mathf.DeltaAngle(fDirection, angle) >= 337.5f || Mathf.DeltaAngle(fDirection, angle) <= 22.5f)
                {
                    hitColliders[i].gameObject.SendMessage("AddVirus", 1.0f - (Vector3.Distance(position, center) / fRadius));
                }
            }
            i++;
        }
    }

}