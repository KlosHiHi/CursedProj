
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.Services
{
    public static class MessageBoxService
    {
        public static async Task ShowSimpleMessageBox(string message = "", MsgBoxType msgBoxType = MsgBoxType.None, string title = "", ButtonEnum button = ButtonEnum.Ok, Icon icon = Icon.None)
        {
            var msgBox = msgBoxType switch
            {
                MsgBoxType.Info => MessageBoxManager.GetMessageBoxStandard("Информация", message, button, Icon.Info),

                MsgBoxType.Warning => MessageBoxManager.GetMessageBoxStandard("Предупреждение", message, button, Icon.Warning),

                MsgBoxType.Error => MessageBoxManager.GetMessageBoxStandard("Ошибка", message, button, Icon.Error),

                MsgBoxType.Success => MessageBoxManager.GetMessageBoxStandard("Успех", message, button, Icon.Success),

                _ => MessageBoxManager.GetMessageBoxStandard(title, message, button, icon),
            };

            await msgBox.ShowAsync();
        }
    }
    public enum MsgBoxType
    {
        None,
        Info,
        Warning,
        Error,
        Success
    }
}
