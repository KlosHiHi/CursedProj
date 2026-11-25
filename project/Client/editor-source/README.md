# Tiptap Editor MVP для Avalonia

Локальный WYSIWYG-редактор заметок на Tiptap. После сборки редактор представляет собой **один HTML-файл** со встроенными JS и CSS. Поэтому в рабочем приложении не нужны CDN, интернет, Node.js или npm.

## Возможности MVP

- абзацы;
- заголовки H1–H3;
- жирный, курсив, зачёркнутый текст;
- маркированный и нумерованный списки;
- Undo / Redo;
- передача документа в формате Tiptap JSON;
- установка документа из C#.

Не включены: блоки кода, цитаты, таблицы, загрузка изображений и серверная часть. Графические заметки можно будет добавить отдельным узлом Tiptap, который хранит `drawingId`, а не встраивает файл изображения в текст.

## 1. Сборка редактора

Требуется Node.js LTS и npm только на этапе разработки/сборки.

Откройте терминал в папке `editor-web`:

```bash
npm install
npm run build
```

Результат появится здесь:

```text
editor-web/dist/index.html
```

Это автономная страница: JS и CSS встроены в HTML. В приложение Avalonia нужно копировать именно этот файл.

Для проверки в браузере:

```bash
npm run dev
```

## 2. Подключение к Avalonia

Добавьте WebView:

```bash
dotnet add package Avalonia.Controls.WebView
```

В XAML подключите namespace и разместите `NativeWebView`:

```xml
<Window
    xmlns="https://github.com/avaloniaui"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:controls="clr-namespace:Avalonia.Controls;assembly=Avalonia.Controls.WebView"
    x:Class="MyNotes.Views.NoteEditorView">

    <Grid>
        <controls:NativeWebView x:Name="EditorWebView"
                                NavigationCompleted="EditorWebView_OnNavigationCompleted"
                                WebMessageReceived="EditorWebView_OnWebMessageReceived" />
    </Grid>
</Window>
```

Если ваш пакет/версия Avalonia использует другой XAML namespace для `NativeWebView`, ориентируйтесь на quick start документации Avalonia для установленной версии.

Скопируйте собранный файл `editor-web/dist/index.html` в проект, например:

```text
MyNotes/
  Assets/
    Editor/
      index.html
```

В `.csproj` приложения добавьте:

```xml
<ItemGroup>
  <Content Include="Assets/Editor/index.html">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```

Загрузите страницу из кода окна:

```csharp
using Avalonia.Controls;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyNotes.Views;

public partial class NoteEditorView : Window
{
    private const string EmptyDocument =
        """{"type":"doc","content":[{"type":"paragraph"}]}""";

    public NoteEditorView()
    {
        InitializeComponent();

        var editorPath = Path.Combine(
            AppContext.BaseDirectory,
            "Assets", "Editor", "index.html");

        EditorWebView.Navigate(new Uri(editorPath));
    }

    private async void EditorWebView_OnNavigationCompleted(
        object? sender,
        WebViewNavigationCompletedEventArgs e)
    {
        if (!e.IsSuccess)
            return;

        await SetDocumentAsync(EmptyDocument);
    }

    public async Task SetDocumentAsync(string tiptapJson)
    {
        var jsonLiteral = JsonSerializer.Serialize(tiptapJson);
        await EditorWebView.InvokeScript(
            $"window.noteEditor.setContent({jsonLiteral});");
    }

    public async Task<string?> GetDocumentAsync()
    {
        var result = await EditorWebView.InvokeScript(
            "JSON.stringify(window.noteEditor.getJSON());");

        return result;
    }

    private void EditorWebView_OnWebMessageReceived(
        object? sender,
        WebMessageReceivedEventArgs e)
    {
        var message = e.Body;
        System.Diagnostics.Debug.WriteLine(message);
    }
}
```

### Важное замечание о `InvokeScript`

В документации Avalonia `NativeWebView.InvokeScript(string)` принимает JavaScript и возвращает `Task<string?>`. В зависимости от версии пакета и движка формат результата может отличаться. Проверьте `result` в отладчике. Для двустороннего обмена используйте `WebMessageReceived` для JS → C# и `InvokeScript` для C# → JS.

## 3. Что происходит при изменении текста

Редактор вызывает:

```javascript
window.invokeCSharpAction(JSON.stringify({
  type: 'documentChanged',
  document: editor.getJSON()
}));
```

`NativeWebView.WebMessageReceived` получает строку в `e.Body`. Документ — это JSON-дерево Tiptap/ProseMirror. Его можно сохранять в локальный JSON-файл, а позже — передавать серверу.

## 4. Проверка автономности

После `npm run build` откройте `dist/index.html` в браузере без подключения к сети. Если редактор работает, значит JS/CSS встроены в HTML и внешние ресурсы не используются.

На Windows нужен WebView2 Runtime. На macOS используется системный WKWebView. На Linux `NativeWebView` зависит от поддерживаемого WebKit/WPE runtime и системных библиотек — см. Linux prerequisites в документации Avalonia.

Документация:
- https://docs.avaloniaui.net/docs/app-development/embedding-web-content
- https://docs.avaloniaui.net/controls/web/nativewebview
