using System.Runtime.InteropServices;
using AudioDeviceCmdlets.CoreAudioApi.Enums;
using AudioDeviceCmdlets.CoreAudioApi.Interfaces;

namespace AudioDeviceCmdlets.CoreAudioApi
{
    public class PolicyConfigClient
    {
        private readonly IPolicyConfig _PolicyConfig;
        private readonly IPolicyConfigVista _PolicyConfigVista;
        private readonly IPolicyConfig10 _PolicyConfig10;

        public PolicyConfigClient()
        {
            _PolicyConfig = new PolicyConfigClientCom() as IPolicyConfig;
            if (_PolicyConfig != null)
                return;

            _PolicyConfigVista = new PolicyConfigClientCom() as IPolicyConfigVista;
            if (_PolicyConfigVista != null)
                return;

            _PolicyConfig10 = new PolicyConfigClientCom() as IPolicyConfig10;
        }

        public void SetDefaultEndpoint(string devID, DeviceRoles eRole)
        {
            if (_PolicyConfig != null)
            {
                Marshal.ThrowExceptionForHR(_PolicyConfig.SetDefaultEndpoint(devID, eRole));
                return;
            }
            if (_PolicyConfigVista != null)
            {
                Marshal.ThrowExceptionForHR(_PolicyConfigVista.SetDefaultEndpoint(devID, eRole));
                return;
            }
            if (_PolicyConfig10 != null)
            {
                Marshal.ThrowExceptionForHR(_PolicyConfig10.SetDefaultEndpoint(devID, eRole));
            }
        }
    }
}