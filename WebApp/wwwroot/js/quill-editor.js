// Jag har byggt den med Hans's video. Rich Text Editor 
function initWysiwyg(wysiwygEditorId, wysiwygToolbarId, textareId, content) {
    const textarea = document.querySelector(textareId)
    const quill = new Quill(wysiwygEditorId, {
        modules: {
            syntax: true,
            toolbar: wysiwygToolbarId
        },
        placeholder: 'Type something',
        theme: 'snow'
    })

    if (content)
        quill.root.innerHTML = content;

    quill.on('text-change', () => {
        textarea.value = quill.root.innerHTML;
    });
}