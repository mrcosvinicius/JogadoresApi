/* ============================================================
   formulario.js — Tudo que envolve o FORMULÁRIO:
   montar os campos, ler os valores digitados, preencher para
   edição e voltar ao estado inicial.
   Não fala com a API: apenas avisa quem o chamou (via handlers).
   ============================================================ */

import { RECURSOS } from './config.js';
import { fontes } from './state.js';
import { $ } from './ui.js';

// Monta os campos do formulário de um recurso e liga os botões
// aos handlers recebidos ({ onSalvar, onCancelar }).
export function montarFormulario(recurso, handlers) {
    const config = RECURSOS[recurso];
    const form = $(`#form-${recurso}`);
    form.innerHTML = '';

    config.campos.forEach(campo => {
        form.appendChild(criarCampo(recurso, campo));
    });

    form.appendChild(criarBotoes(recurso, handlers));
    form.onsubmit = (e) => handlers.onSalvar(recurso, e);
}

// Cria um campo (label + input/select) conforme a configuração
function criarCampo(recurso, campo) {
    const div = document.createElement('div');
    div.className = 'campo';

    const label = document.createElement('label');
    label.textContent = campo.label;
    label.htmlFor = `in-${recurso}-${campo.nome}`;
    div.appendChild(label);

    let input;
    if (campo.tipo === 'select' || campo.tipo === 'multiselect') {
        input = document.createElement('select');
        if (campo.tipo === 'multiselect') input.multiple = true;
        if (campo.opcional) {
            const opVazia = document.createElement('option');
            opVazia.value = '';
            opVazia.textContent = '— nenhum —';
            input.appendChild(opVazia);
        }
        fontes[campo.fonte].forEach(item => {
            const op = document.createElement('option');
            op.value = item[campo.valor];
            op.textContent = item[campo.texto];
            input.appendChild(op);
        });
    } else {
        input = document.createElement('input');
        input.type = campo.tipo;
        if (campo.tipo === 'number') input.min = '0';
    }

    input.id = `in-${recurso}-${campo.nome}`;
    input.name = campo.nome;
    if (campo.obrigatorio) input.required = true;
    div.appendChild(input);
    return div;
}

// Cria a linha de botões (Salvar/Atualizar + Cancelar)
function criarBotoes(recurso, handlers) {
    const acoes = document.createElement('div');
    acoes.className = 'acoes-form';

    const btnSalvar = document.createElement('button');
    btnSalvar.type = 'submit';
    btnSalvar.className = 'btn btn-primario';
    btnSalvar.id = `btn-salvar-${recurso}`;
    btnSalvar.textContent = '💾 Salvar';
    acoes.appendChild(btnSalvar);

    const btnCancelar = document.createElement('button');
    btnCancelar.type = 'button';
    btnCancelar.className = 'btn btn-cancelar hidden';
    btnCancelar.id = `btn-cancelar-${recurso}`;
    btnCancelar.textContent = '✖️ Cancelar';
    btnCancelar.onclick = () => handlers.onCancelar(recurso);
    acoes.appendChild(btnCancelar);

    return acoes;
}

// Lê os valores atuais do formulário e devolve o objeto pronto para a API
export function lerFormulario(recurso, form) {
    const corpo = {};
    RECURSOS[recurso].campos.forEach(campo => {
        const input = form.elements[campo.nome];
        if (campo.tipo === 'multiselect') {
            corpo[campo.nome] = Array.from(input.selectedOptions).map(o => Number(o.value));
        } else if (campo.tipo === 'number' || campo.tipo === 'select') {
            corpo[campo.nome] = input.value === '' ? null : Number(input.value);
        } else {
            corpo[campo.nome] = input.value.trim();
        }
    });
    return corpo;
}

// Preenche o formulário com os dados de um item (modo edição)
export function preencherParaEdicao(recurso, item) {
    const form = $(`#form-${recurso}`);
    const valores = RECURSOS[recurso].valoresParaEdicao(item);

    RECURSOS[recurso].campos.forEach(campo => {
        const input = form.elements[campo.nome];
        const valor = valores[campo.nome];
        if (campo.tipo === 'multiselect') {
            const selecionados = new Set((valor || []).map(Number));
            Array.from(input.options).forEach(op => { op.selected = selecionados.has(Number(op.value)); });
        } else {
            input.value = valor ?? '';
        }
    });

    $(`#btn-salvar-${recurso}`).textContent = '✏️ Atualizar';
    $(`#btn-cancelar-${recurso}`).classList.remove('hidden');
    form.scrollIntoView({ behavior: 'smooth', block: 'center' });
    form.elements[RECURSOS[recurso].campos[0].nome].focus();
}

// Limpa o formulário e volta ao modo "criação"
export function limparFormulario(recurso) {
    const form = $(`#form-${recurso}`);
    form.reset();
    $(`#btn-salvar-${recurso}`).textContent = '💾 Salvar';
    $(`#btn-cancelar-${recurso}`).classList.add('hidden');
}
