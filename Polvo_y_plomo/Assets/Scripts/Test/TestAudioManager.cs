//---------------------------------------------------------
// Script que se añade a un boton para comprobar el AudioManager en el editor.
// Ängel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script de testeo para el AudioManager.
/// Se le asigna un audioClip y preferiblemente se llama a su función pública desde 
/// un botón.
/// Va ciclando la posición en la que se hace sonar el sonido, empezando en la derecha,
/// luego arriba, izquierda y abajo.
/// </summary>
public class TestAudioManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Clip que sonará con el test
    /// </summary>
    [SerializeField]
    private AudioClip TestClip;
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
    /// Variable que ciclará entre 0 y 3 y hará sonar sonidos con la siguiente codificación:
    /// 0 = derecha.
    /// 1 = arriba.
    /// 2 = izquierda.
    /// 3 = abajo.
    /// </summary>
    private int _i = 0;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public void DoTest()
    {
        if (AudioManager.HasInstance())
        {
            switch (_i)
            {
                case 0:
                    AudioManager.Instance.Play(TestClip, 5 * Vector3.right);
                    _i++;
                    break;
                case 1:
                    AudioManager.Instance.Play(TestClip, 5 * Vector3.up);
                    _i++;
                    break;
                case 2:
                    AudioManager.Instance.Play(TestClip, 5 * Vector3.left);
                    _i++;
                    break;
                case 3:
                    AudioManager.Instance.Play(TestClip, 5 * Vector3.down);
                    _i = 0;
                    break;
            }
        }
        else
        {
            Debug.Log("No hay AudioManager Instance");
        }
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class TestAudioManager 
// namespace
