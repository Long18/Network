using Long18.Editor.Tools.Editor.SOBrowser;

namespace _0_CubeRunning.Scripts.SO.Input.Editor
{
    public class LocationSOEditor : SOBrowserEditor<LocationSO>
    {
        const string DEFAULT_NAME = "Location";

        public LocationSOEditor()
        {
            this.CreateDataFolder = false;

            this.DefaultStoragePath = "Assets/0_CubeRunning/Data/SceneData/Locations";
        }
    }
}