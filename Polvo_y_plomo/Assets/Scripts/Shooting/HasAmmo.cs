//---------------------------------------------------------
// Intermediario que añade funcionalidad de municion a las armas de fuego.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente que se puede colocar opcionalmente entre "Controlador de disparo" y "Generador de disparo (componente Shoot)".
/// Contiene la lógica para poder recargar cuando el controlador se lo diga e intentar un disparo (que no realiza si no hay munición).
/// Necesariamente ha de ponerse en el mismo GameObject en el que se situa el compnoente Shoot.
/// 
/// (!) Por ahora solo tiene lógica para parar la recarga del jugador. Se puede implementar facilmente para un enemigo con un nuevo
/// método "CancelaRecarga()" o algo por el estilo (como no tenemos pensado añadirlo no esta implementado).
/// 
/// +++
/// Se ha reescrito un poco el código para incluir el ShootEscopeta
/// </summary>
public class HasAmmo : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Variable float que guarda el tiempo de recarga del disparo. Actualizada en el Update cada vez que este se realiza.
    /// </summary>
    [SerializeField]
    private float Reload = 0.3f;

    /// <summary>
    /// Esta variable es el número de balas máximas que tendrá disponibles el Objeto con Shoot.
    /// </summary>
    [SerializeField]
    private int NumMaxBalas = 6;

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
    /// Guarda el componente Shoot del GameObject con munición.
    /// </summary>
    private Shoot _shoot;

    /// <summary>
    /// Guarda el componente ShootEscopeta del GameObject con munición.
    /// </summary>
    private ShootEscopeta _shootEscopeta;

    /// <summary>
    /// Esta variable es el número de balas que tendrá disponibles el Objeto con Shoot.
    /// </summary>
    private int _numBalas = 6;

    /// <summary>
    /// Guarda el tiempo que falta para recargar otra bala desde el inicio de esta y bala por bala.
    /// </summary>
    private float _tParaSiguienteRecarga;

    /// <summary>
    /// Variable que determina si este componente esta puesto en el jugador (ducktyping
    /// de controlador de player). Inicializada en el Awake().
    /// </summary>
    private bool _isPlayer;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama una vez al cargarse en escena.
    /// Hace comprobaciones necesarias para el componente y registra si somos jugador.
    /// Desactiva este componente para evitar llamadas innecesarias al Update().
    /// </summary>
    private void Awake()
    {
        _shoot = GetComponent<Shoot>();
        _shootEscopeta = GetComponent<ShootEscopeta>();
        if (_shoot == null && _shootEscopeta == null)
        {
            Debug.Log("Se ha puesto el componente HasAmmo en un objeto sin componente Shoot ni ShootEscopeta. No funcionará.");
            Destroy(this);
        }

        if (GetComponent<PlayerGetShootingInput>() == null) _isPlayer = false;
        else _isPlayer = true;

        _numBalas = NumMaxBalas;

        this.enabled = false; // desactiva el update y se activará para la secuencia de recarga bala a bala.
    }

    /// <summary>
    /// Se llama cada frame.
    /// Lleva la lógica de recarga bala a bala.
    /// Si se dispara, hace roll o ataque melee, la recarga se para.
    /// </summary>
    void Update()
    {
        if (GameManager.HasInstance()) _tParaSiguienteRecarga -= Time.deltaTime * GameManager.SlowMultiplier;
        else _tParaSiguienteRecarga -= Time.deltaTime;

        if (_tParaSiguienteRecarga <= 0)
        {
            _numBalas++;
            if (_isPlayer && GameManager.HasInstance()) GameManager.Instance.UpdateAmmoHUD(_numBalas);
            if (_numBalas >= NumMaxBalas) this.enabled = false;
            _tParaSiguienteRecarga = Reload;
        }
        
        if (_isPlayer && IsReloadCanceledThisFrame() && _numBalas > 0)
        {
            this.enabled = false; // deja de recargar al cancelarse la acción.
        } // necesario comprobar _numBalas ya que es posible que se cancele mientras que el jugador tiene 0 balas, evitando la recarga automática.
    }

    /// <summary>
    /// Si el componente es destruido por no poder funcionar, se asegura que el resto de componentes dejen de funcionar también.
    /// No puede destruir el controlador del disparo puesto que aquí no sabemos que script será.
    /// </summary>
    private void OnDestroy()
    {
        if (_shoot != null) Destroy(_shoot);
        if (_shootEscopeta != null) Destroy(_shootEscopeta);
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
    /// Método que recibe el intento de disparo desde el Controlador.
    /// Si el disparo es posible, le comunica al componente _shoot que dispare.
    /// Esto consume munición y si se llega a 0, se hace un intento de recarga automático.
    /// 
    /// Devuelve si el disparo ha sido exitoso o no.
    /// </summary>
    /// <param name="fireDir"></param>
    /// <returns></returns>
    public bool IntentaDisparo(Vector2 fireDir)
    {
        if (_numBalas > 0)
        {
            // Ejecuta el disparo dependiendo de qué componente tenga el arma
            if (_shoot != null) _shoot.ShootBullet(fireDir);
            else if (_shootEscopeta != null) _shootEscopeta.ShootBullet(fireDir);

            _numBalas--;
            if (_numBalas == 0) IntentaRecarga();
            if (_isPlayer && GameManager.HasInstance()) GameManager.Instance.UpdateAmmoHUD(_numBalas);
            return true;  // dispara
        }
        else
        {
            return false;
        }

    }

    /// <summary>
    /// Intenta iniciar la recarga activando este componente.
    /// Evita recargar si ya se tiene la máxima capacidad de balas o si ya se esta recargando
    /// (consiguiendo que no se reinicie la recarga).
    /// </summary>
    public void IntentaRecarga()
    {
        // Si la recarga ya esta activa, no es necesario volver a recargar
        if (!this.enabled && _numBalas < NumMaxBalas)
        {
            _tParaSiguienteRecarga = Reload;
            this.enabled = true; // inicia la recarga en el update.
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Devuelve un booleano indicando si en este frame se ha pulsado alguna tecla que
    /// deberá parar la recarga.
    /// Solo sirve para el jugador.
    /// </summary>
    /// <returns></returns>
    private bool IsReloadCanceledThisFrame()
    {
        return (InputManager.Instance.FireWasPressedThisFrame() || InputManager.Instance.RollWasPressedThisFrame() || InputManager.Instance.MeleeWasPressedThisFrame());
    }
    #endregion

} // class HasAmmo 
// namespace
