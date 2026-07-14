/* ============================================================
   acoes.js — ORQUESTRAÇÃO do CRUD.
   É o "cérebro" que costura os módulos: chama a API, atualiza o
   estado e manda a UI (formulário, tabela, paginação) se redesenhar.
   Os módulos de UI não se importam entre si — só este os conecta.
   ============================================================ */

import { RECURSOS } from './config.js';
import { estado } from './state.js';
import { $, toast } from './ui.js';
import { api } from './api.js';
import { carregarFontes } from './fontes.js';
import { montarFormulario, lerFormulario, preencherParaEdicao, limparFormulario } from './formulario.js';
import { renderizarLinhas } from './tabela.js';
import { renderizarPaginacao } from './paginacao.js';

// Handlers compartilhados do formulário (criar/editar e cancelar)
const handlersFormulario = { onSalvar: salvar, onCancelar: sairModoEdicao };

// Handlers compartilhados das linhas da tabela (editar e excluir)
const handlersLinha = { onEditar: entrarModoEdicao, onExcluir: deletar };

// Monta os formulários de todos os recursos (usado na inicialização)
export function montarFormularios() {
    Object.keys(RECURSOS).forEach(recurso => montarFormulario(recurso, handlersFormulario));
}

// Remonta só os forms que têm <select> dependente (jogadores e times),
// para refletir times/ligas recém-criados ou excluídos.
function remontarFormsComSelects() {
    montarFormulario('jogadores', handlersFormulario);
    montarFormulario('times', handlersFormulario);
}

// ---------- LISTAR (paginado) ----------
export async function listar(recurso) {
    const config = RECURSOS[recurso];
    const pula = estado.pagina[recurso] * estado.tamanhoPagina;

    try {
        const itens = await api(`${config.endpoint}?pula=${pula}&pega=${estado.tamanhoPagina}`);
        renderizarLinhas(recurso, itens, handlersLinha);
        renderizarPaginacao(recurso, itens ? itens.length : 0, () => listar(recurso));
    } catch (erro) {
        toast(erro.message, 'erro');
    }
}

// ---------- SALVAR (criar ou atualizar) ----------
export async function salvar(recurso, evento) {
    evento.preventDefault();
    const config = RECURSOS[recurso];
    const form = evento.target;
    const corpo = lerFormulario(recurso, form);
    const emEdicao = estado.editando.recurso === recurso;

    try {
        if (emEdicao) {
            await api(`${config.endpoint}/${estado.editando.id}`, { method: 'PUT', body: JSON.stringify(corpo) });
            toast(`${config.rotulo} atualizado com sucesso!`);
            sairModoEdicao(recurso);
        } else {
            await api(config.endpoint, { method: 'POST', body: JSON.stringify(corpo) });
            toast(`${config.rotulo} criado com sucesso!`);
            form.reset();
        }
        await recarregar();
    } catch (erro) {
        toast(erro.message, 'erro');
    }
}

// ---------- EDIÇÃO ----------
export function entrarModoEdicao(recurso, item) {
    estado.editando = { recurso, id: item.id };
    preencherParaEdicao(recurso, item);
}

export function sairModoEdicao(recurso) {
    estado.editando = { recurso: null, id: null };
    limparFormulario(recurso);
}

// ---------- EXCLUIR ----------
export async function deletar(recurso, id) {
    if (!confirm('Tem certeza que deseja excluir este registro?')) return;
    try {
        await api(`${RECURSOS[recurso].endpoint}/${id}`, { method: 'DELETE' });
        toast('Registro excluído.');
        if (estado.editando.recurso === recurso && estado.editando.id === id) {
            sairModoEdicao(recurso);
        }
        await recarregar();
    } catch (erro) {
        toast(erro.message, 'erro');
    }
}

// Recarrega fontes + selects + a lista da aba visível
async function recarregar() {
    await carregarFontes();
    remontarFormsComSelects();
    atualizarTudo();
}

// Lista apenas a aba atualmente visível
export function atualizarTudo() {
    const abaAtiva = $('.painel.active');
    if (abaAtiva) listar(abaAtiva.id);
}
