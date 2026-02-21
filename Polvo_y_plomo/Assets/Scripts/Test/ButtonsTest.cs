//---------------------------------------------------------
// Manda mensajes de Debug.Log() para comprobar las acciones de boton (disparo, melee, recarga, habilidad, roll y exit)
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente para testear las acciones de tipo "boton" del InputManager
/// Manda mensajes usando Debug.Log para cada acción.
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
    }
    /// <summary>
    /// Se llama cada frame.
    /// Comprueba cada método de las acciones Fire y Melee del InputManager.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.FireIsPressed()) Debug.Log("Fire is pressed");
        if (InputManager.Instance.FireWasPressedThisFrame()) Debug.Log("Fire was pressed this frame");
        if (InputManager.Instance.FireWasReleasedThisFrame()) Debug.Log("Fire was released this frame");

        if (InputManager.Instance.MeleeIsPressed()) Debug.Log("Melee is pressed");
        if (InputManager.Instance.MeleeWasPressedThisFrame()) Debug.Log("Melee was pressed this frame");
        if (InputManager.Instance.MeleeWasReleasedThisFrame()) Debug.Log("Melee was released this frame");

        if (InputManager.Instance.ReloadIsPressed()) Debug.Log("Reload is pressed");
        if (InputManager.Instance.ReloadWasPressedThisFrame()) Debug.Log("Reload was pressed this frame");
        if (InputManager.Instance.ReloadWasReleasedThisFrame()) Debug.Log("Reload was released this frame");

        if (InputManager.Instance.HabilityIsPressed()) Debug.Log("Hability is pressed");
        if (InputManager.Instance.HabilityWasPressedThisFrame()) Debug.Log("Hability was pressed this frame");
        if (InputManager.Instance.HabilityWasReleasedThisFrame()) Debug.Log("Hability was released this frame");

        if (InputManager.Instance.RollIsPressed()) Debug.Log("Roll is pressed");
        if (InputManager.Instance.RollWasPressedThisFrame()) Debug.Log("Roll was pressed this frame");
        if (InputManager.Instance.RollWasReleasedThisFrame()) Debug.Log("Roll was released this frame");

        if (InputManager.Instance.ExitIsPressed()) Debug.Log("Exit is pressed");
        if (InputManager.Instance.ExitWasPressedThisFrame()) Debug.Log("Exit was pressed this frame");
        if (InputManager.Instance.ExitWasReleasedThisFrame()) Debug.Log("Exit was released this frame");
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
