import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductoEscanerI } from '../models/productoEscaneri';

@Injectable({
  providedIn: 'root',
})
export class ProductosService {
  private http: HttpClient = inject(HttpClient);
private apiUrl: string = "http://localhost:8000/api/ProductoEscaners"              
    //private apiUrl: string = 'http://127.0.0.1:5216/api/ProductoEscaners';

  getProducto(codigo: string): Observable<ProductoEscanerI> {
    return this.http.get<ProductoEscanerI>(`${this.apiUrl}/${codigo}`);
  }

 registrarProducto(producto:any): Observable<ProductoEscanerI> {
    return this.http.post<ProductoEscanerI>(`${this.apiUrl}/escanear`, producto);
  }

  buscarProductos(termino: string): Observable<ProductoEscanerI[]> {
    return this.http.get<ProductoEscanerI[]>(`${this.apiUrl}/buscar?termino=${termino}`);
  }
}
