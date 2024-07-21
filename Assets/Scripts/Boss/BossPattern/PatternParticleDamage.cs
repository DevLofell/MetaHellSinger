using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternParticleDamage : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public int damage = 10;

    private void Awake()
    {
        particleSystem = particleSystem ?? this.gameObject.GetComponent<ParticleSystem>();
    }
    private void Start()
    {
        

        if (particleSystem != null)
        {
            ParticleSystem.TriggerModule trigger = particleSystem.trigger;
            trigger.enabled = true;
            trigger.SetCollider(0, Player.instance.GetComponent<Collider>());
        }
        else
        {
            Debug.LogError("Particle system is not assigned or found.");
        }
    }

    private void OnParticleTrigger()
    {
        Debug.Log("OnParticleTrigger called");

        // 충돌한 파티클의 정보 가져오기
        List<ParticleSystem.Particle> enteredParticles = new List<ParticleSystem.Particle>();
        int numEntered = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);

        Debug.Log("Number of entered particles: " + numEntered);
        
        for (int i = 0; i < numEntered; i++)
        {

            ParticleSystem.Particle particle = enteredParticles[i];
            Vector3 particlePosition = particle.position;

            // 파티클의 위치에서 충돌한 객체를 감지
            Collider[] hitColliders = Physics.OverlapSphere(particlePosition, 3f);
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log("Hit collider: " + hitCollider.name);
                if (hitCollider.CompareTag("Player"))
                {
                    Player player = hitCollider.GetComponent<Player>();
                    if (player != null)
                    {
                        player.UpdateHP(-damage);
                    }
                }
            }
        }

        // 트리거된 파티클을 다시 설정
        particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);
    }
}
