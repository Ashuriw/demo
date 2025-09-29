using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    [Header("Effects")]
    public GameObject           m_RunStopDust;
    public GameObject           m_DodgeDust;
    public GameObject           m_WallJumpDust;
    public GameObject           m_AttackHitBox;

    private PlayerController       m_player;
    private AudioManager m_audioManager;

    void Start()
    {
        m_player = GetComponentInParent<PlayerController>();
        m_audioManager = AudioManager.instance;
    }
    
    void AE_Footstep()
    {
        m_audioManager.PlaySound("Footstep");
    }
    void AE_RunStop()
    {
        m_audioManager.PlaySound("RunStop");
        float dustXOffset = 0.6f;
        float dustYOffset = 0.078125f;
        m_player.SpawnDustEffect(m_RunStopDust, dustXOffset, dustYOffset);
    }
    void AE_Jump()
    {
        m_audioManager.PlaySound("Jump");
        
        m_player.SpawnDustEffect(m_WallJumpDust);
    }
    void AE_Dodge()
    {
        m_audioManager.PlaySound("Dodge");
        float dustYOffset = 0.78f;
        m_player.SpawnDustEffect(m_DodgeDust, 0.0f, dustYOffset);
    }
    void AE_SwordAttack()
    {
        m_audioManager.PlaySound("SwordAttack");
        GameObject hitBox=Instantiate(m_AttackHitBox,transform.position,Quaternion.identity);
        HitBox hitBoxComponent=hitBox.GetComponent<HitBox>();
        hitBoxComponent.ActivateHitBox(m_player.Headed);
    }

    void AE_Combo()
    {
        m_player.OnSpecialStateAnimationEnd();
    }
    void AE_HitEnd()
    {
        m_player.ExitSpecialState();
    }
}
