// Importation des bibliothèques nécessaires.
using UnityEngine;

// Déclaration de la classe PipeController, héritant de MonoBehaviour.
public class PipeController : MonoBehaviour
{
    // Déclaration de variables privées.
    private Transform[] _pipes; // Tableau pour stocker les transformées des tuyaux.
    private int _lastPipeIndex; // Index du dernier tuyau dans le tableau.
    
    // Variables visibles dans l'éditeur Unity pour personnaliser le jeu.
    [SerializeField] private Transform _pipePrefab; // Préfabriqué des tuyaux.
    [SerializeField] private int _pipeCount = 5; // Nombre de tuyaux à gérer.
    
    [SerializeField] private float _speed = 0.1f; // Vitesse de déplacement des tuyaux.
    [SerializeField] private float _spaceBetweenPipes = 4; // Espace entre les tuyaux.
    [SerializeField] private float _maxHeightDifferenceBetweenPipes = 5; // Différence de hauteur maximale entre les tuyaux consécutifs.
    [SerializeField] private float _minHeight, _maxHeight; // Hauteur minimale et maximale pour les tuyaux.
    
    private float _killEdge; // Position x où les tuyaux doivent être réinitialisés.
    
    // Start est appelée avant la première frame de mise à jour.
    private void Start()
    {
        InitializePipes(); // Initialisation des tuyaux.
    }
    
    // Update est appelée une fois par frame.
    private void Update()
    {
        MovePipes(); // Déplace les tuyaux.
        CheckAndRepositionPipe(); // Vérifie si un tuyau doit être réinitialisé.
    }

    // Méthode pour initialiser les tuyaux.
    private void InitializePipes()
    {
        _pipes = new Transform[_pipeCount];
        for (int i = 0; i < _pipeCount; i++)
        {
            // Crée et positionne chaque tuyau.
            var pipe = Instantiate(_pipePrefab, transform);
            var yPos = GetRandomYPosition(i == 0 ? 0 : _pipes[i - 1].position.y);
                //= i == 0 ? Random.Range(_minHeight, _maxHeight) : Mathf.Clamp(_pipes[i - 1].position.y + Random.Range(-_maxHeightDifferenceBetweenPipes, _maxHeightDifferenceBetweenPipes), _minHeight, _maxHeight);
            pipe.position = new Vector3(i * _spaceBetweenPipes, yPos, 0);
            _pipes[i] = pipe;
        }

        // Calcule le bord de "mort" pour les tuyaux, en dehors du champ de vision de la caméra.
        var cam = Camera.main;
        _killEdge = cam.transform.position.x - cam.orthographicSize * cam.aspect - 3f;
        _lastPipeIndex = 0;
    }

    // Méthode pour déplacer les tuyaux.
    private void MovePipes()
    {
        var movement = Vector3.left * (_speed * Time.deltaTime);
        foreach (var pipe in _pipes)
        {
            pipe.position += movement;
        }
    }

    // Méthode pour vérifier et repositionner un tuyau.
    private void CheckAndRepositionPipe()
    {
        var pipeTransform = _pipes[_lastPipeIndex];
        if (!(_killEdge - pipeTransform.position.x > 0.5f)) return;

        // Calcule la nouvelle position x et y pour le tuyau à repositionner.
        var pos = pipeTransform.position;
        pos.x += _spaceBetweenPipes * _pipeCount;
        
        pos.y = GetRandomYPosition(_lastPipeIndex == 0 ? _pipes[^1].position.y : _pipes[_lastPipeIndex - 1].position.y);
        //yPos = _lastPipeIndex == 0 ? yPos : Mathf.Clamp(yPos, _pipes[_lastPipeIndex - 1].position.y - _maxHeightDifferenceBetweenPipes, _pipes[_lastPipeIndex - 1].position.y + _maxHeightDifferenceBetweenPipes);
        pipeTransform.position = pos;
        _lastPipeIndex = (_lastPipeIndex + 1) % _pipeCount;
    }
    
    private float GetRandomYPosition(float previousYPosition)
    {
        var pos = Random.Range(_minHeight, _maxHeight);
        return Mathf.Clamp(pos, previousYPosition - _maxHeightDifferenceBetweenPipes, previousYPosition + _maxHeightDifferenceBetweenPipes);
    }
}
