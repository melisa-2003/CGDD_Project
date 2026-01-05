using UnityEngine;

public class FinalBossTrigger : MonoBehaviour
{
    public FinalBossController bossController;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossController.StartFinalBossSequence();
            Destroy(this); // optional: disable trigger after first use
        }
    }
}
