using UnityEditorInternal;
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

    [Header("Overheat")]
    [SerializeField] float heatPerShot = 1f;
    [SerializeField] float maxHeat = 20f;
    [SerializeField] float coolSpeed = 5f;

    [Header("Audio")]
    [SerializeField] AudioClip fireLoop;
    [SerializeField] AudioClip fireEnd;

    Bullet[] pool;
    int poolIndex;

    InputSystem_Actions input;
    bool isFiring = false;

    float heat;

    private void Awake()
    {
        input = new InputSystem_Actions();

        pool = new Bullet[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(bulletPrefab);
            pool[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        input.Player.Enable();

        input.Player.Attack.performed += ctx => StartFire();
        input.Player.Attack.canceled += ctx => StopFire();
    }

    private void OnDisable()
    {
        input.Player.Attack.performed -= ctx => StartFire();
        input.Player.Attack.canceled -= ctx => StopFire();

        input.Player.Disable();
    }

    void Update()
    {
        heat = Mathf.MoveTowards(heat, 0, coolSpeed * Time.deltaTime);
        //Debug.Log($"Heat: {heat}");
    }

    int onOff = 0;
    void StartFire() 
    {
        anim.SetBool("Shoot", true);
        //---------------------
        if (onOff == 0)
        {
            source.clip = fireLoop;
            source.loop = true;
            source.Play();
            onOff = 1;
         }


        //---------------------
        heat += heatPerShot;
        //---------------------

        Bullet bullet = pool[poolIndex];
        poolIndex = (poolIndex + 1) % poolSize;

        bullet.transform.position = creator.position;
        bullet.transform.parent = null;

        Vector3 direction = creator.forward;

        bullet.gameObject.SetActive(true);
        bullet.Fire(direction.normalized * bulletSpeed);
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
    }
}

    
    


