using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class VisualFilter : MonoSingleton<VisualFilter>
{
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

    [Header("Filter")]
    [SerializeField] private FilterType _selectedFilter;

    void Start()
    {
        AddEntityMat(_susMat);
        AddEntityMat(_infMat);
        AddEntityMat(_recMat);
        AddEntityMat(_placeMat);
    }

    public void AddEntityMat(Material newMat)
    {
        //newMat.EnableKeyword(c_EmissionKey);
        newMat.SetColor("_EmissionColor", _emissionColor);
        newMat.DisableKeyword(c_EmissionKey);
        _entityMats.Add(newMat);
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
        for (int i = 0; i < _entityMats.Count; i++)
        {
            if (i == (int)_selectedFilter)
                _entityMats[i].DisableKeyword(c_EmissionKey);
        }

        foreach(Material mat in _mapCompMats)
            mat.DisableKeyword(c_EmissionKey);
    }

    [Button]
    public void SetFilter()
    {
        for (int i = 0; i < _entityMats.Count; i++)
        { 
            if(i == (int)_selectedFilter)
                _entityMats[i].DisableKeyword(c_EmissionKey);
            else
                _entityMats[i].EnableKeyword(c_EmissionKey);
        }

        foreach (Material mat in _mapCompMats)
            mat.EnableKeyword(c_EmissionKey);
    }
}
