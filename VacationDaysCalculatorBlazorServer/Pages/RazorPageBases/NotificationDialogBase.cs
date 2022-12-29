using Microsoft.AspNetCore.Components;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class NotificationDialogBase : ComponentBase
    {
        public bool ShowDialog { get; set; }

        [Parameter]
        public string Description { get; set; }

        public void Show()
        {
            ShowDialog = true;
            StateHasChanged();
        }

        protected void CloseDialog()
        {
            ShowDialog = false;
            StateHasChanged();
        }
    }
}
