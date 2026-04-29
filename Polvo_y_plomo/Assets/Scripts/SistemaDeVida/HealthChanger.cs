//---------------------------------------------------------
// Gestor de vida
// Miguel Gómez García
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;

public class HealthChanger : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Esta será la variable de la vida con la que iniciarán los gameObject. Debe ser configurable para ajustarse a cada caso especificó y no variará una vez establecida
    /// </summary>
    [SerializeField]
    private int VidaMax = 10; //Máxima vida del GameObject

    [Header("SFX")]

    [SerializeField]
    private AudioClip DanyoJugador;

    [SerializeField]
    private AudioClip CuracionJugador;

    [SerializeField]
    private AudioClip CoberturaCubre;

    [SerializeField]
    private AudioClip CoberturaRota;
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
    /// Esta será la variable de la vida que tendrán los game objects (irá variando)
    /// </summary>
    private int _vida;

    /// <summary>
    /// Un booleano que determinará si somos el jugador
    /// </summary>
    private bool _jugador = false;

    /// <summary>
    /// Almacena el componente _canFlash si existe.
    /// Se asume que iniciará el parpadeo si se recibe daño.
    /// Inicializado en el Start().
    /// </summary>
    private CanFlash _canFlash;

    /// <summary>
    /// Un booleano que determinará si podemos recibir daño o no 
    /// </summary
    private bool _canRecieveDamage = true;

    private bool _cobertura = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al cargarse en la escena de inmediato.
    /// Establece la vida.
    /// </summary>
    private void Awake()
    {
        _vida = VidaMax;
    }

    /// <summary>
    /// Se llama al cargarse en la escena si esta activo, o al activarse por primera vez.
    /// Al iniciar el juego, la vida del gameObject tomará el valor de la vida con la que empieza.
    /// Si existe un GameManager y eres el jugador, establece la variable como true
    /// </summary>
    private void Start()
    {
        if (GetComponent<playerControlledMovement>() != null)
        {
            _jugador = true;
            if (GameManager.HasInstance()) _vida = GameManager.Instance.InitHealthChanger();
        }
        else if (gameObject.CompareTag("Barrel")) _cobertura = true;
        _canFlash = GetComponent<CanFlash>();
    }

    /// <summary>
    /// Si no es jugador informa al Level Manager de la muerte para llegar su cuenta.
    /// Implementado en el OnDestroy() en vez de en el método de muerte para que se puedan considerar las "muertes"
    /// de los EnemySpawnLogic (su desaparición natural tras la aparición).
    /// </summary>
    private void OnDestroy()
    {
        if (!_jugador)
        {
            IsEnemy isenemy = GetComponent<IsEnemy>();
            if (isenemy != null) isenemy.EnemyDied();
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
    /// <summary>
    /// Por lo general todos los ataques harán uno de daño, pero si te curas, no te puedes curar más del maximo de lo que se te permite
    /// Este metodo permitirá curarse (teniendo como tope la vida con la que empiezas) y hacer daño hasta quedarte sin vida
    /// Si te quedas sin vida llamara al metodo para matar
    /// Si eres el jugador, actualiza tu vida en el HUD
    /// </summary>


    /// <summary>
    /// Metodo que permitirá que no nos hagan daño mientras estamos escondidos
    /// </summary
    public void BlockDamage()
    {
        _canRecieveDamage = false;
    }

    /// <summary>
    /// Metodo que permitirá que nos hagan daño mientras no estamos escondidos
    /// </summary
    public void AllowDamage()
    {
        _canRecieveDamage = true;
    }
    public void CambiarVida(int cambio = -1)      
    {
        if (!_canRecieveDamage && cambio < 0) return;

        _vida += cambio;
        if (_vida > VidaMax) // si se da curación y se excede el máximo de vida
            _vida = VidaMax;

        // Actualizar HUD del jugador o sonido de bloqueo de cobertura
        if (_jugador && GameManager.HasInstance())
        {
            GameManager.Instance.UpdateHealthHUD(_vida);
            if (DanyoJugador && cambio < 0) AudioManager.Instance.Play(DanyoJugador, transform.position);
            else if (CuracionJugador && cambio > 0) AudioManager.Instance.Play(CuracionJugador, transform.position);
        }
        else if (CoberturaCubre && _vida > 0) AudioManager.Instance.Play(CoberturaCubre, transform.position);

        // Realizar flash de daño si existe el componente
        if (cambio < 0)
        {
            // Para que los enemigos puedan flashear durante la animacion de spawn
            if (_canFlash != null) _canFlash.StartFlashes();
            else
            {
                EnemySpawnLogic enemySpawn = GetComponent<EnemySpawnLogic>();
                if (enemySpawn != null)
                {
                    _canFlash = GetComponentInChildren<CanFlash>();
                    if (_canFlash != null) _canFlash.StartFlashes();
                }
            }
        }

        // Muerte del objeto
        if (_vida <= 0)
        {
            MetodoMuerte();
        }
    }

    /// <summary>
    /// Con este metodo podremos saber si la vida del jugador es igual o mayor a la vida máxima
    /// Con eso podremos determinar si puede ser curado por objetos o no
    /// </summary>
    public bool CuracionPermitida()
    {
        if (_vida < VidaMax) return true;
        else return false;
    }

    /// <summary>
    /// Método público que devuelve la vida actual del GameObject. Utilizado principalmente para transicionar fases en enemigos grandes (Suzie).
    /// </summary>
    /// <returns></returns>
    public int GetCurrentHealth()
    {
        return _vida;
    }

    /// <summary>
    /// Método público que devuelve la vida maxima del GameObject. Utilizado principalmente para transicionar fases en enemigos grandes (Suzie).
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth()
    {
        return VidaMax;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Este metodo comprueba si el gameObject que le invoca es el jugador.
    /// Dependiendo de si lo llama el gameObject del jugador destruirá reiniciará la escena
    /// De otra manera comprueba si el gameObject tiene cadaver, si lo tiene lo genera el cadaver, de lo contrario destruye el objeto 
    /// Además, avisa al LevelManager cuando lo que muere es un enemigo
    /// (Incompleto/Futuro)
    /// </summary>
    private void MetodoMuerte()
    {
        if (_jugador)
        {
            if (GameManager.HasInstance()) GameManager.Instance.Respawn();
            Destroy(this);
        }
        else // si no es jugador
        {
            // IsEnemy se lleva en el OnDestroy para poder implementar el EnemySpawnLogic

            PointsOnDeath points = GetComponent<PointsOnDeath>();
            if (points != null) points.GivePoints();

            // spawn del cadaver
            if (GetComponent<GeneraCadaver>() != null)
            {
                GeneraCadaver genCad = GetComponent<GeneraCadaver>();
                genCad.PonCadaver();
                if (CoberturaRota) AudioManager.Instance.Play(CoberturaRota, transform.position);
            }
            else Debug.Log("Este Objeto no tiene un componente GeneraCadaver");
            Destroy(gameObject);
            //Hay que hacer más adelante las animaciónes de muerte de los enemigos

        }
    }

    #endregion
}
