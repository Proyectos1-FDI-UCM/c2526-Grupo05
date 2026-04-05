//---------------------------------------------------------
// Este componente espera a que pase una cantidad de tiempo, y cuando lo haga, elimina este GameObject e instancia otro.
// Juan José de Reyna Godoy.
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este componente, tiene un temporizador. Una vez este llegue a la cantidad de tiempo especificada en el editor, 
/// instanciará un objeto que hará daño al colisionar, y se eliminará el objeto actual.
/// </summary>
public class Explode : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Este es el GameObject que se instanciará pasado el tiempo. Hará daño al colisionar con objetos.
    /// </summary>
    [SerializeField]
    private onCollisionDealDamage BoomRange;

    /// <summary>
    /// Es el tiempo que tardará en instanciar el otro objeto y destruir este.
    /// </summary>
    [SerializeField]
    private float BoomTime;
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
    /// Tiempo que ha pasado desde que se activa el componente.
    /// </summary>
    private float _actualTime = 0;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Se llama cada frame. Llevará el temporizador, y cuando termine, hará el proceso de explotar la dinamita.
    /// </summary>
    void Update()
    {
        _actualTime += Time.deltaTime;
        if (_actualTime > BoomTime)
        {
            Instantiate(BoomRange, transform.position, transform.rotation);
            Destroy(this.gameObject);
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

} // class Explode 
// namespace
