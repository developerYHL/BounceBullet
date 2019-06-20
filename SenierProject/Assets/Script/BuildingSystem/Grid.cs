using UnityEngine;
namespace ClientLibrary
{
    public class Grid : MonoBehaviour
    {
        [SerializeField]
        private float size = 1f;

        public Vector3 GetNearestPointOnGrid(Vector3 position) {
            position -= transform.position;

            int xCount = Mathf.RoundToInt(position.x / size);
            int yCount = Mathf.RoundToInt(position.y / size);
            int zCount = Mathf.RoundToInt(position.z / size);

            Vector3 result = new Vector3(
                (float)xCount * size,
                (float)yCount * size,
                (float)zCount * size);

            result += transform.position;

            return result;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            for (float x = -150; x < 50; x += size) {
                for (float z = 30; z > -60; z -= size) {
                    var point = GetNearestPointOnGrid(new Vector3(x, 1f, z));
                    Gizmos.DrawSphere(point, 0.1f);
                }

            }
        }
    }
}