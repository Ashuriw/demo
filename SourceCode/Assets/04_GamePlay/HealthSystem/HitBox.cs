using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Collider2D))]
public class HitBox : MonoBehaviour
{
    [Header("基础设置")]
    public int damage = 10;
    public float delay = 0f;
    public float duration = 0.5f;
    public LayerMask targetLayer;
    public bool showDebug = true;

    [Header("效果")]
    public GameObject hitEffect;

    private Collider2D damageCollider;
    private float activeTime;
    private bool isActive;
    private Collider2D[] hitBuffer = new Collider2D[10];
    private HashSet<Health> hitTargets = new HashSet<Health>();

    private void Awake() {
        damageCollider = GetComponent<Collider2D>();
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    /// <summary>
    /// 激活攻击判定框
    /// </summary>
    /// <param name="direction">1表示正常方向，-1表示反方向</param>
    public void ActivateHitBox(int direction) {
        damageCollider.offset=new Vector2(damageCollider.offset.x*direction,damageCollider.offset.y);
        hitTargets.Clear();
        activeTime = 0f;
        isActive = false;
        damageCollider.enabled = false;

        if (delay > 0) {
            Invoke(nameof(SetActive), delay);
        } else {
            SetActive();
        }
        // 自动销毁
        Destroy(gameObject, delay + duration + 0.1f);
    }

    private void SetActive() {
        isActive = true;
        damageCollider.enabled = true;
    }

    private void Update() {
        if (!isActive) return;

        activeTime += Time.deltaTime;
        if (activeTime >= duration) {
            Deactivate();
            return;
        }

        CheckHit();
    }

    private void CheckHit() {
        int count = GetOverlapResults();
        
        for (int i = 0; i < count; i++) {
            var health = hitBuffer[i].GetComponent<Health>();
            if (health != null && !hitTargets.Contains(health)&&!health.Invincible) {
                Vector2 hitPoint = hitBuffer[i].ClosestPoint(transform.position);
                health.TakeDamage(damage);
                hitTargets.Add(health);
                
                if (hitEffect != null) {
                    Instantiate(hitEffect, hitPoint, Quaternion.identity);
                }
                
            }
        }
    }

    private int GetOverlapResults() {
        if (damageCollider is BoxCollider2D box) {
            return Physics2D.OverlapBoxNonAlloc(
                (Vector2)transform.position + box.offset, 
                box.size, 
                transform.eulerAngles.z, 
                hitBuffer, 
                targetLayer);
        }
        else if (damageCollider is CircleCollider2D circle) {
            return Physics2D.OverlapCircleNonAlloc(
                (Vector2)transform.position + circle.offset, 
                circle.radius, 
                hitBuffer, 
                targetLayer);
        }
        return 0;
    }

    private void Deactivate() {
        isActive = false;
        damageCollider.enabled = false;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (!showDebug || damageCollider == null) return;

        Gizmos.color = isActive ? new Color(1, 0.2f, 0.2f, 0.5f) : new Color(0.2f, 0.5f, 1, 0.3f);
        
        if (damageCollider is BoxCollider2D box) {
            Gizmos.matrix = Matrix4x4.TRS(
                transform.position + (Vector3)box.offset,
                transform.rotation,
                Vector3.one);
            Gizmos.DrawCube(Vector3.zero, box.size);
        }
        else if (damageCollider is CircleCollider2D circle) {
            Gizmos.DrawSphere(
                transform.position + (Vector3)circle.offset,
                circle.radius);
        }
    }
    #endif
}