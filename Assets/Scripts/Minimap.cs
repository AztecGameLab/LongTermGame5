using UnityEngine;
using UnityEngine.UI;

public class Minimap : Singleton<Minimap>
{
    public enum Areas {Air, Lava, Bog, Water, Boss }
    
    [SerializeField] private Image air;
    [SerializeField] private Image bog;
    [SerializeField] private Image lava;
    [SerializeField] private Image water;
    [SerializeField] private Image boss;
    [SerializeField] private Image player;
    
    [SerializeField] private float scaleFactor = 0.6222f;
    [SerializeField] private Vector2 worldOrigin = new Vector2(-37.5f, 4.7f); 
    
    private PlatformerController _player;

    private void Update()
    {
        if (_player == null)
            return;

        var playerPosition = _player.transform.position;

        var xPos = -scaleFactor * playerPosition.x - worldOrigin.x;
        var yPos = -scaleFactor * playerPosition.y - worldOrigin.y;
        
        transform.localPosition = new Vector3(xPos, yPos);
    }

    public void EnableMinimap(PlatformerController player)
    {
        _player = player;
        
        this.player.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void DisableMinimap()
    {
        player.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    
    public void UnlockArea(Areas area)
    {
        switch (area)
        {
            case Areas.Air:
                air.gameObject.SetActive(true);
                break;
            case Areas.Water:
                water.gameObject.SetActive(true);
                break;
            case Areas.Lava:
                lava.gameObject.SetActive(true);
                break;
            case Areas.Bog:
                bog.gameObject.SetActive(true);
                break;
            case Areas.Boss:
                boss.gameObject.SetActive(true);
                break;
        }
    }
}