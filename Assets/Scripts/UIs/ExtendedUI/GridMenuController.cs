using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMenuController : MonoBehaviour
{
    [SerializeField] Transform gridMenu = null;

    [SerializeField] DecorObjectCatalogue objectCatalogue;

    [SerializeField] int GridSize = 10;
    [SerializeField] int currentPage = 0;
    [SerializeField] int MaximumPage = 1;



    public void OnEnable()
    {
        MaximumPage = GetPageTotal();
    }
    public int GetPageTotal()
    {
       return objectCatalogue.MapObjects.Count / GridSize;
    }

    public void ButtonUpdate()
    {

    }

    public int GetStartIndex()
    {

        return 0;
    }

    public void PreviousMenu()
    {
        currentPage = (currentPage - 1 > 0) ? currentPage - 1 : currentPage = MaximumPage;

    }

    public void NextMenu()
    {
        currentPage = (currentPage + 1 < MaximumPage) ? currentPage + 1 : currentPage = 0;

    }
}
