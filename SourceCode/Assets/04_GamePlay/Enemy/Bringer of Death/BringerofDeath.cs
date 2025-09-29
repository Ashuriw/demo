using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerofDeath : BossBase
{
    void Update()
    {
        if(m_target == null)
            DetectTarget();
    }
    public void OnSkill1()
    {
        if (m_target != null)
        {
            transform.position = new Vector3(m_target.transform.position.x, transform.position.y+0.84f, transform.position.z);
        }
    }
    
}
