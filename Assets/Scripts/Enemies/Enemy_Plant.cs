using UnityEngine;

public class Enemy_Plant : Enemy
{
     // на анимации hit выключена Can Transition to self
     
     // нужно повесить событие на анимацию, в середине вызвать метод аттаки
     private static readonly int Attack1 = Animator.StringToHash("attack");

     [Header("Plant Details")] 
     [SerializeField]  private GameObject bulletPrefab;
     [SerializeField] private float bulletSpeed = 7f;
     [SerializeField]  private Transform gunPoint;
     [SerializeField] private float attackCooldDown = 1.5f;
     [SerializeField] private float lastTimeAttacked;

     protected override void Update()
     {
          base.Update();
          
          bool canAttack = Time.time > lastTimeAttacked + attackCooldDown;

          if (IsPlayerDetection && canAttack)
          {
               Attack();
          }
     }

     private void Attack()
     {
          lastTimeAttacked = Time.time;
          Anim.SetTrigger(Attack1);
     }

     protected override void HandleAnimator()
     {
          // пустой, чтобы не вызывался xVelocity. Его нету на Plant
     }

     private void CreateBullet()
     {
          Enemy_Bullet bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity).GetComponent<Enemy_Bullet>();
          Vector2 bulletVelocity = new Vector2(bulletSpeed * FacingDirection, 0);
          bullet.SetVelocity(bulletVelocity);
          
          Destroy(bullet.gameObject, 10f);
     }
}
