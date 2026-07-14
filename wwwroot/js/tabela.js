/* ============================================================
   tabela.js — Tudo que envolve a TABELA de listagem:
   montar o cabeçalho e desenhar as linhas.
   Não fala com a API: recebe os itens e os handlers de ação.
   ============================================================ */

import { RECURSOS } from './config.js';
import { $ } from './ui.js';

// Cria o <thead> (colunas + "Ações") e o <tbody> vazio.
// Também garante o container de paginação logo após a tabela.
export function montarCabecalho(recurso) {
    const tabela = $(`#tabela-${recurso}`);
    const thead = document.createElement('thead');
    const tr = document.createElement('tr');

    RECURSOS[recurso].colunas.forEach(col => {
        const th = document.createElement('th');
        th.textContent = col.label;
        tr.appendChild(th);
    });

    const thAcao = document.createElement('th');
    thAcao.textContent = 'Ações';
    tr.appendChild(thAcao);
    thead.appendChild(tr);

    tabela.innerHTML = '';
    tabela.appendChild(thead);
    tabela.appendChild(document.createElement('tbody'));

    // container de paginação (se ainda não existir)
    const container = tabela.closest('.tabela-container');
    if (!container.parentElement.querySelector('.paginacao')) {
        const pag = document.createElement('div');
        pag.className = 'paginacao';
        container.insertAdjacentElement('afterend', pag);
    }
}

// Desenha as linhas da tabela para os itens recebidos.
// handlers = { onEditar(recurso, item), onExcluir(recurso, id) }
export function renderizarLinhas(recurso, itens, handlers) {
    const config = RECURSOS[recurso];
    const tbody = $(`#tabela-${recurso} tbody`);
    tbody.innerHTML = '';

    if (!itens || itens.length === 0) {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td colspan="${config.colunas.length + 1}" class="vazio">Nenhum registro nesta página.</td>`;
        tbody.appendChild(tr);
        return;
    }

    itens.forEach(item => {
        const tr = document.createElement('tr');

        config.colunas.forEach(col => {
            const td = document.createElement('td');
            const valor = item[col.chave];
            td.innerHTML = col.render ? col.render(valor ?? []) : (valor ?? '—');
            tr.appendChild(td);
        });

        tr.appendChild(criarCelulaAcoes(recurso, item, handlers));
        tbody.appendChild(tr);
    });
}

// Cria a célula com os botões Editar e Excluir
function criarCelulaAcoes(recurso, item, handlers) {
    const td = document.createElement('td');
    td.className = 'acoes-celula';

    const btnEdit = document.createElement('button');
    btnEdit.className = 'btn btn-editar';
    btnEdit.textContent = 'Editar';
    btnEdit.onclick = () => handlers.onEditar(recurso, item);
    td.appendChild(btnEdit);

    const btnDel = document.createElement('button');
    btnDel.className = 'btn btn-excluir';
    btnDel.textContent = 'Excluir';
    btnDel.onclick = () => handlers.onExcluir(recurso, item.id);
    td.appendChild(btnDel);

    return td;
}
