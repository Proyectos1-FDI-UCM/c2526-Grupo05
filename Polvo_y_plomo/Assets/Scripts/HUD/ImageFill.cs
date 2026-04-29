//---------------------------------------------------------
// Script que controla el porcentaje de llenado de una imagen tipo Filled.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script al que se le puede asignar desde fuera un porcentaje y que
/// se encarga de aplicarlo al fillAmmount de una imagen tipo Filled.
/// </summary>
public class ImageFill : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Imagen que deberá ser de tipo Filled y cuyo parámetro fillAmount
    /// será modificado.
    /// </summary>
    [SerializeField]
    private Image ImageFilled;
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
    /// Se llama una vez al cargarse en escena.
    /// Hace comprobaciones necesarias para el componente.
    /// </summary>
    private void Awake()
    {
        if (ImageFilled == null)
        {
            Debug.Log("No se ha asignado imagen en el componente \"ImageFill\", no funcionará");
            Destroy(this);
        }
        if (ImageFilled.type != Image.Type.Filled)
        {
            Debug.Log("Se ha asignado en el componente \"ImageFill\" una imagen que no es del tipo Filled. No funcionará");
            Destroy(this);
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
    /// Actualiza el parámetro fillAmmount de la imagen.
    /// </summary>
    /// <param name="fillAmount"></param>
    public void UpdateImageFillAmmount(float fillAmount)
    {
        ImageFilled.fillAmount = fillAmount;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class ImageFill 
// namespace
