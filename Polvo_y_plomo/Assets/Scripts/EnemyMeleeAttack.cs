//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class EnemyMeleeAttack : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField]
    private float CooldownMelee = 2.5f;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private CanMelee _canMelee;

    private ChasePlayer _chasePlayer;

    private Rigidbody2D _rb;

    private float _tiempoDesdeUltimoMelee = -99f;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _canMelee = GetComponent<CanMelee>();
        if (_canMelee == null)
        {
            Debug.Log("Se ha puesto el componente  \"EnemyMeleeAttack\" en un objeto sin el componente \"CanMelee\", y no podrá atacar.");
            Destroy(this);
        }

        _chasePlayer = GetComponent<ChasePlayer>();
        if (_chasePlayer == null)
        {
            Debug.Log("Se ha puesto el componente  \"EnemyMeleeAttack\" en un objeto sin el componente \"ChasePlayer\", y no podrá atacar.");
            Destroy(this);
        }

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.Log("Se ha puesto el componente  \"EnemyMeleeAttack\" en un objeto sin el componente \"Rigidbody2D\", y no podrá atacar.");
            Destroy(this);
        }

        if (!PlayerCore.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"ChasePlayer\" en una escena sin instancia de PlayerCore. No podrá perseguir al jugador");
            Destroy(this);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (!_chasePlayer.IsChasing() && Time.time - _tiempoDesdeUltimoMelee > CooldownMelee)
        {
            CanMelee();

            _tiempoDesdeUltimoMelee = Time.time;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void CanMelee()
    {
        Vector2 dirMvtoEnemigo = (PlayerCore.Instance.ReadPlayerPosition() - transform.position).normalized;
        Vector2 posHitbox = (Vector2)transform.position + dirMvtoEnemigo;
        

        _canMelee.HitboxMelee(dirMvtoEnemigo, posHitbox);
    }
    #endregion   

} // class EnemyMeleeAttack 
// namespace
