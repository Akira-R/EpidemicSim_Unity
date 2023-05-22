using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class VisualFilter : MonoSingleton<VisualFilter>
{
    const string suscolor_code = "#00C8FF";
    const string infcolor_code = "#FF0000";
    const string reccolor_code = "#00FF00";
    const string placecolor_code = "#FFFF00";

    public enum FilterType {
        Susceptible,
        Infectious,
        Recovered,
        Place
    }

    const string c_EmissionKey = "_EMISSION";
    [SerializeField] private Color _emissionColor;

    [SerializeField] private Material _susMat;
    [SerializeField] private Material _infMat;
    [SerializeField] private Material _recMat;
    [SerializeField] private Material _placeMat;

    private List<Material> _mapCompMats = new List<Material>();
    private List<Material> _entityMats = new List<Material>();

    private List<Color> _entityCols = new List<Color>();

    //[Header("Filter")]
    //[SerializeField] private FilterType _selectedFilter;

    [SerializeField]
    private UITransformTween _filterPanel;

    void Start()
    {
        AddEntityMat(_susMat);
        AddEntityMat(_infMat);
        AddEntityMat(_recMat);
        AddEntityMat(_placeMat);

        AddEntityColor(suscolor_code);
        AddEntityColor(infcolor_code);
        AddEntityColor(reccolor_code);
        AddEntityColor(placecolor_code);

        DefaultFilter();
    }

    public void AddEntityMat(Material newMat)
    {
        //newMat.EnableKeyword(c_EmissionKey);
        //newMat.SetColor("_EmissionColor", _emissionColor);
        //newMat.DisableKeyword(c_EmissionKey);
        _entityMats.Add(newMat);
    }

    public void AddEntityColor(string colorCode) 
    {
        Color color;
        ColorUtility.TryParseHtmlString(colorCode, out color);
        _entityCols.Add(color);
    }

    public void AddMapCompMat(Material newMat)
    {
        //newMat.EnableKeyword(c_EmissionKey);
        newMat.SetColor("_EmissionColor", _emissionColor);
        newMat.DisableKeyword(c_EmissionKey);
        _mapCompMats.Add(newMat);
    }

    [Button]
    public void DefaultFilter() 
    {
        if (_filterPanel.GetToggleStatus()) return;

        for (int i = 0; i < _entityMats.Count; i++)
        {
            _entityMats[i].color = _entityCols[i];
        }

        foreach(Material mat in _mapCompMats)
            mat.DisableKeyword(c_EmissionKey);

        EntityManager.Instance.SetDisplayMarker(false);
        EntityManager.Instance.SetDisplayStatus(false);
        EntityManager.Instance.ResetDisplayStatus();
    }

    public void SetFilter(FilterType filter)
    {
        for (int i = 0; i < _entityMats.Count; i++)
        {
            if (i == (int)filter)
            {
                _entityMats[i].color = _entityCols[i];
                //_entityMats[i].DisableKeyword(c_EmissionKey);
                //Debug.Log("Disable filter " + i);
            }
            else
            {
                _entityMats[i].color = _emissionColor;
                //_entityMats[i].EnableKeyword(c_EmissionKey);
                //Debug.Log("Enable filter " + i);
            }
        }

        foreach (Material mat in _mapCompMats)
            mat.EnableKeyword(c_EmissionKey);

        EntityManager.Instance.SetDisplayMarker(filter == FilterType.Place);
        EntityManager.Instance.SetDisplayStatus(filter == FilterType.Place);
    }

    public void SusFilter()
    {
        SetFilter(FilterType.Susceptible);
    }

    public void InfFilter()
    {
        SetFilter(FilterType.Infectious);
    }

    public void RecFilter()
    {
        SetFilter(FilterType.Recovered);
    }

    public void PlaceFilter()
    {
        SetFilter(FilterType.Place);
    }
}
