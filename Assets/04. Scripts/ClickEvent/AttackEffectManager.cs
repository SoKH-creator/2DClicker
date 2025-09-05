using UnityEngine;

public class AttackEffectManager : MonoBehaviour
{
    // 유니티 인스펙터에서 드래그하여 연결할 파티클 프리팹
    public GameObject attackParticlePrefab;

   
    /// <param name="position">파티클이 생성될 위치</param>
    public void SpawnAttackParticle(Vector3 position)
    {
        // 파티클 프리팹이 할당되었는지 확인
        if (attackParticlePrefab == null)
        {
            Debug.LogWarning("AttackEffectManager: attackParticlePrefab이 할당되지 않았습니다.");
            return;
        }

        // 파티클 오브젝트를 생성
        GameObject particleInstance = Instantiate(attackParticlePrefab, position, Quaternion.identity);

        // 파티클 시스템 컴포넌트를 가져와 재생
        ParticleSystem particleSystem = particleInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();

            // 파티클 재생이 끝나면 오브젝트를 파괴하여 메모리 정리
            Destroy(particleInstance, particleSystem.main.duration);
        }
        else
        {
            Debug.LogWarning("AttackEffectManager: attackParticlePrefab에 ParticleSystem 컴포넌트가 없습니다.");
        }
    }
}