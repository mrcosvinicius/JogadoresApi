/* ============================================================
   paginacao.js — Desenha os controles de paginação
   (Anterior / Próxima) e atualiza o número da página.
   Recebe um callback onNavegar() chamado ao trocar de página.
   ============================================================ */

import { estado } from './state.js';
import { $ } from './ui.js';

export function renderizarPaginacao(recurso, quantidade, onNavegar) {
    const container = $(`#tabela-${recurso}`).closest('.card');
    const pag = container.querySelector('.paginacao');
    const paginaAtual = estado.pagina[recurso];

    pag.innerHTML = '';

    const btnAnterior = document.createElement('button');
    btnAnterior.className = 'btn btn-pagina';
    btnAnterior.textContent = 'Anterior';
    btnAnterior.disabled = paginaAtual === 0;
    btnAnterior.onclick = () => { estado.pagina[recurso]--; onNavegar(); };

    const info = document.createElement('span');
    info.className = 'info-pagina';
    info.textContent = `Página ${paginaAtual + 1}`;

    const btnProxima = document.createElement('button');
    btnProxima.className = 'btn btn-pagina';
    btnProxima.textContent = 'Próxima';
    btnProxima.disabled = quantidade < estado.tamanhoPagina;
    btnProxima.onclick = () => { estado.pagina[recurso]++; onNavegar(); };

    pag.appendChild(btnAnterior);
    pag.appendChild(info);
    pag.appendChild(btnProxima);
}
