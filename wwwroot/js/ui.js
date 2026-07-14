/* ============================================================
   ui.js — Pequenos utilitários de interface (sem regra de negócio).
   ============================================================ */

// Atalho para document.querySelector
export const $ = (seletor) => document.querySelector(seletor);

// Gera o HTML de uma "etiqueta" (usado nas colunas de lista)
export const tag = (texto) => `<span class="tag">${texto}</span>`;

// Mostra uma notificação temporária no canto da tela
export function toast(mensagem, tipo = 'sucesso') {
    const el = document.createElement('div');
    el.className = `toast-item ${tipo}`;
    el.textContent = mensagem;
    $('#toast').appendChild(el);
    setTimeout(() => el.remove(), 4000);
}
