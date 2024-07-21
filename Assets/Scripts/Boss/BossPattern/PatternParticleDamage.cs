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

        // �浹�� ��ƼŬ�� ���� ��������
        List<ParticleSystem.Particle> enteredParticles = new List<ParticleSystem.Particle>();
        int numEntered = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);

        Debug.Log("Number of entered particles: " + numEntered);
        
        for (int i = 0; i < numEntered; i++)
        {

            ParticleSystem.Particle particle = enteredParticles[i];
            Vector3 particlePosition = particle.position;

            // ��ƼŬ�� ��ġ���� �浹�� ��ü�� ����
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

        // Ʈ���ŵ� ��ƼŬ�� �ٽ� ����
        particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);
    }
}
