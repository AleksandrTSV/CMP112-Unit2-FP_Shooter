using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;
    [SerializeField] AudioClip[] deathSound;

    // Reference to the player's transform.
    public Transform target;
    private NavMeshAgent navMeshAgent;

    private AudioSource source;
    private Coroutine soundRoutine;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void SetTarget(Transform newTarget) 
    {
        target = newTarget;
    }

    private void OnEnable()
    {
        PlayRandomSound();
        soundRoutine = StartCoroutine(RepeatSound());
    }

    private void OnDisable()
    {
        if (soundRoutine != null)
            StopCoroutine(soundRoutine);
    }

    void Update()
    {
        // If there's a reference to the player...
        if (target != null)
        {
            // Set the enemy's destination to the player's current position.
            navMeshAgent.SetDestination(target.position);
        }
    }

    void PlayRandomSound() 
    {
        if (sounds.Length == 0) return;

        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.Play();
    }

    IEnumerator RepeatSound()
    {
        yield return new WaitForSeconds(5f);
        PlayRandomSound();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }

        else if (other.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);
        }
    }
}