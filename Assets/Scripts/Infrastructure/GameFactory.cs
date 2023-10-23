using Camera;
using Garvity;
using Player;
using Serivces;
using UnityEngine;

namespace Infrastructure
{
    public class GameFactory : IGameFactory
    {
        private IArtificialGravityService _artificialGravityService;

        public GameFactory(IArtificialGravityService artificialGravityService)
        {
            _artificialGravityService = artificialGravityService;
        }

        public GameObject SpawnObjectInRandomPointOnSphere(string namePrefab, Transform center, float radius)
        {
            var spawnedObject = Instantiate(namePrefab, GetRandomPositionOnSphere(center, radius), center);
            Vector3 spawnPoint;
            Rotation(spawnedObject, center.position);
            TryCheckGround(spawnedObject, out spawnPoint);
            spawnedObject.transform.position = spawnPoint;
            Register(spawnedObject);
            return spawnedObject;
        }

        public CameraFollow CreateCamera(Transform parent) => 
            Instantiate(AssetsPath.CameraPath)
                .GetComponent<CameraFollow>();

        public SnakeHead CreateSnakeHead(Transform spawnApple, float radiusApple) =>
            SpawnObjectInRandomPointOnSphere(
                    AssetsPath.SnakeHeadPath, spawnApple, radiusApple)
                .GetComponent<SnakeHead>();

        private void Rotation(GameObject gameObject, Vector3 center)
        {
            var gameObjectTransform = gameObject.transform;
            var diff = gameObjectTransform.position - center;
            Quaternion orientationDirection =
                Quaternion.FromToRotation(-gameObjectTransform.up, diff) * gameObjectTransform.rotation;
            gameObjectTransform.rotation = orientationDirection;
        }

        public GameObject Instantiate(string namePrefab)
        {
            var gameObject = Object.Instantiate(FindPrefab(namePrefab));
            Register(gameObject);
            return gameObject;
        }

        public GameObject Instantiate(string namePrefab, Vector3 position)
        {
            var gameObject = Object.Instantiate(FindPrefab(namePrefab), position, Quaternion.identity);
            Register(gameObject);
            return gameObject;
        }
        public GameObject Instantiate(string namePrefab, Vector3 position, Transform parent)
        {
            var gameObject = Object.Instantiate(FindPrefab(namePrefab), position, Quaternion.identity, parent);
            Register(gameObject);
            return gameObject;
        }
        public GameObject Instantiate(GameObject prefab, Vector3 position)
        {
            var gameObject = Object.Instantiate(prefab, position, Quaternion.identity);
            Register(gameObject);
            return gameObject;
        }

        private bool TryCheckGround(GameObject spawnedObject, out Vector3 spawnPoint)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(spawnedObject.transform.position,
                    spawnedObject.transform.up,
                    out hit, 
                    10000))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Surface"))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    spawnPoint = new Vector3(hit.point[0], hit.point[1], hit.point[2]);
                    spawnPoint = hit.point - spawnedObject.transform.up * 0.5f;
                    return true;
                }
            }

            spawnPoint = Vector3.zero;
            return false;
        }

        private void Register(GameObject gameObject)
        {
            foreach (var attracted in gameObject.GetComponentsInChildren<IArtificiallyAttracted>())
                    _artificialGravityService.AddAttracted(attracted);
        }

        private Vector3 GetRandomPositionOnSphere(Transform center, float radius) => 
            center.position + new Vector3(Random.value-0.5f,Random.value-0.5f,Random.value-0.5f).normalized*radius;

        private GameObject FindPrefab(string namePrefab) =>
            GetLoadedObject<GameObject>(namePrefab);

        private static T GetLoadedObject<T>(string path) where T : Object => 
            Resources.Load<T>(path);
    }
}