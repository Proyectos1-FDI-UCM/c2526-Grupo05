//---------------------------------------------------------
// Maneja la transparencia de un collider puesto en la cámara para ocultar elementos del HUD al haber enemigos detras de ellos
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente añadido a ciertos colliders de la cámara para iniciar transparencias de elementos del HUD.
/// </summary>
public class HUDTransparencyController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Struct que almacena el FadeIn y FadeOut de un elemento de HUD que debe ocultarse / enseñarse al tener enemigos detrás.
    /// </summary>
    [System.Serializable]
    public struct HUD_Fades
    {
        /// <summary>
        /// FadeColor con un FadeIn configurado
        /// </summary>
        public FadeColor FadeIn;

        /// <summary>
        /// FadeColor con un FadeOut configurado
        /// </summary>
        public FadeColor FadeOut;
    }

    /// <summary>
    /// Array de elementos del HUD que se ocultaran al tener enemigos o elementos importantes detrás de ellos
    /// </summary>
    [SerializeField]
    private HUD_Fades[] ElementosDelHUD;

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
    /// Determina si el elemento esta transparentado o no
    /// </summary>
    private bool _fadedOut = false;

    /// <summary>
    /// Lleva la cuenta de los elementos que han entrado para asegurarse de la transparencia no se solidifica 
    /// mientras aun quedan elementos dentro.
    /// </summary>
    private int _objectsInside = 0;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Al entrar en colision realiza los FadeOuts de los elementos configurados si no se habian hecho ya.
    /// Se asegura de que no haya conflictos entre FadeIn y FadeOut,
    /// El objeto debe estar en la layer HUDTransparency, y esta layer ha de estar configurada para chocar solo con los elementos que deban hacer que el HUD se transparente.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _objectsInside++;

        // Si aun no esta transparentado
        if (!_fadedOut)
        {
            _fadedOut = true;

            foreach (HUD_Fades fade in ElementosDelHUD)
            {
                // Apagar los FadeIn en caso de que estuvieran activos para evitar que haya 2 elementos sobreescribiendose
                if (fade.FadeIn != null) fade.FadeIn.enabled = false;

                // Realizar un FadeOut de cada elemento para transparentarlos, asegurandose de que el alpha actual no se cambie bruscamente.
                if (fade.FadeOut != null)
                {
                    fade.FadeOut.SetStartAlpha(fade.FadeOut.GetCurrentAlpha());
                    fade.FadeOut.enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// Al entrar en colision realiza los FadeIns de los elementos configurados si no se habian hecho ya.
    /// Se asegura de que no haya conflictos entre FadeIn y FadeOut,
    /// El objeto debe estar en la layer HUDTransparency, y esta layer ha de estar configurada para chocar solo con los elementos que deban hacer que el HUD se transparente.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        _objectsInside--;
        // Si aun no esta solido
        if (_fadedOut && _objectsInside <= 0)
        {
            _fadedOut = false;

            foreach (HUD_Fades fade in ElementosDelHUD)
            {
                // Apagar los FadeOut en caso de que estuvieran activos para evitar que haya 2 elementos sobreescribiendose
                if (fade.FadeOut != null) fade.FadeOut.enabled = false;

                // Realizar un FadeIn de cada elemento para transparentarlos, asegurandose de que el alpha actual no se cambie bruscamente.
                if (fade.FadeIn != null)
                {
                    fade.FadeIn.SetStartAlpha(fade.FadeIn.GetCurrentAlpha());
                    fade.FadeIn.enabled = true;
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

} // class HUDTransparencyController 
// namespace
