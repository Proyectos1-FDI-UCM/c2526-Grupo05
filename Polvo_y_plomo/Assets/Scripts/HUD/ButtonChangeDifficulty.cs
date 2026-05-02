//---------------------------------------------------------
// Script simple para llamar a los métodos de incremento y decremento de dificultad del DificultyManager
// Responsable de la creación de este archivo
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este script sirve para cambiar las dificultades del juego con funciones públicas (llamando a botones).
/// Se le ha de asignar un ChangeDifficultyText configurado para funcionar bien.
/// </summary>
public class ButtonChangeDifficulty : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Script ChangeDifficultyText que tendrá que ser asignado.
    /// </summary>
    [SerializeField]
    private ChangeDifficultyText changeText;
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

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Método público para incrementar la dificultad
    /// </summary>
    public void IncreaseDifficulty()
    {
        if (DifficultyManager.HasInstance())
        {
            DifficultyManager.Instance.IncreaseDifficulty();
            if (changeText != null) changeText.UpdateDifficultyText();
            else Debug.Log("ChangeText no asignado al ButtonChangeDifficulty, no se verá en la interfaz el cambio");
        }
        else Debug.Log("Se ha intentado llamar a IncreaseDifficulty desde un boton en una escena sin DifficultyManager");
    }

    /// <summary>
    /// Método público para bajar la dificultad
    /// </summary>
    public void DecreaseDifficulty()
    {
        if (DifficultyManager.HasInstance())
        {
            DifficultyManager.Instance.DecreaseDifficulty();
            if (changeText != null) changeText.UpdateDifficultyText();
            else Debug.Log("ChangeText no asignado al ButtonChangeDifficulty, no se verá en la interfaz el cambio");
        }
        else Debug.Log("Se ha intentado llamar a DecreaseDifficulty desde un boton en una escena sin DifficultyManager");
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class ButtonChangeDifficulty 
// namespace
