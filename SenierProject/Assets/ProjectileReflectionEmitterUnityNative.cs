using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
 * Projectile reflection demonstration in Unity 3D
 * 
 * Demonstrates a projectile reflecting in 3D space a variable number of times.
 * Reflections are calculated using Raycast's and Vector3.Reflect
 * 
 * Developed on World of Zero: https://youtu.be/GttdLYKEJAM
 */
public class ProjectileReflectionEmitterUnityNative : MonoBehaviour
{
    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;
    public LayerMask blockingLayer;

    int reflectCount = 0;
    // Adjust the speed for the application.
    public float speed = 10.0f;

    // The target (cylinder) position.
    private Transform target;
    float continuousTime;

    TrailRenderer renderer;

    float time;

    List<Vector3> reflectPositions = new List<Vector3>();

    BulletCtrl bulletCtrl;

    private void OnDrawGizmosSelected() {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 25;
        Gizmos.DrawRay(transform.position, direction);
    }

    private void Start() {
        continuousTime = transform.parent.GetComponent<BulletCtrl>().continuousTime;
        GetComponent<TrailRenderer>().time = continuousTime;
        bulletCtrl = transform.parent.GetComponent<BulletCtrl>();


        //Handles.color = Color.red;
        //Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(this.transform.position, 0.25f);
    }

    private void Update() {
        if (reflectCount < maxReflectionCount) {
            MoveObject();
        }
        else if (reflectCount == maxReflectionCount) {
            bulletCtrl.ActiveCount();
            reflectCount++;
        };
    }
    private void OnDestroy() {
    }
    private void OnDisable() {
        
        
    }
    private void OnEnable() {
        //DrawPredictedReflectionPattern(this.transform.position, this.transform.forward, maxReflectionCount);
    }


    private void On() {
        //gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(50, 0, 0));
        //Handles.color = Color.red;
        //Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(this.transform.position, 0.25f);

    }

    void OnDrawGizmos()
    {
        //Handles.color = Color.red;
        //Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(this.transform.position, 0.25f);

        //DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount);
    }

  

    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0) {
            return;
        }

        //Vector3 startingPosition = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxStepDistance,blockingLayer))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
            
        }
        else
        {
            position += direction * maxStepDistance;
            //이동이 끝나면 사라져야함
        }

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(startingPosition, position);
        print("asd : " + direction);
        reflectPositions.Add(position);
        //print("maxReflectionCount : " + maxReflectionCount);
        //print("reflectionsRemaining : " + reflectionsRemaining);
        //print("reflectPositions.Add(position) : " + reflectPositions[maxReflectionCount-reflectionsRemaining]);
        DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }

    public void MoveObject() {
        //ReflectionCount를 다 소모했을 때 실행
        if (reflectCount >= maxReflectionCount) {
            //transform.parent.GetComponent<BulletCtrl>().ActiveCount();
            
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
        ClearBullet();
        if(reflectPositions.Count == 0)
            DrawPredictedReflectionPattern(this.transform.position, this.transform.forward, maxReflectionCount);
        else {
            print("not Null");
        }
        //print(reflectPositions[reflectPositions.Count-1]);
    }

    public void ClearBullet() {
        reflectPositions.Clear();
        transform.position = transform.parent.position;//new Vector3(0, 0, 0);
        print("fff : " + reflectPositions);
        reflectCount = 0;
    }
}