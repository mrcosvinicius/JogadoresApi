/* ============================================================
   state.js — Estado compartilhado da aplicação.
   Como é exportado como objeto, todos os módulos que importam
   enxergam a MESMA referência (mudanças são visíveis em todos).
   ============================================================ */

// Cache das listas usadas nos selects (times e ligas)
export const fontes = { times: [], ligas: [] };

// Estado de paginação e de edição
export const estado = {
    tamanhoPagina: 5,
    pagina: { jogadores: 0, times: 0, ligas: 0 },
    editando: { recurso: null, id: null }
};
