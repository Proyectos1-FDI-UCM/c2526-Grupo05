//---------------------------------------------------------
// Script responsable de controlar el disparo de un enemigo sin munición hacia el jugador
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este script controla el disparo de un enemigo hacia el jugador cuando ChasePlayer le indique que
/// puede atacar (siendo necesario este componente). ChasePlayer ha de estar situado en el último padre de
/// todos los GameObjects implicados en el Prefab del Enemigo.
/// No esta pensado para utilizar munición (tendrá infinita y si el enemigo tiene HasAmmo será ignorado).
/// Es necesario que el enemigo tenga el componente Shoot y que este este situado en el mismo GameObject
/// que este script, en el arma del enemigo.
/// Siempre disparará hacia el jugador, y es necesario que exista LevelManager instance y tenga configurado
/// el PlayerTransform para que funcione.
/// 
/// Se debe colocar en un GameObject con padre (junto a Shoot), y este deberá ser el enemigo para que funcione bien.
/// </summary>
public class EnemyShootingAttack : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Almacena el tiempo entre disparo y disparo del enemigo, siempre que pueda disparar.
    /// </summary>
    [SerializeField]
    private float CooldownDisparos = 3f;

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
    /// Almacena el transform del jugador, leido del LevelManager.
    /// Inicializado en el Start().
    /// </summary>
    private Transform _playerTransform;

    /// <summary>
    /// Almacena el componente Shoot que ha de tener el GameObject con este script.
    /// Inicialziado en el Start();
    /// </summary>
    private Shoot _shoot;

    /// <summary>
    /// Almacena el componente ChasePlayer que ha de tener el mayor padre de los GameObject del enemigo con este script.
    /// Inicializado en el Start();.
    /// </summary>
    private ChasePlayer _chasePlayer;

    /// <summary>
    /// Almacena el último tiempo en el que el enemigo disparó.
    /// </summary>
    private float _tUltimoDisparo = -99;

    /// <summary>
    /// Bool que dice si hay o no GameManager en la escena.
    /// Inicicializado en el Start().
    /// </summary>
    private bool _gameManager = false;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Se llama una vez si el componente esta activo al cargarse en escena, o según se active. Después del Awake().
    /// Hace comprobaciones necesarias para el componente y registra otras.
    /// </summary>
    void Start()
    {
        if (!LevelManager.HasInstance())
        {
            Debug.Log("Se ha colocado el componente \"EnemyShootingAttack\" en una escena sin LevelManager. No funcionará");
            Destroy(this);
        }
        else
        {
            _playerTransform = LevelManager.Instance.PlayerTransform();
            if (_playerTransform == null)
            {
                Debug.Log("Se ha colocado el componente \"EnemyShootingAttack\" en una escena al que no se le ha asignado al LevelManager el Transform del jugador. No funcionará");
                Destroy(this);
            }
        }

        _shoot = GetComponent<Shoot>();
        if (_shoot == null)
        {
            Debug.Log("Se ha colocado el componente \"EnemyShootingAttack\" en un objeto que no tiene el componente \"Shoot\". No podrá disparar");
            Destroy(this);
        }

        _chasePlayer = transform.root.GetComponent<ChasePlayer>(); // se revisa en el objeto más "grande", el "mayor padre"
        if (_chasePlayer == null)
        {
            Debug.Log("Se ha colocado el componente \"EnemyShootingAttack\" en un objeto cuyo mayor padre no tiene el componente \"ChasePlayer\". No podrá disparar");
            Destroy(this);
        }

        _gameManager = GameManager.HasInstance();
    }

    /// <summary>
    /// Se llama cada frame si el componente está activo.
    /// Verifica si el enemigo puede disparar y si es posible realiza el disparo.
    /// </summary>
    void Update()
    {
        float compareTime = CooldownDisparos;
        if (_gameManager) compareTime *= 1 / GameManager.SlowMultiplier;
        
        if (!_chasePlayer.IsChasing() && Time.time - _tUltimoDisparo > CooldownDisparos)
        {
            Dispara();
            _tUltimoDisparo = Time.time;
        }
    }

    /// <summary>
    /// Si el componente se destruye por no poder funcionar, se asegura de que los otros muy relacionados no
    /// puedan dar problemas, destruyendolos también.
    /// </summary>
    private void OnDestroy()
    {
        Destroy(GetComponent<Shoot>());
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
    /// Calcula la dirección en la que se debe disparar (enemigo -> jugador) y llama a 
    /// _shoot para que realize el disparo.
    /// </summary>
    private void Dispara()
    {
        Vector2 fireDir = (Vector2)_playerTransform.position - (Vector2)transform.parent.position; // z = 0 automáticamente
        _shoot.ShootBullet(fireDir);
    }
    #endregion   

} // class EnemyShootingAttack 
// namespace
