import { HttpErrorResponse } from '@angular/common/http';

interface ProblemDetails {
  title?: string;
  detail?: string;
  errors?: Record<string, string[]>;
}

/** Extrai mensagens legíveis de uma resposta de erro da API (ProblemDetails/ValidationProblem). */
export function extrairMensagensErro(erro: unknown): string[] {
  if (erro instanceof HttpErrorResponse) {
    if (erro.status === 0) {
      return ['Não foi possível conectar à API. Verifique se o backend está em execução.'];
    }

    const corpo = erro.error as ProblemDetails | string | null;

    if (corpo && typeof corpo === 'object') {
      if (corpo.errors) {
        const mensagens = Object.values(corpo.errors).flat();
        if (mensagens.length > 0) {
          return mensagens;
        }
      }
      if (corpo.detail) {
        return [corpo.detail];
      }
      if (corpo.title) {
        return [corpo.title];
      }
    }

    if (typeof corpo === 'string' && corpo.trim()) {
      return [corpo];
    }

    return [`Erro ${erro.status} ao comunicar com a API.`];
  }

  return ['Ocorreu um erro inesperado.'];
}
