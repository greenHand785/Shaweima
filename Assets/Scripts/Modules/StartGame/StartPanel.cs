using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel: MonoBehaviour
{
    public Animation m_animation;
    public Text txt;
    public Text time;

    private void Start()
    {
        PlayAni();

        EventCenter.AddListener(CombatEventType.Event_GameStart, OnLevelOver);

    }

    private void Update()
    {
        txt.text = GameManager.Instance.GetReleaseTime().ToString("F0");
        time.text = $"第{GameManager.Instance.curLevel +1}年";
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(CombatEventType.Event_GameStart, OnLevelOver);
    }

    void OnLevelOver()
    {
        PlayAni();
    }

    public void PlayAni()
    {
        m_animation.Play("StartPanel");
    }

}

