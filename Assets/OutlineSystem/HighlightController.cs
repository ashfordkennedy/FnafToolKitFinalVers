using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    [SerializeField] private List<Renderer> _renderers = new List<Renderer>();
    [SerializeField] private bool preselectedRenderers = false;
    private bool _renderersGenerated = false;

    public static Material highlightMaterial;
    

    public void OnEnable()
    {
        if (preselectedRenderers == false && _renderersGenerated == false)
        {
            Renderer renderer;
            this.gameObject.TryGetComponent<Renderer>(out renderer);
            _renderers.Add(renderer);
            _renderersGenerated = true;
        }
    }

    public void EnableHighlight()
    {
        for (int i = 0; i < _renderers.Count; i++)
        {
         //   _renderers[i].materials.
        }
    }

    public void DisableHighlight()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
