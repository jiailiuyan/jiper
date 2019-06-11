using System.Linq;
using System.Windows.Input;

namespace Ji.JilyKey
{
    public static class InputHelper
    {
        public static bool IsKeysDown(this System.Windows.Input.KeyboardDevice keyboardDevice, params Key[] keys)
        {
            bool isdown = false;

            if (keys.All(key => (keyboardDevice.GetKeyStates(key) & System.Windows.Input.KeyStates.Down) > 0))
            {
                isdown = true;
            }

            return isdown;
        }
    }
}