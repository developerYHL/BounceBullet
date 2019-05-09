using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ClientLibrary
{
    public class BulletCtrl : MonoBehaviourPun
    {
        public float damage = 25;   // 공격력
        public int maxReflectionCount = 5;
        public float maxStepDistance = 200;
        public LayerMask blockingLayer;

        int reflectCount = 0;
        // Adjust the speed for the application.
        public float speed;

        // The target (cylinder) position.
        private Transform target;
        //float continuousTime;

        TrailRenderer trailRenderer;

        float time;

        List<Vector3> reflectPositions = new List<Vector3>();

        private void Start() {
            CalculateTarget();
            //GetComponent<TrailRenderer>().time = continuousTime;
        }

        private void Update() {
            if (reflectCount < maxReflectionCount) {
                MoveObject();
            }
            else if (reflectCount == maxReflectionCount) {
                reflectCount++;
            };
        }

        private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining) {
            if (reflectionsRemaining == 0) {
                return;
            }

            Ray ray = new Ray(position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxStepDistance, blockingLayer)) {
                direction = Vector3.Reflect(direction, hit.normal);
                position = hit.point;
            }
            else {
                position += direction * maxStepDistance;
                //이동이 끝나면 사라져야함
            }

            reflectPositions.Add(position);
            DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
        }

        private void OnDrawGizmosSelected() {
            // Draws a 5 unit long red line in front of the object
            Gizmos.color = Color.red;
            Vector3 direction = transform.forward * 25;
            Gizmos.DrawRay(transform.position, direction);
        }

        public void MoveObject() {
            //ReflectionCount를 다 소모했을 때 실행
            if (reflectCount >= maxReflectionCount) {
            }
            else {
                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime; // calculate distance to move

                transform.position = Vector3.MoveTowards(transform.position, reflectPositions[reflectCount], step);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(transform.position, reflectPositions[reflectCount]) < 0.001f) {
                    // Swap the position of the cylinder.
                    reflectCount++;
                    //DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount);
                }
            }
        }

        public void CalculateTarget() {
            //ClearBullet();
            if (reflectPositions.Count == 0)
                DrawPredictedReflectionPattern(transform.position, transform.forward, maxReflectionCount);
            else {
                print("not Null");
            }
        }

        public void ClearBullet() {
            reflectPositions.Clear();
            transform.position = transform.parent.position;
            reflectCount = 0;
        }

        private void OnCollisionEnter(Collision collision)
        {
            IDamageable target = collision.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(damage);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}