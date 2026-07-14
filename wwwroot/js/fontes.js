/* ============================================================
   fontes.js — Carrega as listas usadas nos <select> dos forms
   (times e ligas) e guarda no cache compartilhado (state.fontes).
   ============================================================ */

import { api } from './api.js';
import { fontes } from './state.js';
import { toast } from './ui.js';

export async function carregarFontes() {
    try {
        fontes.times = await api('/time?pega=100');
        fontes.ligas = await api('/liga?pega=100');
    } catch (erro) {
        toast('Falha ao carregar times/ligas: ' + erro.message, 'erro');
    }
}
