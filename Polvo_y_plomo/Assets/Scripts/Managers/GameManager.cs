//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro P. Gómez Martín
// Marco A. Gómez Martín
// Template-P1
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Componente responsable de la gestión global del juego. Es un singleton
/// que orquesta el funcionamiento general de la aplicación,
/// sirviendo de comunicación entre las escenas.
///
/// El GameManager ha de sobrevivir entre escenas por lo que hace uso del
/// DontDestroyOnLoad. En caso de usarlo, cada escena debería tener su propio
/// GameManager para evitar problemas al usarlo. Además, se debería producir
/// un intercambio de información entre los GameManager de distintas escenas.
/// Generalmente, esta información debería estar en un LevelManager o similar.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Componente con el FadeIn configurado
    /// Realizará un FadeIn de pantalla negra al morir el jugador.
    /// </summary>
    [SerializeField]
    private FadeColor FadeIn;

    /// <summary>
    /// Componente con el FadeOut configurado
    /// Realizará un FadeOut de pantalla negra al reaparecer el jugador.
    /// </summary>
    [SerializeField]
    private FadeColor FadeOut;

    /// <summary>
    /// Tiempo que tardara la escena en reiniciarse
    /// </summary>
    [SerializeField]
    private float TiempoEsperaRespawn = 3f;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Esta es la vida actual del jugador.
    /// Inicializada en 10 por ser con la que empieza.
    /// </summary>
    private int _vidaJugador = 10;

    /// <summary>
    /// Esta es la munición actual del jugador.
    /// Inicializada en 6 por ser en la que empieza.
    /// </summary>
    private int _municionJugador = 6;

    /// <summary>
    /// Guarda un tiempo concreto. Usado para esperar en el Update().
    /// </summary>
    private float _t;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// Desactiva el componente para evitar que se corra updates.
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de este manager al que ya existe.
            // Esto permitirá al GameManager real mantener su estado interno
            // pero acceder a los elementos de la nueva escena
            // o bien olvidar los de la escena previa de la que venimos
            TransferManagerSetup();

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
            this.enabled = false;
        } // if-else somos instancia nueva o no.
    }

    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
    }

    /// <summary>
    /// Se llama cada frame si el componente está activo.
    /// Por ahora solo sirve para esperar un cierto tiempo, según lo necesita Respawn().
    /// </summary>
    private void Update()
    {
        if (Time.time - _t > TiempoEsperaRespawn)
        {
            ReinicioEscena();
            this.enabled = false;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(int index)
    {
        // Antes y después de la carga fuerza la recolección de basura, por eficiencia,
        // dado que se espera que la carga tarde un tiempo, y dado que tenemos al
        // usuario esperando podemos aprovechar para hacer limpieza y ahorrarnos algún
        // tirón en otro momento.
        // De Unity Configuration Tips: Memory, Audio, and Textures
        // https://software.intel.com/en-us/blogs/2015/02/05/fix-memory-audio-texture-issues-in-unity
        //
        // "Since Unity's Auto Garbage Collection is usually only called when the heap is full
        // or there is not a large enough freeblock, consider calling (System.GC..Collect) before
        // and after loading a level (or put it on a timer) or otherwise cleanup at transition times."
        //
        // En realidad... todo esto es algo antiguo por lo que lo mismo ya está resuelto)
        System.GC.Collect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    } // ChangeScene

    /// <summary>
    /// Este metodo actualiza la vida en el HUD
    /// </summary>
    public void UpdateHealthHUD(int NuevaVidaJugador)
    {
        _vidaJugador = NuevaVidaJugador;
        // codigo update HUD vida
    }

    public void UpdateAmmoHUD(int NuevaMunicionJugador)
    {
        _municionJugador = NuevaMunicionJugador;
        // codigo update HUD ammo
    }

    /// <summary>
    /// Método que se encarga de llevar los procesos tras la muerte del jugador.
    /// Desactiva el input del jugador, inicia un FadeIn de pantalla negra y activa este componente para que en
    /// el Update() se lleve un contador para esperar al FadeIn y luego reiniciar la escena.
    /// Se llama desde HealthChanger, cuando muere el jugador.
    /// </summary>
    public void Respawn()
    {
        InputManager.Instance.DesactivarInput();
        if (FadeIn != null) FadeIn.enabled = true;
        else Debug.Log("Componente FadeColor de FadeIn no asignado");
        _t = Time.time;
        this.enabled = true; // comienza el temporizador en el update
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // De momento no hay nada que inicializar
    }

    /// <summary>
    /// Transfiere datos importantes de un GameManager que ha de destruirse al activo.
    /// </summary>
    private void TransferManagerSetup()
    {
        // De momento no hay que transferir ningún setup
        // a otro manager
    }

    /// <summary>
    /// Reinicia la escena actual, activa el FadeOut de la pantalla negra y reactiva el input del jugador.
    /// </summary>
    private void ReinicioEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (FadeOut != null) FadeOut.enabled = true;
        else Debug.Log("Componente FadeColor de FadeOut no asignado");

        InputManager.Instance.ActivarInput();
    }
    #endregion
} // class GameManager 
// namespace