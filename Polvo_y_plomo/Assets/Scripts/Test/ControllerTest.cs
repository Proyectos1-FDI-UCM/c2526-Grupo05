//---------------------------------------------------------
// Contiene la clase ControllerTest
// Guillermo Jiménez Díaz
// Template-P1
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Añadir aquí el resto de directivas using


/// <summary>
/// Componente simple para comprobar que funcionan los controles usando el InputManager.
/// Si se usan los controles de movimiento, el GameObject se moverá una unidad con respecto
/// a donde se haya creado.
/// Si se pulsa el botón de disparo (_Fire_) entonces se cambia el color del sprite
/// (requiere un componente de tipo SpriteRenderer)
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ControllerTest : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

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
    /// Color original del SpriteRenderer
    /// </summary>
    private Color _spriteColor;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 


    /// <summary>
    /// Se ejecuta al crear el gameobject
    /// Cacheamos el color original 
    /// </summary>
    private void Start()
    {
        if (spriteRenderer != null)
        {
            _spriteColor = spriteRenderer.color;    
        }
    }

    /// <summary>
    /// Se ejecuta en cada frame
    /// Mueve el objeto y cambia el color en función de las acciones del Input
    /// </summary>
    void Update()
    {
        if (InputManager.Instance)
        {
            transform.localPosition = InputManager.Instance.MovementVector;
            if (spriteRenderer != null)
            {
                if (InputManager.Instance.FireWasPressedThisFrame())
                {
                    spriteRenderer.color = Color.green;
                }

                if (InputManager.Instance.FireWasReleasedThisFrame())
                {
                    spriteRenderer.color = this._spriteColor;
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

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class ControllerTest 
// namespace
