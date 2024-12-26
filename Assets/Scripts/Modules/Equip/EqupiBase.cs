using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqupiBase : ObjectBase
{
    public GameObject equipPos;
    public List<AudioSource> sources;

    
    // Start is called before the first frame update
    void Awake()
    {
        EventCenter.AddListener<bool>(CombatEventType.Event_GameStateChanged, OnGameStateChanged);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool>(CombatEventType.Event_GameStateChanged, OnGameStateChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGameStateChanged(bool state)
    {
        if(sources == null)
        {
            return;
        }
        foreach (var item in sources)
        {
            if (state)
            {
                item.Play();
            }
            else
            {
                item.Stop();
            }
        }
    }
}
