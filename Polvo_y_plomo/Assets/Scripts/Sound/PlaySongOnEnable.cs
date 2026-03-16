//---------------------------------------------------------
// Componente al que se le puede asignar un AudioClip de música que sonará al activarse el componente.
// Ängel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente que llama al AudioManager al ser activado para hacer que suene
/// como canción un AudioClip asignable.
/// Se desactiva trás la activación.
/// </summary>
public class PlaySongOnEnable : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// AudioClip (canción) que sonará al activarse este componente.
    /// </summary>
    [SerializeField]
    private AudioClip Music;

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
    /// Lleva si se ha llamado al Start o no.
    /// Esto es porque para una llamada al cargarse en la escena es necesario esperar a la 
    /// inicialización del AudioManager (no sirve ponerla en OnEnable().
    /// Una vez se llama, se usa el OnEnable().
    /// </summary>
    private bool _startHasBeenDone = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al cargarse en escena.
    /// Hace comprobaciones necesarias para el componente.
    /// </summary>
    private void Awake()
    {
        if (Music == null)
        {
            Debug.Log("Componente \"PlaySongOnEnable\" puesto sin asignarle Music. No funcionará.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Se llama una vez si el componente esta activo, o al activarse por primera vez.
    /// Lleva la primera activación del componente, después del Awake(). Es necesario
    /// por problemas de orden de inicialización con OnEnable().
    /// </summary>
    private void Start()
    {
        if (AudioManager.HasInstance())
        {
            AudioManager.Instance.PlayMusic(Music);
        }
        this.enabled = false;
        _startHasBeenDone = true;
    }

    /// <summary>
    /// Se llama cada vez que se activa el componente.
    /// </summary>
    private void OnEnable()
    {
        if (_startHasBeenDone)
        {
            if (AudioManager.HasInstance())
            {
                AudioManager.Instance.PlayMusic(Music);
            }
            this.enabled = false;
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

    #endregion

} // class PlaySongOnEnable 
// namespace
