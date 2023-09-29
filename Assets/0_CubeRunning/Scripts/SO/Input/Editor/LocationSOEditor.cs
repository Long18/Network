
using Long18.Tools;

namespace _0_CubeRunning.Scripts.SO.Input.Editor
{
    public class LocationSOEditor : BrowserSOEditor<LocationSO>
    {
        const string DEFAULT_NAME = "Location";

        public LocationSOEditor()
        {
            this.CreateDataFolder = false;

            this.DefaultStoragePath = "Assets/0_CubeRunning/Data/SceneData/Locations";
        }
    }
}