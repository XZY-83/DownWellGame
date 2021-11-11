using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
    [Header("Shoot")]
    public GameObject projectile;
    public int projectileDamage = 4;
    public int maxProjectile = 8;
    public int currentProjectile;
    bool reloaded = true;
    public float shotDelay = 1f;
    public float shotReboundSpeed = 1f;
    float shotTimer = 0f;

    [Header("Stepping")]
    public GameObject hitBox;
    public LayerMask enemyLayerMask;
    public float stepUpSpeed = 7f;
    public float unshootableTime = 1f;
    public bool shootable = true;
    ContactFilter2D enemyFilter;
    Collider2D[] enemyColliders;

    [Header("Damaged")]
    public float leapSpeed;
    public Vector2 knuckBackSpeed;
    public float knuckBackDistance;
    public float invincibleTime;
    bool isInvincible = false;
    public bool IsInvincible { get; }

    public UnityEvent OnDamaged;

    // Start is called before the first frame update
    void Start()
    {
        shotTimer = shotDelay;

        enemyFilter = new ContactFilter2D();
        enemyFilter.SetLayerMask(enemyLayerMask);

        enemyColliders = new Collider2D[3];

        currentProjectile = maxProjectile;
    }

    // Update is called once per frame
    void Update()
    {
        shotTimer += Time.deltaTime;

        if (!reloaded && GetComponent<PlayerController>().GroundCollision())
        {
            Reload();
            reloaded = true;
        }

        StepOn();
    }

    #region Shoot Function
    public void Shoot()
    {
        if (shotTimer >= shotDelay && shootable && currentProjectile > 0)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            newProjectile.GetComponent<Projectile>().damage = projectileDamage;

            currentProjectile--;

            if (currentProjectile < maxProjectile) reloaded = false;

            //GetComponent<PlayerController>().ShotRebound();
            GetComponent<PlayerController>().LeapOff(shotReboundSpeed);
            Camera.main.GetComponent<CameraShake>().Shake(.03f);

            GetComponent<PlayerAnimation>().Shoot();

            if(SoundManager.instance != null) SoundManager.instance.PlayEffSound("gun");  //��������Ʈ
            GetComponent<Effector>().Generate("Shoot");

            bulletCount.instance.countBullet();

            shotTimer = 0;
        }
    }

    public void Reload()
    {
        currentProjectile = maxProjectile;
        bulletCount.instance.bulletReload();
    }
    #endregion

    public void Damaged(Enemy enemy)
    {
        if (isInvincible) return;

        //Debug.Log("Player Damaged");

        //if (!isInvincible) OnDamaged.Invoke();

        Vector3 knuckbackDir = transform.position - enemy.transform.position;
        int direction = knuckbackDir.x > 0 ? 1 : -1;
        GetComponent<PlayerController>().KnuckBack(knuckBackSpeed, direction, knuckBackDistance);

        //StartCoroutine(BecomeInvincible());
        isInvincible = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .3f);
        Invoke("BecomeVincible", invincibleTime);

        Camera.main.GetComponent<CameraShake>().Shake(.08f);
    }

    void BecomeVincible()
    {
        isInvincible = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .3f);

        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
    }

    void StepOn()
    {
        var hitNum = hitBox.GetComponent<CircleCollider2D>().OverlapCollider(enemyFilter, enemyColliders);

        bool playerBound = false;

        foreach (var enemyCollider in enemyColliders)
        {
            //Debug.Log(enemyCollider);

            if (enemyCollider != null)
            {
                playerBound = true;
                enemyCollider.GetComponent<Enemy>().Die();

                if (!reloaded) Reload();
                StartCoroutine(SteppingUp());
            }
        }

        if (playerBound) GetComponent<PlayerController>().LeapOff(leapSpeed);
    }

    IEnumerator SteppingUp()
    {
        shootable = false;
        yield return new WaitForSeconds(unshootableTime);
        shootable = true;
    }
}
