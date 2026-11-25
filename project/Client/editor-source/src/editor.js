import { Editor } from '@tiptap/core'
import StarterKit from '@tiptap/starter-kit'
import Placeholder from '@tiptap/extension-placeholder'
import './style.css'

const statusText = document.querySelector('#status-text')

const editor = new Editor({
  element: document.querySelector('#editor'),
  extensions: [
    StarterKit.configure({
      blockquote: false,
      codeBlock: false,
      heading: { levels: [1, 2, 3] },
    }),
    Placeholder.configure({ placeholder: 'Начните писать заметку...' }),
  ],
  content: { type: 'doc', content: [{ type: 'paragraph' }] },
  onUpdate: ({ editor }) => {
    statusText.textContent = 'Есть несохранённые изменения'
    sendToHost({ type: 'documentChanged', document: editor.getJSON() })
  },
})

function sendToHost(message) {
  const payload = JSON.stringify(message)
  if (typeof window.invokeCSharpAction === 'function') {
    window.invokeCSharpAction(payload)
  }
}

const actions = {
  bold: () => editor.chain().focus().toggleBold().run(),
  italic: () => editor.chain().focus().toggleItalic().run(),
  strike: () => editor.chain().focus().toggleStrike().run(),
  paragraph: () => editor.chain().focus().setParagraph().run(),
  h1: () => editor.chain().focus().toggleHeading({ level: 1 }).run(),
  h2: () => editor.chain().focus().toggleHeading({ level: 2 }).run(),
  h3: () => editor.chain().focus().toggleHeading({ level: 3 }).run(),
  bulletList: () => editor.chain().focus().toggleBulletList().run(),
  orderedList: () => editor.chain().focus().toggleOrderedList().run(),
  undo: () => editor.chain().focus().undo().run(),
  redo: () => editor.chain().focus().redo().run(),
}

document.querySelector('.toolbar').addEventListener('click', (event) => {
  const button = event.target.closest('button[data-action]')
  if (button) actions[button.dataset.action]?.()
})

function setContent(json) {
  const parsed = typeof json === 'string' ? JSON.parse(json) : json
  editor.commands.setContent(parsed)
  statusText.textContent = 'Документ загружен'
}

window.noteEditor = {
  setContent,
  getJSON: () => editor.getJSON(),
  setSaved: () => { statusText.textContent = 'Сохранено' },
}

sendToHost({ type: 'editorReady' })
