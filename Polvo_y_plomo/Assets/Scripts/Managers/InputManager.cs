    //---------------------------------------------------------
// Contiene el componente de InputManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// Template-P1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Manager para la gestión del Input. Se encarga de centralizar la gestión
/// de los controles del juego. Es un singleton que sobrevive entre
/// escenas.
/// La configuración de qué controles realizan qué acciones se hace a través
/// del asset llamado InputActionSettings que está en la carpeta Settings.
/// 
/// A modo de ejemplo, este InputManager tiene métodos para consultar
/// el estado de dos acciones:
/// - Move: Permite acceder a un Vector2D llamado MovementVector que representa
/// el estado de la acción Move (que se puede realizar con el joystick izquierdo
/// del gamepad, con los cursores...)
/// - Fire: Se proporcionan 3 métodos (FireIsPressed, FireWasPressedThisFrame
/// y FireWasReleasedThisFrame) para conocer el estado de la acción Fire (que se
/// puede realizar con la tecla Space, con el botón Sur del gamepad...)
///
/// Dependiendo de los botones que se quieran añadir, será necesario ampliar este
/// InputManager. Para ello:
/// - Revisar lo que se hace en Init para crear nuevas acciones
/// - Añadir nuevos métodos para acceder al estado que estemos interesados
///  
/// </summary>
public class InputManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static InputManager _instance;

    /// <summary>
    /// Controlador de las acciones del Input. Es una instancia del asset de 
    /// InputAction que se puede configurar desde el editor y que está en
    /// la carpeta Settings
    /// </summary>
    private InputSystem_Actions _theController;
    
    /// <summary>
    /// Acción para Fire. Si tenemos más botones tendremos que crear más
    /// acciones como esta (y crear los métodos que necesitemos para
    /// conocer el estado del botón)
    /// </summary>
    private InputAction _fire;


    /// <summary>
    /// Acción para Melee.
    /// </summary>
    private InputAction _melee;

    /// <summary>
    /// Acción para Reload
    /// </summary>
    private InputAction _reload;

    /// <summary>
    /// Acción para Hability
    /// </summary>
    private InputAction _hability;

    /// <summary>
    /// Acción para Roll
    /// </summary>
    private InputAction _roll;

    /// <summary>
    /// Acción para Exit.
    /// </summary>
    private InputAction _exit;


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// 
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // InputManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Nos destruímos. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer InputManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
    } // Awake

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
    } // OnDestroy

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static InputManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    } // Instance

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>True si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Propiedad para acceder al vector de movimiento.
    /// Según está configurado el InputActionController,
    /// es un vector normalizado 
    /// </summary>
    public Vector2 MovementVector { get; private set; }


    /// <summary>
    /// Propiedad para acceder al vector de la acción look.
    /// No estará normalizado.
    /// </summary>
    public Vector2 LookVector { get; private set; }


    #region Metodos para "Fire"
    /// <summary>
    /// Método para saber si el botón de disparo (Fire) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool FireIsPressed()
    {
        return _fire.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de disparo (Fire) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool FireWasPressedThisFrame()
    {
        return _fire.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de disparo (Fire) ha dejado de pulsarse
    /// durante este frame
    /// <returns>Devuelve true, si el botón se ha dejado de pulsar en
    /// este frame; y false, en otro caso.
    /// </returns>
    /// </summary>
    public bool FireWasReleasedThisFrame()
    {
        return _fire.WasReleasedThisFrame();
    }
    #endregion
    #region Metodos para "Melee"

    /// <summary>
    /// Método para saber si el botón de melee (Melee) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool MeleeIsPressed()
    {
        return _melee.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de melee (Melee) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool MeleeWasPressedThisFrame()
    {
        return _melee.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de melee (Melee) ha dejado de pulsarse
    /// durante este frame
    /// <returns>Devuelve true, si el botón se ha dejado de pulsar en
    /// este frame; y false, en otro caso.
    /// </returns>
    /// </summary>
    public bool MeleeWasReleasedThisFrame()
    {
        return _melee.WasReleasedThisFrame();
    }
    #endregion
    #region Metodos para "Reload"
    /// <summary>
    /// Método para saber si el botón de recarga (Reload) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool ReloadIsPressed()
    {
        return _reload.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de recarga (Reload) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool ReloadWasPressedThisFrame()
    {
        return _reload.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de recarga (Reload) ha dejado de pulsarse
    /// durante este frame
    /// <returns>Devuelve true, si el botón se ha dejado de pulsar en
    /// este frame; y false, en otro caso.
    /// </returns>
    /// </summary>
    public bool ReloadWasReleasedThisFrame()
    {
        return _reload.WasReleasedThisFrame();
    }
    #endregion
    #region Metodos para "Hability"
    /// <summary>
    /// Método para saber si el botón de habilidad activa (Hability) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool HabilityIsPressed()
    {
        return _hability.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de habilidad activa (Hability) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool HabilityWasPressedThisFrame()
    {
        return _hability.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de habilidad activa (Hability) ha dejado de pulsarse
    /// durante este frame
    /// <returns>Devuelve true, si el botón se ha dejado de pulsar en
    /// este frame; y false, en otro caso.
    /// </returns>
    /// </summary>
    public bool HabilityWasReleasedThisFrame()
    {
        return _hability.WasReleasedThisFrame();
    }
    #endregion
    #region Metodos para "Roll"
    /// <summary>
    /// Método para saber si el botón de roll (Roll) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool RollIsPressed()
    {
        return _roll.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de roll (Roll) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool RollWasPressedThisFrame()
    {
        return _roll.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de roll (Roll) ha dejado de pulsarse
    /// durante este frame
    /// <returns>Devuelve true, si el botón se ha dejado de pulsar en
    /// este frame; y false, en otro caso.
    /// </returns>
    /// </summary>
    public bool RollWasReleasedThisFrame()
    {
        return _roll.WasReleasedThisFrame();
    }
    #endregion
    #region Metodos para "Exit"
    /// <summary>
    /// Método para saber si el botón de pausa (Exit) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool ExitIsPressed()
    {
        return _exit.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de pausa (Exit) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool ExitWasPressedThisFrame()
    {
        return _exit.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de pausa (Exit) ha dejado de pulsarse
    /// durante este frame
    /// <returns>Devuelve true, si el botón se ha dejado de pulsar en
    /// este frame; y false, en otro caso.
    /// </returns>
    /// </summary>
    public bool ExitWasReleasedThisFrame()
    {
        return _exit.WasReleasedThisFrame();
    }
    #endregion



    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // Creamos el controlador del input y activamos los controles del jugador
        _theController = new InputSystem_Actions();
        _theController.Player.Enable();

        // Cacheamos la acción de movimiento
        InputAction movement = _theController.Player.Move;
        // Por si alguien lo borra
        if (movement == null) Debug.Log("Se ha borrado la acción \"Move\" y el jugador no se va a poder mover");
        else
        {
            // Para el movimiento, actualizamos el vector de movimiento usando
            // el método OnMove
            movement.performed += OnMove;
            movement.canceled += OnMove;
        }

        // Cacheo la acción de mirar
        InputAction look = _theController.Player.Look;
        // Por si la borran
        if (look == null) Debug.Log("Se ha borrado la acción \"Look\" y el jugador no se va a poder mover el cursor");
        else
        {
            look.performed += OnLook;
            look.canceled += OnLook;
        }

        // Para el disparo solo cacheamos la acción de disparo.
        // El estado lo consultaremos a través de los métodos públicos que 
        // tenemos (FireIsPressed, FireWasPressedThisFrame 
        // y FireWasReleasedThisFrame)
        _fire = _theController.Player.Fire;
        // Por si la borran
        if (_fire == null) Debug.Log("Se ha borrado la acción \"Fire\" y le jugador no podrá disparar");

        // Para el melee hago lo mismo que el disparo.
        // Tendrá métodos MeleeIsPressed, MeleeWasPressedTihsFrame y MeleeWasReleasedThisFrame.
        _melee = _theController.Player.Melee;
        // Por si la borran
        if (_melee == null) Debug.Log("Se ha borrado la acción \"Melee\" y el jugador no podrá atacar a melee");

        // Y a partir de aqui lo mismo...
        // Para la recarga
        // Tendrá métodos ReloadIsPressed, ReloadWasPressedTihsFrame y ReloadWasReleasedThisFrame.
        _reload = _theController.Player.Reload;
        // Por si la borran
        if (_reload == null) Debug.Log("Se ha borrado la acción \"Reload\" y el jugador no podrá recargar");

        // Para la habilidad activa
        // Tendrá métodos HabilityIsPressed, HabilityWasPressedTihsFrame y HabilityWasReleasedThisFrame.
        _hability = _theController.Player.Hability;
        // Por si la borran
        if (_hability == null) Debug.Log("Se ha borrado la acción \"Hability\" y el jugador no podrá usar su habilidad activa");

        // Para el roll
        // Tendrá métodos RollIsPressed, RollWasPressedTihsFrame y RollWasReleasedThisFrame.
        _roll = _theController.Player.Roll;
        // Por si la borran
        if (_roll == null) Debug.Log("Se ha borrado la acción \"Roll\" y el jugador no podrá usar su roll");

        // Para el exit
        // Tendrá métodos ExitIsPressed, ExitWasPressedTihsFrame y ExitWasReleasedThisFrame.
        _exit = _theController.Player.Exit;
        // Por si la borran
        if (_exit == null) Debug.Log("Se ha borrado la acción \"Exit\" y el jugador no podrá abrir el menú de pausa");

    }

    /// <summary>
    /// Método que es llamado por el controlador de input cuando se producen
    /// eventos de movimiento (relacionados con la acción Move)
    /// </summary>
    /// <param name="context">Información sobre el evento de movimiento</param>
    private void OnMove(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        LookVector = context.ReadValue<Vector2>();
    }

    #endregion
} // class InputManager 
// namespace