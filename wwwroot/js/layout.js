/* ============================================================
   layout.js — Monta a ESTRUTURA da página (o "esqueleto dinâmico").
   As abas e as seções de cada recurso são geradas a partir do
   config (RECURSOS), eliminando a repetição que existia no HTML.
   Deve rodar ANTES de montar formulários e tabelas.
   ============================================================ */

import { RECURSOS } from './config.js';
import { $ } from './ui.js';

export function montarLayout() {
    montarAbas();
    montarSecoes();
}

// Cria os botões de aba na <nav id="tabs">
function montarAbas() {
    const nav = $('#tabs');
    Object.entries(RECURSOS).forEach(([recurso, config], indice) => {
        const botao = document.createElement('button');
        botao.className = 'tab' + (indice === 0 ? ' active' : '');
        botao.dataset.tab = recurso;
        botao.textContent = `${config.icone} ${config.rotuloPlural}`;
        nav.appendChild(botao);
    });
}

// Cria uma <section> por recurso, cada uma com o card do form
// e o card da lista (a primeira começa visível)
function montarSecoes() {
    const main = $('#conteudo');
    Object.entries(RECURSOS).forEach(([recurso, config], indice) => {
        const section = document.createElement('section');
        section.id = recurso;
        section.className = 'painel' + (indice === 0 ? ' active' : '');
        section.appendChild(criarCardFormulario(recurso, config));
        section.appendChild(criarCardLista(recurso, config));
        main.appendChild(section);
    });
}

function criarCardFormulario(recurso, config) {
    const card = document.createElement('div');
    card.className = 'card';

    const titulo = document.createElement('h2');
    titulo.textContent = `➕ Cadastrar ${config.rotulo}`;

    const form = document.createElement('form');
    form.id = `form-${recurso}`;
    form.noValidate = true;

    card.appendChild(titulo);
    card.appendChild(form);
    return card;
}

function criarCardLista(recurso, config) {
    const card = document.createElement('div');
    card.className = 'card';

    const titulo = document.createElement('h2');
    titulo.textContent = `📋 Lista de ${config.rotuloPlural}`;

    const container = document.createElement('div');
    container.className = 'tabela-container';

    const tabela = document.createElement('table');
    tabela.id = `tabela-${recurso}`;
    container.appendChild(tabela);

    card.appendChild(titulo);
    card.appendChild(container);
    return card;
}
