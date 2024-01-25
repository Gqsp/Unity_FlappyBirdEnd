using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounterUI : MonoBehaviour
{
    [SerializeField] private Sprite[] _digits;
    
    private int _score;
    
    [SerializeField] private Image[] _images;
    
    public static Action OnScore;
    
    private void Start()
    {
        foreach (var image in _images)
        {
            image.gameObject.SetActive(false);
        }
        
        OnScore += AddScore;
        UpdateCounter();
    }

    private void OnDestroy()
    {
        OnScore -= AddScore;
    }

    private void AddScore()
    {
        _score++;
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        for (int i = 0; i < _score.ToString().Length; i++)
        {
            _images[i].gameObject.SetActive(true);
            _images[i].sprite = _digits[int.Parse(_score.ToString()[i].ToString())];
        }
    }
    
}
