//---------------------------------------------------------
// Modifica un texto asignable para comprobar las acciones de boton (disparo, melee, recarga, habilidad, roll y exit)
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente para testear las acciones de tipo "boton" del InputManager
/// Cambia el texto del TextMeshProUGUI asignado para indicar que inputs se estan usando.
/// </summary>
public class ButtonsTest : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Escribirá el input recibido en este texto.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI TextoDeInput;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Se llama la primera vez que el componente esta activo, después del Awake.
    /// Realiza comprobaciones necesarias para el componente.
    /// </summary>
    private void Start()
    {
        if (!InputManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"ButtonsTest\" en una escena sin InputManager. No funcionará.");
            Destroy(this);
        }
        if (TextoDeInput == null)
        {
            Debug.Log("Se ha puesto el componente \"ButtonsTest\" sin asignarle TextoDeInput. No funcionará");
            Destroy(this);
        }
    }
    /// <summary>
    /// Se llama cada frame.
    /// Comprueba cada método de acciones en el InputManager.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.FireIsPressed()) TextoDeInput.text = "Fire is pressed";
        if (InputManager.Instance.FireWasPressedThisFrame()) TextoDeInput.text = "Fire was pressed this frame";
        if (InputManager.Instance.FireWasReleasedThisFrame()) TextoDeInput.text = "Fire was released this frame";

        if (InputManager.Instance.MeleeIsPressed()) TextoDeInput.text = "Melee is pressed";
        if (InputManager.Instance.MeleeWasPressedThisFrame()) TextoDeInput.text = "Melee was pressed this frame";
        if (InputManager.Instance.MeleeWasReleasedThisFrame()) TextoDeInput.text = "Melee was released this frame";

        if (InputManager.Instance.ReloadIsPressed()) TextoDeInput.text = "Reload is pressed";
        if (InputManager.Instance.ReloadWasPressedThisFrame()) TextoDeInput.text = "Reload was pressed this frame";
        if (InputManager.Instance.ReloadWasReleasedThisFrame()) TextoDeInput.text = "Reload was released this frame";

        if (InputManager.Instance.HabilityIsPressed()) TextoDeInput.text = "Hability is pressed";
        if (InputManager.Instance.HabilityWasPressedThisFrame()) TextoDeInput.text = "Hability was pressed this frame";
        if (InputManager.Instance.HabilityWasReleasedThisFrame()) TextoDeInput.text = "Hability was released this frame";

        if (InputManager.Instance.RollIsPressed()) TextoDeInput.text = "Roll is pressed";
        if (InputManager.Instance.RollWasPressedThisFrame()) TextoDeInput.text = "Roll was pressed this frame";
        if (InputManager.Instance.RollWasReleasedThisFrame()) TextoDeInput.text = "Roll was released this frame";

        if (InputManager.Instance.ExitIsPressed()) TextoDeInput.text = "Exit is pressed";
        if (InputManager.Instance.ExitWasPressedThisFrame()) TextoDeInput.text = "Exit was pressed this frame";
        if (InputManager.Instance.ExitWasReleasedThisFrame()) TextoDeInput.text = "Exit was released this frame";

        if (InputManager.Instance.MovementVector != Vector2.zero) TextoDeInput.text = "Move is being used and has a value of (" + InputManager.Instance.MovementVector.x + ", " + InputManager.Instance.MovementVector.y + ").";
        if (InputManager.Instance.LookVector != Vector2.zero) TextoDeInput.text = "Look is being used and has a value of (" + InputManager.Instance.LookVector.x + ", " + InputManager.Instance.LookVector.y + ").";
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

} // class FireNMeleeTest 
// namespace
