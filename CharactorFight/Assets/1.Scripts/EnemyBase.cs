using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    Rigidbody rb;
    SpriteRenderer sr;
    Animator anim;

    [SerializeField] float hp_max = 1;
    [SerializeField] float cur_hp;
    [SerializeField] float damage = 1;

    [SerializeField] bool is_death = false;



    void Death()
    {

    }
    void Start()
    {
        
    }

    void Update()
    {
        if(hp_max <= 0)
            Death();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBase>();
        }
    }
}
