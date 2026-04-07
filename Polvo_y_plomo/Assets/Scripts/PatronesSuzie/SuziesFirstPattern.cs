//---------------------------------------------------------
// Archivo que controla la lógica y el funcionamiento del primer patrón de ataque de Suzie Ramírez
// Creado por Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using Mono.Cecil.Cil;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que determina cuándo asomará Suzie a disparar, en que dirección (en función de la del jugador en el momento de asomar), durante cuánto tiempo y en qué
/// momentos de ese intervalo disparará (ya que dispara 2 veces cada vez que sale); si debe esconderse antes (por recibir demasiado daño), o si ni siquiera debe
/// asomar porque le toca lanzar dinamita (es un ciclo de ataques: escopeta-escopeta-dinamita-escopeta-escopeta-dinamita...).
/// </summary>
public class SuziesFirstPattern : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Prefab de dinamita
    /// </summary>
    [SerializeField] GameObject DynamitePrefab = null;
    /// <summary>
    /// Número de acciones por ciclo. Cada ciclo acaba con un lanzamiento de dinamita, y el resto de acciones son "peeks" para disparar con la escopeta (2 veces
    /// por peek).
    /// </summary>
    [SerializeField] private int ActionsPerCycle = 3;
    /// <summary>
    /// Cantidad de vida que debe perder Suzie mientras está asomada, para entender que tiene que esconderse antes de tiempo
    /// </summary>
    [SerializeField] private int HealthLossToHide = 2;
    /// <summary>
    /// Distancia que se desplaza Suzie para asomar (en función del tamaño de la carreta, teniendo en cuenta que ella aparece cubierta en mitad de esta)
    /// </summary>
    [SerializeField] private float DistanceToPeekingPos = 4f;
    /// <summary>
    /// Tiempo máximo que puede tardar Suzie desde que asoma, en realizar sus 2 disparos de escopeta
    /// </summary>
    [SerializeField] private float MaxShootingTime = 2.8f;
    /// <summary>
    /// Tiempo mínimo (de cortesía para el jugador) entre disparos de escopeta
    /// </summary>
    [SerializeField] private float LapseBetweenShots = 0.2f;
    /// <summary>
    /// Tiempo máximo que puede permanecer Suzie escondida tras la carreta
    /// </summary>
    [SerializeField] private float MaxHidingTime = 3f;
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
    /// HealthChanger que determina si Suzie tiene vida
    /// </summary>
    private HealthChanger _health = null;
    /// <summary>
    /// ShootEscopeta que determina si Suzie tiene escopeta
    /// </summary>
    private ShootEscopeta _shoot = null;
    /// <summary>
    /// Posición inicial de Suzie. Deberá volver a ella cada vez que se esconda después de haber asomado para disparar.
    /// </summary>
    private Vector2 _initialPos = Vector2.zero;
    /// <summary>
    /// Bool que dice si Suzie está asomada o no
    /// </summary>
    private bool _isPeeking = false;
    /// <summary>
    /// Último momento en el que Suzie estaba asomando (se guarda cada vez que Suzie se esconde). Inicializado a -99 para que pueda asomar en segundo 0.
    /// </summary>
    private float _lastPeekingMoment = -99f;
    /// <summary>
    /// Variable que tomará valores aleatorios para determinar cuánto tiempo debe permanecer Suzie escondida. Utilizada tanto después de asomar para disparar,
    /// como después de lanzar dinamita.
    /// </summary>
    private float _rndHidingTime = 1f;
    /// <summary>
    /// Momento del ciclo actual de ataques, que determina si Suzie debe disparar o lanzar dinamita
    /// </summary>
    private int _peekingCycle = 0;
    /// <summary>
    /// Transform que guarda la posición del jugador. Se buscará continuamente para determinar las acciones de Suzie.
    /// </summary>
    Transform _playerPos = null;
    /// <summary>
    /// Vida de Suzie cada vez que empieza a asomar
    /// </summary>
    int _currentAttackStartingHealth = 20;
    /// <summary>
    /// Momento en el que Suzie asoma
    /// </summary>
    private float _peekingStartingMoment = 0;
    /// <summary>
    /// Bool que determina si Suzie ha disparado SOLO una vez
    /// </summary>
    private bool _hasShotOnlyOnce = false;
    /// <summary>
    /// Momento en el que Suzie dispara por primera vez desde que asomó
    /// </summary>
    private float _firstShotMoment = 0;
    /// <summary>
    /// Variable que tomará valores aleatorios para determinar cuánto debe esperar Suzie desde que asoma para disparar por primera vez
    /// </summary>
    private float _firstRndShootingMoment = 0.2f;
    /// <summary>
    /// Variable que tomará valores aleatorios para determinar cuánto debe esperar Suzie desde que disparó por última vez (primera desde que asomó), para volver a
    /// disparar
    /// </summary>
    private float _secondRndShootingMoment = 0.4f;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start de programación defensiva que comprueba si hay LevelManager en la escena (para tomar la posición del jugador), en cuyo caso contrario destruirá el
    /// componente ya que sin la posición del jugador, es totalmente inútil.
    /// Además, verifica que Suzie tenga PV y escopeta, para que la función del script sea total.
    /// Después, establece la posición inicial de Suzie.
    /// </summary>
    void Start()
    {
        if (!LevelManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"SuziesFirstPattern\" en una escena sin LevelManager. No podrá tomar la posición del jugador");
            Destroy(this);
        }

        _health = GetComponent<HealthChanger>();
        if (_health == null)
        {
            Debug.Log("Se ha puesto el componente \"SuziesFirstPattern\" en un GameObject sin PV. No podrá esconderse tras recibir daño.");
        }
        
        _shoot = GetComponentInChildren<ShootEscopeta>();
        if (_shoot == null)
        {
            Debug.Log("Se ha puesto el componente \"SuziesFirstPattern\" en un GameObject sin escopeta. No podrá disparar.");
        }

        if (DynamitePrefab == null)
        {
            Debug.Log("Se ha puesto el componente \"SuziesFirstPattern\" en un GameObject sin prefab de dinamita asignado. No podrá lanzarla.");
        }

        _initialPos = transform.position;
    }

    /// <summary>
    /// Update que lleva los ciclos de atacar (escopeta/dinamita)-esconderse, determinando cuándo, cómo, durante cuánto, hasta cuándo...
    /// Más especificado línea a línea por claridad.
    /// </summary>
    void Update()
    {
        // Si no está asomando y ya ha estado escondida todo lo que debería, realiza una acción
        if (!_isPeeking && Time.time - _lastPeekingMoment > _rndHidingTime)
        {
            // Si no le toca lanzar dinamita, asoma para disparar
            if (_peekingCycle != ActionsPerCycle - 1)
            {
                _playerPos = LevelManager.Instance.PlayerTransform();

                // En función de si el jugador está a la derecha o a la izquierda, asoma hacia ese lado
                if (_playerPos.position.x < transform.position.x) SuzieShot(0);
                else if (_playerPos.position.x > transform.position.x) SuzieShot(1);

                // Si está JUSTO delante de Suzie (prácticamente imposible, pero probable), asoma hacia un lado aleatorio
                else
                {
                    int rndPeekingSide01 = UnityEngine.Random.Range(0, 2);
                    SuzieShot(rndPeekingSide01);
                }

                // Guarda la vida al empezar el ataque para después poder determinar si debe esconderse antes de tiempo
                if (_health != null) _currentAttackStartingHealth = _health.GetCurrentHealth();
                _isPeeking = true;
                _peekingStartingMoment = Time.time;
            }
            else SuzieDynamite();

            _peekingCycle = (_peekingCycle + 1) % ActionsPerCycle;
        }

        if (_isPeeking)
        {
            // Si ha perdido suficiente vida, se esconde
            if (_health != null && _health.GetCurrentHealth() <= (_currentAttackStartingHealth - HealthLossToHide)) SuzieHide();

            // Si no, si no ha disparado ninguna vez, y ya le toca, dispara
            else if (!_hasShotOnlyOnce && Time.time - _peekingStartingMoment > _firstRndShootingMoment)
            {
                if (_shoot != null)
                {
                    _playerPos = LevelManager.Instance.PlayerTransform();
                    Vector2 dir = (_playerPos.position - transform.position).normalized;
                    _shoot.ShootBullet(dir);
                }

                _hasShotOnlyOnce = true;
                _firstShotMoment = Time.time;
            }

            // Si no, si ya ha disparado pero solo una vez, y le toca otra, pues vuelve a disparar
            else if (_hasShotOnlyOnce && Time.time - _firstShotMoment > _secondRndShootingMoment)
            {
                if (_shoot != null)
                {
                    _playerPos = LevelManager.Instance.PlayerTransform();
                    Vector2 dir = (_playerPos.position - transform.position).normalized;
                    _shoot.ShootBullet(dir);
                }

                // Tras el segundo disparo, se esconde
                SuzieHide();
            }
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
    /// Método privado que desplaza y rota a Suzie hacia el lado que le corresponda para asomar (si side es 0, va hacia la izquierda, si es 1 va hacia la
    /// derecha.
    /// Además calcula los momentos en los que Suzie debe disparar, que son aleatorios dentro de un intervalo, y que deben ser en momentos mínimamente separados.
    /// </summary>
    /// <param name="side"></param>
    private void SuzieShot(int side)
    {
        if (side == 0)
        {
            transform.position -= new Vector3(DistanceToPeekingPos, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, 45f);
        }
        else
        {
            transform.position += new Vector3(DistanceToPeekingPos, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, -45f);
        }

        _health.AllowDamage();

        _firstRndShootingMoment = UnityEngine.Random.Range(LapseBetweenShots, MaxShootingTime - LapseBetweenShots * 2);
        _secondRndShootingMoment = UnityEngine.Random.Range(_firstRndShootingMoment + LapseBetweenShots, MaxShootingTime);
    }

    /// <summary>
    /// Método privado que llama al de lanzamiento de dinamita en función de la dirección actual de Jugador-Suzie.
    /// Lleva la lógica de tiempo de asomado y escondida por separado al disparo con escopeta, ya que su funcionamiento es ampliamente distinto y no tiene
    /// sentido por como está hecho ahora mismo, que llame al SuzieHide().
    /// </summary>
    private void SuzieDynamite()
    {
        _playerPos = LevelManager.Instance.PlayerTransform();
        MoveToCoordsAndExplode dynamite = DynamitePrefab.GetComponent<MoveToCoordsAndExplode>();
        if (dynamite != null)
        {
            Instantiate(dynamite);
            dynamite.SetFinalPosition(_playerPos.position);
        }

        _lastPeekingMoment = Time.time;
        _rndHidingTime = UnityEngine.Random.Range(1f, MaxHidingTime);

        GetComponent<SuziePhaseManager>().ReportarAtaqueTerminado();
        this.enabled = false;
    }

    /// <summary>
    /// Método privado que establece todos los valores al estado inicial para indicar que Suzie, está escondida. Además, determina mediante un aleatorio cuánto
    /// tiempo estará escondida esta vez.
    /// </summary>
    private void SuzieHide()
    {
        if (_isPeeking)
        {
            _isPeeking = false;
            _hasShotOnlyOnce = false;
            transform.position = _initialPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);

            _lastPeekingMoment = Time.time;
            _rndHidingTime = UnityEngine.Random.Range(1f, MaxHidingTime);

            _health.BlockDamage();
        }
    }
    #endregion   

} // class SuziesFirstPattern 
// namespace
