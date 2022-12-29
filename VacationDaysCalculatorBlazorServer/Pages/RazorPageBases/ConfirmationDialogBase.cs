using Microsoft.AspNetCore.Components;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class ConfirmationDialogBase : ComponentBase
    {
        public bool ShowDialog { get; set; }

        public void Show()
        {
            ShowDialog = true;
            StateHasChanged();
        }

        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; }

        [Parameter]
        public string Question { get; set; }

        public async Task OnConfirmationChanged(bool isConfirmed)
        {
            ShowDialog = false;
            await ConfirmationChanged.InvokeAsync(isConfirmed);
        }
    }
}
