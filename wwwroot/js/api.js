/* ============================================================
   api.js — Comunicação HTTP com a API.
   Centraliza o fetch e o tratamento de erros (inclui as
   mensagens de validação retornadas pelo [ApiController]).
   ============================================================ */

export async function api(caminho, opcoes = {}) {
    const resp = await fetch(caminho, {
        headers: { 'Content-Type': 'application/json' },
        ...opcoes
    });

    if (!resp.ok) {
        let detalhe = `Erro ${resp.status}`;
        try {
            const corpo = await resp.json();
            // Erros de validação vêm em "errors"; outros em "title"
            if (corpo.errors) {
                detalhe = Object.values(corpo.errors).flat().join(' ');
            } else if (corpo.title) {
                detalhe = corpo.title;
            }
        } catch { /* corpo não era JSON */ }
        throw new Error(detalhe);
    }

    // 204 (NoContent) não tem corpo
    return resp.status === 204 ? null : resp.json();
}
