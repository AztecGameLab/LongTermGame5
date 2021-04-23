using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Integrate minimap unlock data with SaveSystem
public class Minimap : Singleton<Minimap>
{
    [Serializable]
    public class MinimapArea
    {
        public Image image;
        public Area area;
        
        public void Unlock()
        {
            image.gameObject.SetActive(true);
        }
    }
    
    public enum Area { Air, Lava, Bog, Water, Boss }

    [Header("Minimap Settings")]
    [SerializeField] private MinimapArea[] minimapAreas;
    [SerializeField] private Image playerMarker;
    [SerializeField] private float scaleFactor = 0.6222f;
    [SerializeField] private Vector2 worldOrigin = new Vector2(-37.5f, 4.7f);

    private bool _isEnabled = false;
    private PlatformerController _player;
    private readonly Dictionary<Area, MinimapArea> _mappedAreas = new Dictionary<Area, MinimapArea>();

    private void OnValidate()
    {
        foreach (var minimapArea in minimapAreas)
            _mappedAreas.Add(minimapArea.area, minimapArea);
    }

    private void Update()
    {
        if (_isEnabled)
            UpdateMinimapPosition();
    }

    private void UpdateMinimapPosition()
    {
        var playerPosition = _player.transform.position;

        var xPos = -scaleFactor * playerPosition.x - worldOrigin.x;
        var yPos = -scaleFactor * playerPosition.y - worldOrigin.y;
        
        transform.localPosition = new Vector3(xPos, yPos);
    }

    public void EnableMinimap(PlatformerController player)
    {
        _player = player;
        _isEnabled = true;
        playerMarker.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void DisableMinimap()
    {
        RemovePlayerBinding();
        playerMarker.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    // Useful for when the player is unloaded, to remove null pointer errors
    public void RemovePlayerBinding()
    {
        _player = null;
        _isEnabled = false;
    }
    
    public void UnlockArea(params Area[] areas)
    {
        foreach (var area in areas)
            _mappedAreas[area].Unlock();    
    }

    // Useful shortcut for debugging, might not be relevant to gameplay
    public void UnlockAllAreas()
    {
        foreach (var minimapArea in minimapAreas)
            minimapArea.Unlock();
    }
}