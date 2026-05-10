import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TestI } from '../models/testi';

@Injectable({
  providedIn: 'root',
})
export class TestService {
  private http: HttpClient = inject(HttpClient);
  private apiUrl: string = "http://localhost:8000/api/Cuestionarios"              
 // private apiUrl: string = 'http://127.0.0.1:5216/api/Cuestionarios';

  getPreguntas(idCuestionario: number = 1): Observable<TestI[]> {
    return this.http.get<TestI[]>(`${this.apiUrl}/${idCuestionario}/preguntas`);
  }

  enviarTest(datos: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/guardar-lote`, datos);
  }
}
