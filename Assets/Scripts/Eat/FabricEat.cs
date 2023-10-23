using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

internal class FabricEat
{
    private readonly IGameFactory _gameFactory;
    private Transform _centerOfGravity;
    private List<Eat> _eats = new ();
    private int _countEat = 100;
    private float _radius;

    public FabricEat(IGameFactory gameFactory)
    {
        _gameFactory = gameFactory;
    }

    public void SetCenterOfGravity(Transform center, float radius)
    {
        _centerOfGravity = center;
        _radius = radius;
        SpawnAllEats();
    }

    private void SpawnAllEats()
    {
        for (int i = 0; i < _countEat; i++)
        {
            var eat = _gameFactory.SpawnObjectInRandomPointOnSphere(
                    AssetsPath.EatPath,
                    _centerOfGravity,
                    _radius)
                .GetComponent<Eat>();
            eat.Collected += OnCollected;
            _eats.Add(eat);
        }
        
    }

    private void OnCollected(Eat eat)
    {
        eat.Collected -= OnCollected;
        SpawnNewEat();
    }

    private void SpawnNewEat()
    {
        var eat = SpawnObject();
        eat.Collected += OnCollected;
    }

    private Eat SpawnObject()
    {
        return _gameFactory.SpawnObjectInRandomPointOnSphere(
                AssetsPath.EatPath,
                _centerOfGravity,
                _radius)
            .GetComponent<Eat>();
    }
}