using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Showcase.Blazor.WebServer
{
    public sealed class RenderModeInteractiveServer : RenderModeAttribute  
    {
        public override IComponentRenderMode Mode => (IComponentRenderMode)RenderMode.InteractiveServer;
    }
}
