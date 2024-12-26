using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour
{
    public ParticleSystem[] skillEffects;
    public float skillHarm;

    // Start is called before the first frame update
    void Start()
    {
        skillEffects = transform.GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseSkill()
    {
        if (skillEffects.Length == 0) return;

        foreach (var item in skillEffects)
        {
            item.Play();
        }

        Collider[] items = Physics.OverlapSphere(transform.position, 2.5f);

        StartCoroutine(SetHarm(items));
    }

    IEnumerator SetHarm(Collider[] items)
    {
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].transform.root.tag == "Enemy")
            {
                items[i].transform.root.GetComponent<ObjectBase>().Injured(skillHarm);
            }
        }

        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
