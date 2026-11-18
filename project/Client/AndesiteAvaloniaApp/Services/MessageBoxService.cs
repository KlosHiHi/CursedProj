//using MsBox.Avalonia;
//using MsBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.Services
{

    public static class MessageBoxService
    {
        public static async Task ShowInfo() { }
        public static async Task ShowError() { }
        public static async Task ShowSuccess() { }
        //    public static async Task<ButtonResult> ShowInfo(string message, ButtonEnum button = ButtonEnum.Ok, Icon icon = Icon.Info)
        //    {
        //        var msgBox = MessageBoxManager.GetMessageBoxStandard("Предупреждение", message, button, icon);

        //        return await msgBox.ShowAsync();
        //    }

        //    public static async Task<ButtonResult> ShowError(string message, ButtonEnum button = ButtonEnum.Ok, Icon icon = Icon.Error)
        //    {
        //        var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", message, button, icon);

        //        return await msgBox.ShowAsync();
        //    }

        //    public static async Task<ButtonResult> ShowSuccess(string message, ButtonEnum button = ButtonEnum.Ok, Icon icon = Icon.Success)
        //    {
        //        var msgBox = MessageBoxManager.GetMessageBoxStandard("Успех", message, button, icon);

        //        return await msgBox.ShowAsync();
        //    }
    }
}
