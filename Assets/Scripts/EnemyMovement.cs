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
    private NavMeshAgent navMeshAgent; //Component for searching and approaching to the player 

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
    public void SetTarget(Transform newTarget) //To set the target that agent will chase
    {
        target = newTarget;
    }

    private void OnEnable() //When enemy appears
    {
        PlayRandomSound(); //Play one of the three SFX
        soundRoutine = StartCoroutine(RepeatSound()); //will do it again after certain amount of time
    }

    private void OnDisable() //Stops the routine after enemy's disappearence (secures multiple simultaneous uses of SFX) 
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

    void PlayRandomSound() //Play one of the three SFX
    {
        if (sounds.Length == 0) return;

        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.Play();
    }

    IEnumerator RepeatSound() //will do it again after certain amount of time2
    {
        yield return new WaitForSeconds(5f);
        PlayRandomSound();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // if hits the player, enemy disapears
        {
            gameObject.SetActive(false);
        }

        else if (other.CompareTag("Bullet")) // if hits a bullet, enemy disapears
        {
            gameObject.SetActive(false);
        }
    }
}