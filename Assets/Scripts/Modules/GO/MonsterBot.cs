using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Reflection;
using UnityEngine;

public class MonsterBot : BotBase
{
    public float StiffTime=0.08F;

    public int SteaGold;      //偷錢能力
    public int OffensivePower;//攻擊力

    public AudioSource source;
    public AudioClip death;
    public AudioClip getGold;

    private bool isStiff = false;            // 是否处于僵硬状态
    protected override void Init()
    {
        base.Init();
        CanSurvivalTime = 9999;
    }


    public GameObject attackEffectPrefab;
    void Start()
    {
        Init();

        botState=BotState.Walk;
        tempWayPos.Add(transform.position);
        tempWayPos.Add(GoldSystem.Instance.transform.position);
        mover.SetPathPoints(tempWayPos);
    }

    private void CheAttack()
    {
        if(botState!=BotState.Dead&&botState!=BotState.BeDamege)    
        {
            if(Vector3.Distance(transform.position, GoldSystem.Instance.transform.position)<1)
            {
                botState=BotState.Attack;
            }
        }
    }

    // <summary>
    /// 处理怪物被攻击逻辑
    /// </summary>
    public void BeAttacked()
    {
        if (botState != BotState.Dead && !isStiff) // 只有怪物未死亡且不处于僵硬状态时才处理
        {
            isStiff = true; // 设置为僵硬状态

            // 播放受攻击动画
            if (animator != null)
            {
                animator.SetTrigger("BeDamegeTrigger");
            }

            // 停止移动并启动僵硬恢复协程
            StartCoroutine(RecoveryFromStiff());
        }
    }
    /// <summary>
    /// 僵硬状态恢复协程
    /// </summary>
    private IEnumerator RecoveryFromStiff()
    {
        // 停止移动
        mover.enabled=false;
        botState=BotState.BeDamege;
        // 等待僵硬时间
        yield return new WaitForSeconds(StiffTime);
        // 恢复正常移动
        isStiff = false;
        mover.enabled=true; // 恢复移动功能
        // 切换回行走状态
        botState = BotState.Walk;
        
    }


    /// <summary>
    /// 播放攻击特效（动画事件调用此方法）
    /// </summary>
    public void PlayAttackEffect()
    {
        if (botState==BotState.Dead) return;
        if (attackEffectPrefab != null )
        {
            // 在目标点生成特效
            GameObject attackEffect = Instantiate(attackEffectPrefab, attackEffectPrefab.transform.position, Quaternion.identity);
            // 销毁特效（延迟 2 秒）
            Destroy(attackEffect, 1f);
            if (type== ObjectType.自爆机器人) 
            {
                botState=BotState.Dead;
                Destroy(gameObject, 1f);
            } 
            else  if (type== ObjectType.偷钱佬) 
            {
                //减金币
                PlaySound(getGold);
            }
            else  if (type== ObjectType.快速级绿色小怪) 
            {
                //减血量
                //GameManager.Instance.m_Ship.Injured(OffensivePower);
                //GoldSystem.Instance.SubGold(SteaGold);

            }
            if (type== ObjectType.慢速黄色小怪) 
            {
                //减血量
                //减血量

            }

            GameManager.Instance.m_Ship.Injured(OffensivePower);
            GoldSystem.Instance.SubGold(SteaGold);

            //应用攻击效果
        }
    }
    // Update is called once per frame
    void Update()
    {
      base.Update();  
      CheAttack();
      if (botState != BotState.Dead&& HP <= 0) 
      {
            botState = BotState.Dead;
            PlaySound(death);
      }
    }


    private void PlaySound(AudioClip clip)
    {
        if(clip == null ||source == null)
        {
            return;
        }
        source.clip = clip;
        source.Play();
    }
}
