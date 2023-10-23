using System.Collections.Generic;
using Camera;
using Garvity;
using Infrastructure;
using Player;
using Serivces;
using UnityEngine;

public class GameBootstrtaper : MonoBehaviour
{
    private IInputService _inputService;
    private IArtificialGravityService _artificialGravityService;
    private List<IUpdatableService> _updatableService = new ();
    private IGameFactory _gameFactory;
    private SnakeHead _snakeHead;
    private CameraFollow _cameraFollow;


    private void Awake()
    {
        RegisterServices();
        
        CreateGameFactory();

        InitialWorld();
    }

    private void RegisterServices()
    {
        RegisterInputService();
        RegisterArtificialGravityService();
    }

    private void CreateGameFactory() =>
        _gameFactory = new GameFactory(_artificialGravityService);

    private void InitialWorld()
    {
        
        FabricEat fabricEat = new FabricEat(_gameFactory);
        
        Vector3 spawnApplePosition = Vector3.zero;
        float radiusSpawn = 100;
        GameObject apple = _gameFactory.Instantiate(AssetsPath.ApplePath, spawnApplePosition);
        
        _artificialGravityService.SetCenterOfGravity(apple.transform);
        fabricEat.SetCenterOfGravity(apple.transform, radiusSpawn);

        _snakeHead = _gameFactory.CreateSnakeHead(apple.transform, radiusSpawn);
        _snakeHead.Construct(_gameFactory, _inputService);
        
        _cameraFollow = _gameFactory.CreateCamera(_snakeHead.transform);
        _cameraFollow.Construct(_snakeHead.transform, apple.transform);
        
    }

    private void RegisterArtificialGravityService()
    {
        _artificialGravityService = new ArtificialGravityService();
        AddUpdatableService(_artificialGravityService);
    }

    private void RegisterInputService()
    {
        _inputService = new InputService();
        AddUpdatableService(_inputService);
    }
    

    private void AddUpdatableService(IService service)
    {
        if (service is IUpdatableService updatableService)
            _updatableService.Add(updatableService);
    }

    private void FixedUpdate()
    {
        foreach (var updatableService in _updatableService) 
            updatableService.Update();
    }
}