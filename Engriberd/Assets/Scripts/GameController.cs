﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public List<Bird> Birds;
    public List<Enemy> Enemies;

    private bool _isGameEnded = false;

    private Bird _shotBird;
    public BoxCollider2D TapCollider;

    void Start()
    {
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;
        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];
    }

    public void ChangeBird()
    {
        TapCollider.enabled = false;

        if (_isGameEnded)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        Birds.RemoveAt(0);

        if (Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if (Enemies.Count == 0)
        {
            _isGameEnded = true;
        }
    }

    public void AssignTrail(Bird bird)
    {
        if (TrailController.Instance)
        {
            TrailController.Instance?.SetBird(bird);
            StartCoroutine(TrailController.Instance?.SpawnTrail());
            TapCollider.enabled = true;
        }
    }

    void OnMouseUp()
    {
        if (_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }
}