using UnityEngine;

public class PlayerCharacterInputProcessor : PlayerOperatorProcessor<PlayerCharacterInputProcessor>
{
    private Camera3DController camera3DController;
    private Player3DController PlayerController;
    public PlayerCharacterInputProcessor()
    {
    }
    public void InitializedPlayerController(Player3DController PlayerController)
    {
        this.PlayerController = PlayerController;
        //ÒÆ¶¯WASD×¢²á
        var UpKey = new InputKey(KeyCode.W);
        var DownKey = new InputKey(KeyCode.S);
        var LeftKey = new InputKey(KeyCode.A);
        var RightKey = new InputKey(KeyCode.D);
        InputManager.Instance.RegisterDirectionKeyInfo(Player3D_DefaultValue.UpDownMoveEvent, UpKey, DownKey,false);
        InputManager.Instance.RegisterDirectionKeyInfo(Player3D_DefaultValue.LeftRightMoveEvent, RightKey, LeftKey,false);
        InputManager.Instance.eventManager.AddEventListener<float>(Player3D_DefaultValue.UpDownMoveEvent, (input) => { 
             PlayerController.GetMoveInputVertical(input);
        });
        InputManager.Instance.eventManager.AddEventListener<float>(Player3D_DefaultValue.LeftRightMoveEvent, (input) => { 
             PlayerController.GetMoveInputHorizontal(input);
        });
        //Êó±ê×¢²á
        InputManager.Instance.RegisterMouseKeyInputInfo(Player3D_DefaultValue.MouseXEvent, E_MouseInputType.MouseMove);
        InputManager.Instance.eventManager.AddEventListener<Vector2>(Player3D_DefaultValue.MouseXEvent, (input) => {
            PlayerController.GetMouseInput(input);
        });
    }
    public void InitializedPlayerCamera(Camera3DController camera3DController)
    {
        //Êó±ê×¢²á
        InputManager.Instance.RegisterMouseKeyInputInfo(Player3D_DefaultValue.MouseYEvent, E_MouseInputType.MouseMove);
        InputManager.Instance.eventManager.AddEventListener<Vector2>(Player3D_DefaultValue.MouseYEvent, (input) => {
            camera3DController.GetMouseInput(input);
        });
    }

}
public static class Player3D_DefaultValue
{
    public static DE_InputKey UpDownMoveEvent => DE_InputKey.Get("UpDownMove");
    public static DE_InputKey LeftRightMoveEvent => DE_InputKey.Get("LeftRightMove");
    public static DE_InputKey MouseXEvent => DE_InputKey.Get("MouseXEvent");
    public static DE_InputKey MouseYEvent => DE_InputKey.Get("MouseYEvent");
    static Player3D_DefaultValue()
    {
        var upDownMove = DE_InputKey.Add("UpDownMove");
        var LeftRightMove = DE_InputKey.Add("LeftRightMove");
        var MouseXEvent = DE_InputKey.Add("MouseXEvent");
        var MouseYEvent = DE_InputKey.Add("MouseYEvent");
    }
}