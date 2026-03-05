//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
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

    private Shoot _shoot;

    /// <summary>
    /// Esta variable es el número de balas que tendrá disponibles el Objeto con Shoot.
    /// </summary>
    private int _numBalas = 6;

    /// <summary>
    /// Guarda el tiempo desde la última recarga con éxito.
    /// </summary>
    
    private float _tiempoUltimaRecarga;

    private bool _isPlayer;
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
    private void Awake()
    {
        _shoot = GetComponent<Shoot>();
        if (_shoot == null)
        {
            Debug.Log("Se ha puesto el componente HasAmmo en un objeto sin componente Shoot. No funcionará.");
            Destroy(this);
        }

        if (GetComponent<PlayerGetShootingInput>() == null) _isPlayer = false;
        else _isPlayer = true;


        this.enabled = false; // desactiva el update y se activará para la secuencia de recarga bala a bala.
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Time.time - _tiempoUltimaRecarga > Reload)
        {
            _numBalas++;
            if (_numBalas >= NumMaxBalas) this.enabled = false;
            _tiempoUltimaRecarga = Time.time;
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

    public bool IntentaDisparo(Vector2 fireDir)
    {
        if (_numBalas > 0)
        {
            _shoot.ShootBullet(fireDir);
            _numBalas--;
            if (_numBalas == 0) IntentaRecarga();
            return true;  // dispara
        }
        else return false;

    }

    public void IntentaRecarga()
    {
        // Si la recarga ya esta activa, no es necesario volver a recargar
        if (!this.enabled && _numBalas < NumMaxBalas)
        {
            _tiempoUltimaRecarga = Time.time;
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
    private bool IsReloadCanceledThisFrame()
    {
        return (InputManager.Instance.FireWasPressedThisFrame() || InputManager.Instance.RollWasPressedThisFrame() || InputManager.Instance.MeleeWasPressedThisFrame());
    }
    #endregion

} // class HasAmmo 
// namespace
