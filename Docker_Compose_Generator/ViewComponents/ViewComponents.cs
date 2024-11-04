using Microsoft.AspNetCore.Mvc;
using Docker_Compose_Generator.Models;
using System.Collections.Generic;
using Docker_Compose_Generator.Models;

namespace Docker_Compose_Generator.ViewComponents
{
    public class NetworkListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<NetworkDTO> networks)
        {
            return View("_NetworkListPartial", networks);
        }

        public class ServiceListViewComponent : ViewComponent
        {
            public IViewComponentResult Invoke(List<ServiceDto> services)
            {
                return View("_ServiceListPartial", services);
            }
        }

        public class VolumeListViewComponent : ViewComponent
        {
            public IViewComponentResult Invoke(List<VolumeDTO> volumes)
            {
                return View("_VolumeListPartial", volumes);
            }
        }
    }
}
