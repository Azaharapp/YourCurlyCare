export interface ProductoEscanerI {
    id?: number;
    codigoBarras: string;
    nombre: string;
    marca: string;
    ingredientes: string;
    alcohol: boolean;
    silicona: boolean;
    sulfato: boolean;
    esApto: boolean;
    idUsuario: number;
    fechaRegistro: Date;
}
