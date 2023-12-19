using Microsoft.AspNetCore.Components;

namespace BlackHole.Client.Services
{
    public class MainPageService
    {
        private string _title = string.Empty;
        public string Title 
        { 
            get => _title;
            set
            {
                _title = value;
                MainPageChanged?.Invoke();
            }
        }

        private RenderFragment? _toolsButtons;
        public RenderFragment? ToolsButtons
        {
            get => _toolsButtons;
            set
            {
                _toolsButtons = value;
                MainPageChanged?.Invoke();
            }
        }

        public Action? MainPageChanged { get; set; }
    }
}
