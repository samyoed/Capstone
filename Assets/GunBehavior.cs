using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour {

    [SerializeField] public int bullet_num = 1;
    [SerializeField] public int bullet_spread = 15;
    [SerializeField] public float bullet_force = 2500f;
    [SerializeField] public float bullet_screen_shake_time = 0.8f;
    [SerializeField] public float bullet_screen_shake_force = 0.15f;
    [SerializeField] public float bullet_damage = 0.35f;
    [SerializeField] public Sprite bullet_sprite;
    [SerializeField] public Sprite bulletUI_sprite;

    [SerializeField] protected string weapon_name = "Weapon_Name";
    [SerializeField] protected bool meele = true;
    [SerializeField] protected float meele_damage = 0.25f;

    [SerializeField] protected float spd = 5f;
    [SerializeField] protected float friction = 0.15f;
    [SerializeField] protected float recoil = 0.5f;
    [SerializeField] protected int cooldown = 20;
    [SerializeField] protected int ammo = -1;
    
    protected float use_angle;
    protected int timer;

    Vector2 activePosition;
    Vector2 velocity;

    bool active = false;
    bool thrown = false;
    bool player = false;
    
    

    protected void init() {
        active = false;
        thrown = false;
        player = false;

        activePosition = transform.position;
        timer = 0;
        use_angle = 0;
        transform.position = new Vector3(transform.position.x, transform.position.y, 5);
        meele = false;
    }

    public void useWeapon() 
    {
        use();
        Debug.Log("shot");
    }

    protected void use(){
        //Check Cooldown & Ammo
        if (timer > 0){
            return;
        }
        
        if (ammo == 0){
            return;
        }
        if (ammo > 0){
            ammo--;
        }
        timer = cooldown;

        //Use Weapon
        velocity = new Vector2(Mathf.Cos(0) * recoil, Mathf.Sin(0) * recoil);

        for (int i = 0; i < bullet_num; i++){
            GameObject bullet = new GameObject(weapon_name + "_bullet");
            bullet.gameObject.layer = 11;
            bullet.transform.position = new Vector3(transform.position.x + (Mathf.Cos(use_angle) * 0.05f), transform.position.y + (Mathf.Sin(use_angle) * 0.05f), transform.position.z);
            bullet.AddComponent<SpriteRenderer>();
            bullet.GetComponent<SpriteRenderer>().sprite = bullet_sprite;
            bullet.AddComponent<Rigidbody2D>();
            bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
            bullet.AddComponent<BoxCollider2D>();
            //Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), ignore_collider);

            float bullet_angle = ((use_angle * Mathf.Rad2Deg) + Random.Range(-bullet_spread, bullet_spread)) * Mathf.Deg2Rad;
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(bullet_angle) * bullet_force, Mathf.Sin(bullet_angle) * bullet_force));
            bullet.transform.eulerAngles = new Vector3(0f, 0f, bullet_angle * Mathf.Rad2Deg);

            bullet.AddComponent<TrailEffectScript>();
            bullet.AddComponent<BulletScript>();
            bullet.GetComponent<BulletScript>().damage = bullet_damage;
        }
    }

}
