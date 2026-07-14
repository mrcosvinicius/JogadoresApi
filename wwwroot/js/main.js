/* ============================================================
   main.js — Ponto de entrada da aplicação.
   Ordem de inicialização:
   1. monta o layout (abas + seções) a partir do config
   2. liga a navegação das abas
   3. carrega as fontes (times/ligas) para os selects
   4. monta os formulários e os cabeçalhos das tabelas
   5. carrega a aba inicial (jogadores)
   ============================================================ */

import { montarLayout } from './layout.js';
import { configurarAbas } from './abas.js';
import { carregarFontes } from './fontes.js';
import { montarCabecalho } from './tabela.js';
import { RECURSOS } from './config.js';
import { listar, montarFormularios } from './acoes.js';

async function iniciar() {
    montarLayout();                          // 1. abas + seções
    configurarAbas((recurso) => listar(recurso)); // 2. navegação

    await carregarFontes();                  // 3. times/ligas

    montarFormularios();                     // 4. formulários
    Object.keys(RECURSOS).forEach(recurso => montarCabecalho(recurso));

    listar('jogadores');                     // 5. aba inicial
}

iniciar();
