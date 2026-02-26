//---------------------------------------------------------
// Script que gestiona el bloqueo y visibilidad del cursor
// Samuel Asensio Torres
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este código nos permite bloquear el cursor del ratón en el centro
/// y su visibilidad en el juego para mejorar la experiencia del juego.
/// 
/// A su vez, permite controlar el estado mediante la tecla Escape
/// </summary>
public class CursorBloqueado : MonoBehaviour
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

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Comienzo la partida bloqueando el cursor.
    /// Comprobamos que el InputManager existe antes de empezar.
    /// </summary>
    void Start()
    {
        if (!InputManager.HasInstance())
        {
            Debug.Log("CursorBloqueado: No se encontró InputManager. El script no funcionará.");
            Destroy(this);
        }

        LockCursor();
    }

    /// <summary>
    /// Alternancia de estado de pulsación de la tecla escape.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.ExitWasPressedThisFrame())
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
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
    /// Bloquearemos el ratón en el centro de la pantalla.
    /// Y su visibilidad será nula, es decir, invisible, es más estético esto para no molestar.
    /// </summary>
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Rompe el bloqueo y permite que el ratón pueda moverse libremente.
    /// A su vez, se vuelve a hacer visible.
    /// </summary>
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion

} // class CursorBloqueado 
// namespace
