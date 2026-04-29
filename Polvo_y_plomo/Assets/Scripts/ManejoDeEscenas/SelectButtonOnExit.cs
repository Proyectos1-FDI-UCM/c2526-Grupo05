//---------------------------------------------------------
// Para escenas de test. Al presionar la accion Exit se seleccionará un botón asignable.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
// Añadir aquí el resto de directivas using


/// <summary>
/// Para escenas de test. Al presionar la accion Exit se seleccionará un botón asignable.
/// </summary>
public class SelectButtonOnExit : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Boton que se seleccionará al presionar la accion de Exit.
    /// </summary>
    [SerializeField]
    private GameObject Button;

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

    private void Start()
    {
        if (Button == null)
        {
            Debug.Log("Componente SelectButtonOnExit puesto sin asignarle boton. No funcionará");
            Destroy(this);
        }

        if (!InputManager.HasInstance())
        {
            Debug.Log("Componente SelectButtonOnExit puesto en escena sin InputManager. No funcionará");
            Destroy(this);
        }
    }
    void Update()
    {
        if (InputManager.Instance.ExitWasPressedThisFrame())
        {
            EventSystem.current.SetSelectedGameObject(Button);
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

} // class SelectButtonOnExit 
// namespace
