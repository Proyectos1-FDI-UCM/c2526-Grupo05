//---------------------------------------------------------
// Este componente permite a un enemigo a atacar cuando se ha acercado al jugador lo suficiente
// CamiloSandovalSánchez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Esta clase permite a un enemigo a atacar cuando se ha acercado al jugador lo suficiente
/// utilizando el componente ChasePlayer. 
/// También se define un tiempo de cooldown editable que no
/// permitirá realizar ataques hasta que haya pasado. 
/// Por último, llama al componente canMelee para spawnear 
/// al objeto melee correspondiente.
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

    /// <summary>
    /// Tiempo de cooldown
    /// </summary>
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

    /// <summary>
    /// Variable para usar el componente CanMelee
    /// </summary>
    private CanMelee _canMelee;
    /// <summary>
    /// Variable para usar el componente ChasePlayer
    /// </summary>
    private ChasePlayer _chasePlayer;
    /// <summary>
    /// Variable para guardar el momento de cada ataque
    /// </summary>
    private float _tiempoDesdeUltimoMelee = -99f;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Método Start de programación defensiva, detecta los componentes CanMelee, ChasePlayer, Rigidbody2D y se comprueba que existe la instancia del jugador con PlayerCore.
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

        if (!PlayerCore.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"ChasePlayer\" en una escena sin instancia de PlayerCore. No podrá perseguir al jugador");
            Destroy(this);
        }
    }

    /// <summary>
    /// En el Update se comprueba, a través del ChasePlayer, que el enemigo está a distancia de ataque melee.
    /// Cuando se cumple la condición y ha transcurrido el cooldown, se llama al método del ataque melee y se establece el tiempo de este último ataque melee.
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

    /// <summary>
    /// Este métoddo crea una variable de direccion del ataque y una de la posición del mismo.
    /// Posteriormente llama al método que crea el objeto melee en el componente de CanMelee.
    /// </summary>
    private void CanMelee()
    {
        Vector2 dirMvtoEnemigo = (PlayerCore.Instance.ReadPlayerPosition() - transform.position).normalized;
        Vector2 posHitbox = (Vector2)transform.position + dirMvtoEnemigo;

        _canMelee.HitboxMelee(dirMvtoEnemigo, posHitbox);
    }
    #endregion   

} // class EnemyMeleeAttack 
// namespace
