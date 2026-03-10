//---------------------------------------------------------
// Script que se encarga del movimiento rectilíneo de la bala
// Samuel Asensio Torres
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente al que se le asigna una velocidad inicial y se encarga de mover el objeto a esta velocidad.
/// Pensado para añadirlo a las balas.
/// </summary>
public class BulletMove : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField]
    private float speed = 15f; // Velocidad de la bala

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
    /// Bool que dice si hay o no GameManager en la escena
    /// </summary>
    private bool _gameManager = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen

    void Start()
    {
        _gameManager = GameManager.HasInstance();
    }
    /// <summary>
    /// Se ejecuta cada frame. Mueve la bala hacia su derecha local (0 grados) 
    /// para coincidir con la rotación del disparo.
    /// </summary>
    void Update()
    {
        if (_gameManager) transform.Translate(Vector2.right * speed * Time.deltaTime * GameManager.Instance.SlowMultiplier);
        else transform.Translate(Vector2.right * speed * Time.deltaTime);
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

} // class BulletMove 
// namespace
