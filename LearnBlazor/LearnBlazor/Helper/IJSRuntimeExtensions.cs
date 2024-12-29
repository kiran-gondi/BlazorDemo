using Microsoft.JSInterop;

namespace LearnBlazor.Helper
{
    public static class IJSRuntimeExtensions
    {
        public static async Task ToastrSuccess(this IJSRuntime jsRunTime, string message)
        {
            await jsRunTime.InvokeVoidAsync("showToastr", "success", message);
        }

        public static async Task ToastrError(this IJSRuntime jsRunTime, string message)
        {
            await jsRunTime.InvokeVoidAsync("showToastr", "error", message);
        }
    }
}
