using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanelControl : MonoBehaviour
{
    public SkillSystem[] skills;
    // Start is called before the first frame update
    void Start()
    {
        skills = GetComponentsInChildren<SkillSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
