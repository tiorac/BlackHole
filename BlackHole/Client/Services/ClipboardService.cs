using Microsoft.JSInterop;

namespace BlackHole.Client.Services
{
    public class ClipboardService
    {
        public ClipboardService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private readonly IJSRuntime _jsRuntime;

        public ValueTask<string> ReadTextAsync()
        {
            try
            {
                return _jsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Clipboard error: {ex}");
                return new ValueTask<string>(string.Empty);
            }
        }

        public ValueTask WriteTextAsync(string text)
        {
            try
            {
                return _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Clipboard error: {ex}");
                return new ValueTask();
            }
        }
    }
}
