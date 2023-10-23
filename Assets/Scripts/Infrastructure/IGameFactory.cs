using Camera;
using Player;
using UnityEngine;

namespace Infrastructure
{
    public interface IGameFactory
    {
        GameObject Instantiate(string namePrefab);
        GameObject Instantiate(string namePrefab, Vector3 position);
        GameObject Instantiate(string namePrefab, Vector3 position, Transform parent);

        public GameObject SpawnObjectInRandomPointOnSphere(string namePrefab, Transform center, float radius);
        public SnakeHead CreateSnakeHead(Transform spawnApple, float radiusApple);
        public CameraFollow CreateCamera(Transform parent);
    }
}