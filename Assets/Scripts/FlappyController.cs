// Importation des bibliothèques nécessaires pour le projet.

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// MonoBehaviour est la classe de base à partir de laquelle tous les scripts Unity dérivent.
public class FlappyController : MonoBehaviour
{
    // Ces variables sont utilisées pour ajuster la force de saut et la rotation de l'oiseau.
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _maxAngularRotation;
    [SerializeField] private float _minAngularRotation;
    
    // Rigidbody2D est utilisé pour appliquer la physique sur l'objet.
    private Rigidbody2D _rigidbody;
    
    // Animator est utilisé pour contrôler les animations de l'oiseau.
    private Animator _animator;
    private static readonly int Flap = Animator.StringToHash("Flap");
    
    // Variable pour suivre si le joueur est mort ou non.
    private bool _isDead;
    
    
    // Ici, elle est utilisée pour initialiser le composant Rigidbody2D.
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    
    // Elle contient la logique principale du contrôle du joueur.
    private void Update()
    {
        // Si le joueur est mort, ne fait rien (retourne immédiatement).
        if (_isDead) return;
        
        // Vérifie si la touche Espace est pressée pour faire sauter l'oiseau.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Applique une force verticale sur l'oiseau pour le faire sauter.
            _rigidbody.velocity = Vector2.up * _jumpForce;
            
            // Joue l'animation de saut.
            _animator.SetTrigger(Flap);
        }
        
        // Calcule l'angle de rotation de l'oiseau en fonction de sa vitesse verticale.
        var angle = Mathf.Lerp(_minAngularRotation, _maxAngularRotation, Mathf.InverseLerp(-5f, 7f, _rigidbody.velocity.y));
        // Applique la rotation à l'oiseau.
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
    }

    // Cette méthode est appelée lorsqu'un autre objet entre en collision avec l'oiseau.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
            // Marque l'oiseau comme mort.
            _isDead = true;

            StartCoroutine(WaitForDeath());
        } else if (other.CompareTag("ScoreZone"))
        {
            // Appelle la méthode AddScore de la classe ScoreCounterUI.
            ScoreCounterUI.OnScore?.Invoke();
        }
    }
    
    private IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(2f);
        Reset();
    }
    
    // Cette méthode recharge la scène actuelle, redémarrant le jeu.
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
