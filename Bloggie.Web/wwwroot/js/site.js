// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
import { Editor } from '@tiptap/core';
import StarterKit from '@tiptap/starter-kit';

new Editor({
    element: document.querySelector('.element'),
    extensions: [StarterKit],
    content: '<p>Hello World!</p>',
    
    
});
