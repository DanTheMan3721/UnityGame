using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject Projectile;
    public Transform launchPos;
    public Transform backLaunchPos;
    public SpriteRenderer Sprite;
    private void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }
    public void Fire()
    {
        if (!Sprite.flipX)
        {
            GameObject proj = Instantiate(Projectile, launchPos.position, Projectile.transform.rotation);
            Vector3 origScale = proj.transform.localScale;


            proj.transform.localScale = new Vector3(
                origScale.x,
                origScale.y,
                origScale.z
                );
        }
            
        else
        {
            GameObject proj = Instantiate(Projectile, backLaunchPos.position, Projectile.transform.rotation);
            Vector3 origScale = proj.transform.localScale;
            proj.transform.localScale = new Vector3(
            origScale.x * -1,
            origScale.y,
            origScale.z
            );
        }
    }
}
