/* ============================================================
   config.js — "Mapa" declarativo de cada recurso.
   Aqui definimos, para cada recurso: o endpoint, os campos do
   formulário, as colunas da tabela e como preencher o form na edição.
   Nenhuma lógica de DOM ou rede fica aqui — é só configuração.
   ============================================================ */

import { tag } from './ui.js';

export const RECURSOS = {
    jogadores: {
        endpoint: '/jogador',
        rotulo: 'Jogador',
        rotuloPlural: 'Jogadores',
        icone: '👤',
        campos: [
            { nome: 'nome',     label: 'Nome',     tipo: 'text',   obrigatorio: true },
            { nome: 'posicao',  label: 'Posição',  tipo: 'text',   obrigatorio: true },
            { nome: 'gols',     label: 'Gols',     tipo: 'number', obrigatorio: true },
            { nome: 'timeId',   label: 'Time',     tipo: 'select', fonte: 'times', texto: 'nome', valor: 'id', obrigatorio: true }
        ],
        colunas: [
            { chave: 'id',      label: 'ID' },
            { chave: 'nome',    label: 'Nome' },
            { chave: 'posicao', label: 'Posição' },
            { chave: 'gols',    label: 'Gols' },
            { chave: 'time',    label: 'Time' }
        ],
        // mapeia o objeto lido (ReadDto) para os valores do formulário (BaseDto)
        valoresParaEdicao: item => ({
            nome: item.nome, posicao: item.posicao, gols: item.gols, timeId: item.timeId ?? ''
        })
    },
    times: {
        endpoint: '/time',
        rotulo: 'Time',
        rotuloPlural: 'Times',
        icone: '🛡️',
        campos: [
            { nome: 'nome',     label: 'Nome',     tipo: 'text', obrigatorio: true },
            { nome: 'estadio',  label: 'Estádio',  tipo: 'text', obrigatorio: true },
            { nome: 'ligasIds', label: 'Ligas (segure Ctrl p/ várias)', tipo: 'multiselect', fonte: 'ligas', texto: 'nome', valor: 'id' }
        ],
        colunas: [
            { chave: 'id',      label: 'ID' },
            { chave: 'nome',    label: 'Nome' },
            { chave: 'estadio', label: 'Estádio' },
            { chave: 'ligas',   label: 'Ligas',     render: v => v.map(x => tag(x.nome)).join('') },
            { chave: 'elenco',  label: 'Jogadores', render: v => v.length }
        ],
        valoresParaEdicao: item => ({
            nome: item.nome, estadio: item.estadio, ligasIds: item.ligas.map(l => l.id)
        })
    },
    ligas: {
        endpoint: '/liga',
        rotulo: 'Liga',
        rotuloPlural: 'Ligas',
        icone: '🏆',
        campos: [
            { nome: 'nome', label: 'Nome', tipo: 'text', obrigatorio: true }
        ],
        colunas: [
            { chave: 'id',    label: 'ID' },
            { chave: 'nome',  label: 'Nome' },
            { chave: 'times', label: 'Times', render: v => v.map(x => tag(x.nome)).join('') }
        ],
        valoresParaEdicao: item => ({ nome: item.nome })
    }
};
