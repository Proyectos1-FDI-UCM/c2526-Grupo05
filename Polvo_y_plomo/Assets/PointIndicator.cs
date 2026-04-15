//---------------------------------------------------------
// Este script se encarga de proporcionar el feedback de los puntos que te da cada enemigo, indicando cuantos te da encima del enemigo al que matas
// Miguel Gómez García
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PointIndicator : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField]
    TextMeshProUGUI pointText;

    Vector3 position;

    [SerializeField]
    float DisappearSpeed = 1f;

    [SerializeField]
    float MoveSpeed = 1f;

    [SerializeField]
    float Subida = 0f;

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
    /// Le vamos quitando la transparecia constantemente para que acabe desapareciendo
    /// También le indicaremos la velocidad a la que sube para darle una sensación más fluida
    /// Por último le obligamos a permanecer quiero en el lugar en el que se ha generado primeramente
    /// </summary>
    void Update()
    {
        pointText.alpha -= DisappearSpeed * Time.deltaTime;

        position += Vector3.up * MoveSpeed * Time.deltaTime;

        transform.position = Camera.main.WorldToScreenPoint(position);

        if (pointText.alpha <= 0) gameObject.SetActive(false);
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
    /// Script que hace aparecer los puntos dados encima de una posicion que pasaremos para que se vea en el mismo lugar que en el HUD
    /// </summary>
    public void SpawnHere(Vector3 _position, int points)
    {
        position = _position + new Vector3(0, Subida, 0);
        pointText.text = points.ToString();

        pointText.alpha = 1.0f;


        transform.position = Camera.main.WorldToScreenPoint(position);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class PointIndicator 
// namespace
