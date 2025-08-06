export class LocalStorageUtils {

  public obterUsuario() {
     const usuario = localStorage.getItem('miniloja.user');
        return usuario ? JSON.parse(usuario) : null;
  }

  public salvarDadosLocaisUsuario(response: any) {
    this.salvarTokenUsuario(response.accessToken);
    this.salvarUsuario(response.userToken);
  }

  public limparDadosLocaisUsuario() {
    localStorage.removeItem('miniloja.token');
    localStorage.removeItem('miniloja.user');
  }

  public obterTokenUsuario(): string | null {
  return localStorage.getItem('miniloja.token');
  }

  public salvarTokenUsuario(token: string | null | undefined) {
  if (token) {
    localStorage.setItem('miniloja.token', token);
  } else {
    localStorage.removeItem('miniloja.token');
  }
}

  public salvarUsuario(usuario: string) {
    localStorage.setItem('miniloja.user', JSON.stringify(usuario));
  }
}
