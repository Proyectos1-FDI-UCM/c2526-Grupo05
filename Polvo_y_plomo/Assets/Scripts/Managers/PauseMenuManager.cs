//---------------------------------------------------------
// Maneja la lógica del menú de pausa.
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
public class PauseMenuManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Almacena el panel con todos los botones del menu de pausa.
    /// Tendra 3 botones; "Reanudar partida", "Settings" y "Volver al menú"
    /// </summary>
    [SerializeField]
    private GameObject PausePanel;

    /// <summary>
    /// Almacena el panel con toda la lógica de los settings.
    /// </summary>
    [SerializeField]
    private GameObject SettingsPanel;

    /// <summary>
    /// Almacena el panel que se abrirá al presionar por primera vez "Volver al menu".
    /// Da un aviso sobre la perdida del progreso si se vuelve al menú.
    /// Tendrá 2 botones: uno para "Si, quiero salir" y otro para "No, no quiero salir".
    /// </summary>
    [SerializeField]
    private GameObject WarningPanel;

    /// <summary>
    /// Componente CursorBloqueado que deberá ser asignada si existe en la escena para
    /// liberar el mouse tras presionar "Reanudar partida".
    /// </summary>
    [SerializeField]
    private CursorBloqueado BlockCursor;

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
    /// Booleano que registra si el juego esta pausado (menu abierto)
    /// </summary>
    private bool _gamePaused = false;

    /// <summary>
    /// Booleano que registra si se han abierto los settings.
    /// </summary>
    private bool _settingsOpen = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al cargarse en la escena.
    /// Hace comprobaciones necesarias para el componente.
    /// </summary>
    private void Awake()
    {
        if (PausePanel == null)
        {
            Debug.Log("No se ha puesto en el PauseMenuManager un panel de menu de pausa. No funcionará");
            Destroy(this);
        }

        if (SettingsPanel == null)
        {
            Debug.Log("No se ha asignado al PauseMenuManager un panel de settings. No funcionará");
            Destroy(this);
        }

        if (WarningPanel == null)
        {
            Debug.Log("No se ha asignado al PauseMenumMaager un panel de warning. No funcionará");
            Destroy(this);
        }

        if (BlockCursor == null)
        {
            Debug.Log("No se ha asiginado al PauseMenuManager un componente CursorBloqueado. Se asume que no existe en la escena."
                + " Si no es así, el bloqueo del mouse podrá funcionar mal.");
        }
    }

    /// <summary>
    /// Se llama al cargarse en la escena si esta activo, o al activarse por primera vez.
    /// Hace comprobaciones necesarias para el componente.
    /// </summary>
    private void Start()
    {
        if (!InputManager.HasInstance())
        {
            Debug.Log("PauseMenuManager puesto en una escena sin InputManager. No funcionará");
            Destroy(this);
        }

        if (!GameManager.HasInstance())
        {
            Debug.Log("PauseMenuManager puesto en una escena sin GameManager. No funcionará.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Verífica si se ha presionado la tecla de pausa
    /// </summary>
    private void Update()
    {
        if (InputManager.Instance.ExitWasPressedThisFrame())
        {
            if (_settingsOpen) // settings abierto (y por construccion juego pausado) -> cerrar settings
            {
                _settingsOpen = false;
                OpenPauseMenuFromInput();
            }
            else // settings cerrado... ¿esta abierto el menu?
            {
                if (!_gamePaused) // menu no abierto -> abrir pausa
                {
                    if (BlockCursor != null)
                    {
                        BlockCursor.enabled = false; // quizas ya se ha liberado el cursor. Lo apago ya que es posible presionar Exit en el menu
                        BlockCursor.UnlockCursor(); // es posible por el orden de ejecución de las cosas que no se haya liberado, lo fuerzo.
                    }

                    _gamePaused = true;
                    InputManager.Instance.DesactivarInput();
                    GameManager.Instance.PauseGame(); // pausar el juego
                    OpenPauseMenuFromInput();
                }
                else // menu abierto -> cerrar la pausa
                {
                    if (BlockCursor != null)
                    {
                        BlockCursor.enabled = true; // se reactiva y la proxima vez que se active Exit se abre el menu con el cursor liberado
                        BlockCursor.LockCursor(); // bloqueo el mouse
                    }

                    _gamePaused = false;
                    _settingsOpen = false;
                    InputManager.Instance.ActivarInput();
                    GameManager.Instance.ResumeGame(); // reanudar el juego
                    PausePanel.SetActive(false);
                }
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

    /// <summary>
    /// Abre el menú de pausa (settings desactivado).
    /// Actualiza la lógica para indicar que se ha cerrado el panel de Settings.
    /// </summary>
    public void OpenPauseMenuFromButton()
    {
        OpenPauseMenuFromInput();
        _settingsOpen = false;
    }

    /// <summary>
    /// Abre el menú de settings (pausa desactivado).
    /// Actualiza la lógica para indicar que se ha abierto el panel de Settings.
    /// </summary>
    public void OpenSettingsMenuFromButton()
    {
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(true);
        _settingsOpen = true;
    }

    /// <summary>
    /// Reanudar el juego.
    /// </summary>
    public void ResumeGameFromButton()
    {
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        _settingsOpen = false;
        _gamePaused = false;

        if (BlockCursor != null)
        {
            BlockCursor.enabled = true; // se reactiva y la proxima vez que se active Exit se abre el menu con el cursor liberado
            BlockCursor.LockCursor(); // bloqueo el mouse
        }
        
        InputManager.Instance.ActivarInput();
        GameManager.Instance.ResumeGame(); // reanudar el juego
    }


    /// <summary>
    /// Metodo llamado por el boton de "Volver al menu" que abrirá el panel de advertencia
    /// sobre la perdida del progreso al volver al menú principal.
    /// </summary>
    public void OpenWarningMenuFromButton()
    {
        WarningPanel.SetActive(true);
    }

    /// <summary>
    /// Método llamado por el boton dentro del panel de advertencia de "No, no quiero salir".
    /// </summary>
    public void CloseWarningMenuFromButton()
    {
        WarningPanel.SetActive(false);
    }

    /// <summary>
    /// Volver al menu principal (boton definitivo, de "Si, quiero salir")
    /// (!) Se asume que el menu principal esta en la escena de build index 0.
    /// </summary>
    public void GoToMainMenuFromButton()
    {
        GameManager.Instance.MatchEnded();
        GameManager.Instance.ChangeScene(0);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Abre el menú de pausa con el input (settings desactivado).
    /// No actualiza lógica, solo abre el menú (la lógica se llevará en el procesamiento del input)
    /// </summary>
    private void OpenPauseMenuFromInput()
    {
        PausePanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }
    #endregion

} // class PauseMenuManager 
// namespace
