using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DestroyTerrain : MonoBehaviour {

	CircleCollider2D c;
    public Action<Vector3, float> creused;
    ParticleSystem ps;
    Cinemachine.CinemachineBasicMultiChannelPerlin cameraNoise;
    public AudioClip[] clips;
    AudioSource source;
    public AudioClip magicClip;
    private void Start()
	{
        cameraNoise = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        cameraNoise.m_FrequencyGain = 0;
		c = GetComponent<CircleCollider2D>();
        ps = GetComponentInChildren<ParticleSystem>();
        source = GetComponent<AudioSource>();
    }
    public bool MAGIC_MIKE = false;
    private bool soundAvailable = true;
    public bool canMagic = true;

    private void Update()
    {
        if(Input.GetMouseButtonUp(1) && !MAGIC_MIKE && canMagic)
        {
            canMagic = false;
            MAGIC_MIKE = true;
            c.radius *= 2f;
            source.PlayOneShot(magicClip,0.6f);
            Invoke("StopMagicMike", 0.5f);
            Invoke("CanMagicAgain", 3f);
        }
    }

    void CanMagicAgain()
    {
        canMagic = true;
    }

    public void StopMagicMike()
    {
        MAGIC_MIKE = false;
        c.radius *= 0.5f;
        cameraNoise.m_FrequencyGain = 1f;
    }

    private void OnTriggerStay2D(Collider2D other)
	{
        if (other is PolygonCollider2D && (Input.GetMouseButton(0) || MAGIC_MIKE))
        {
            WormsTerrain terrain = other.GetComponent<WormsTerrain>();
            if (terrain != null)
            {
                bool hard = terrain.DestroyGround(c);
                // cameraNoise.m_AmplitudeGain = MAGIC_MIKE?1.25f:(hard?1f:0.8f);
                cameraNoise.m_FrequencyGain = MAGIC_MIKE?2.25f:(hard?5.5f:1f);
                if (creused != null)
                    creused.Invoke(transform.position, MAGIC_MIKE?1.25f:(hard?0.25f:1f));
				if(ps != null)
                {
                    Color r = UnityEngine.Random.ColorHSV(0.25f,0.75f,0.85f,1f,0.7f,1f);
	                ps.startColor = MAGIC_MIKE?r:(hard?Color.red:Color.white);
                    ps.Play();
                }
                if(soundAvailable && hard)
                {
                    AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Length)];
                    source.PlayOneShot(clip,1f);
                    soundAvailable = false;
                    Invoke("soundOKAgain", clip.length*0.25f);
                }
            }
        }
        else
        {
            // cameraNoise.m_AmplitudeGain = 0f;
            // cameraNoise.m_FrequencyGain = 0f;
        }
    }

    void soundOKAgain()
    {
        soundAvailable = true;
    }
}
