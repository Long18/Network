using WilliamEditor.Tools.SOBrowser;

namespace _0_CubeRunning.Scripts.SO.Input.Editor
{
    public class LocationSOEditor : SOBrowserEditor<LocationSO>
    {
        public LocationSOEditor()
        {
            this.CreateDataFolder = false;

            this.DefaultStoragePath = "Assets/0_CubeRunning/Data/SceneData/Locations";
        }
    }
}