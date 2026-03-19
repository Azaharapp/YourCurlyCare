export interface UsuarioRegistro {
    nombre: string,
    apellido?: string, 
    username: string,
    email: string,
    password:string
}

export interface UsuarioLogin {
    email: string,
    password:string
}

