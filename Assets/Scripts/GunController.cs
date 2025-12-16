using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class GunController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform creator;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource source;

    [Header("Bullet")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] int poolSize = 30;
    [SerializeField] float bulletSpeed = 40f;

    [Header("Fire")]
    [SerializeField] float fireRate = 0.2f;

    /*[Header("Overheat")]
    [SerializeField] float heatPerShot = 1f;
    [SerializeField] float maxHeat = 20f;       //For the future development
    [SerializeField] float coolSpeed = 5f;
    */

    [Header("Audio")]
    [SerializeField] AudioClip fireLoop;
    [SerializeField] AudioClip fireEnd;

    Bullet[] pool; //array of bullets we can use(performance purposes)
    int poolIndex;

    bool isFiring = false;
    private float nextFireTime = 0.2f; //Fire rate


    private void Awake()
    { 
        pool = new Bullet[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(bulletPrefab);  //Create 30 instances of bullets 
            pool[i].gameObject.SetActive(false);  // and we will only ue these ones(instead of using Instantiate and then Destroy)
        }
    }

    void Update()
    {    
        if (isFiring && Time.time > nextFireTime) //Checks if you are holding fire button
        {
            Bullet bullet = pool[poolIndex];
            poolIndex = (poolIndex + 1) % poolSize; //so we dont have OutOfBoundsException

            bullet.transform.position = creator.position;
            bullet.transform.parent = null;

            Vector3 direction = creator.forward;

            bullet.gameObject.SetActive(true);
            bullet.Fire(direction.normalized * bulletSpeed); //gives the direction to the bullet

            nextFireTime = Time.time + fireRate;
        }
    }

    void OnAttack(InputValue value) //Check if fire button is pressed 
    {
        if (value.Get<float>() > 0.5f)
        {
            StartFire();
        }

        else 
        {
            StopFire();
        }
    }
    
    int onOff = 0; //to ensure that our SFX will activate ones
    void StartFire() 
    {
        anim.SetBool("Shoot", true); //Activate shooting animation
        //---------------------
        if (onOff == 0)
        {
            source.clip = fireLoop;
            source.loop = true;
            source.Play();
            onOff = 1;
        }

        isFiring = true;
    }
    void StopFire()
    {
        if (onOff == 1)
        {
            anim.SetBool("Shoot", false);
            source.Stop();
            source.PlayOneShot(fireEnd);
            onOff = 0;
        }

        isFiring = false;
    }
}

    
    


