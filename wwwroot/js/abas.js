/* ============================================================
   abas.js — Navegação entre as abas (Jogadores / Times / Ligas).
   Recebe um callback onTrocar(recurso) chamado ao mudar de aba.
   ============================================================ */

import { $ } from './ui.js';

export function configurarAbas(onTrocar) {
    document.querySelectorAll('.tab').forEach(botao => {
        botao.addEventListener('click', () => {
            // alterna o visual (aba e painel ativos)
            document.querySelectorAll('.tab').forEach(b => b.classList.remove('active'));
            document.querySelectorAll('.painel').forEach(p => p.classList.remove('active'));
            botao.classList.add('active');
            $(`#${botao.dataset.tab}`).classList.add('active');

            // avisa quem chamou para recarregar a lista da aba
            onTrocar(botao.dataset.tab);
        });
    });
}
