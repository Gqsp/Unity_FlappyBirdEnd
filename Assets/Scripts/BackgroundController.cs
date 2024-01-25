using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private int _offset = 9;
    [SerializeField] private SpriteRenderer[] _backgrounds;
    
    private int _lastBackgroundIndex;
    private float _cameraLeftEdge;
    
    private void Start()
    {
        var cam = Camera.main;
        _cameraLeftEdge = cam.transform.position.x - cam.orthographicSize * cam.aspect;
        _lastBackgroundIndex = 0;
    }
    
    private void Update()
    {
        var movement = Vector3.left * (_speed * Time.deltaTime);

        foreach (var background in _backgrounds)
        {
            background.transform.position += movement;
        }

        var bg = _backgrounds[_lastBackgroundIndex];
        if (!(_cameraLeftEdge - bg.bounds.max.x > 0.5f)) return;
        
        var newXPosition = bg.transform.position.x + _offset * _backgrounds.Length;
        bg.transform.position = new Vector3(newXPosition, 0, 0);
        _lastBackgroundIndex = (_lastBackgroundIndex + 1) % _backgrounds.Length;
    }
}