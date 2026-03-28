import { Component, inject } from '@angular/core';
import Quagga from '@ericblade/quagga2';
import { ProductosService } from '../../services/productos-service';
import { ProductoEscanerI } from '../../models/productoEscaneri';
import { FormsModule} from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-escaner',
  imports: [CommonModule, 
    FormsModule,],
  templateUrl: './escaner.html',
  styleUrl: './escaner.css',
})
export class Escaner {
  private productosService = inject(ProductosService);

  public codigoEscaneado: string = "";
  public productoEncontrado: ProductoEscanerI | null = null;
  public mostrarFormulario: boolean = false;


  pararEscaner(): void {
    Quagga.stop();
  }

  escanear(): void {
    const elementoCamara = document.querySelector('#camara');

    if (elementoCamara) {
      Quagga.init({
        inputStream: {
          type: "LiveStream",                     //camara en tiempo real
          target: elementoCamara,
          constraints: {
            facingMode: "environment"             //la camara trasera en el caso del movil
          },
        },
        decoder: {
          readers: ["ean_reader", "code_128_reader"]
        }
      }, (error) => {
        if (error) console.error("Error al iniciar la cámara:", error);
        else {
          console.log("Cámara iniciada correctamente");
          Quagga.start();
          this.configurarDeteccion();
        }
      });

      this.configurarDeteccion();
    } else {
      console.error("No es posible acceder a la cámara.");
    }
  }

  private configurarDeteccion(): void {
    Quagga.onDetected((data) => {
      const codigo = data.codeResult.code || '';

      console.log("Se ha detectado el siguiente codigo de barras:", codigo);

      this.productosService.getProducto(codigo).subscribe({
        next: (respuesta) => {
          this.productoEncontrado = respuesta;
          this.pararEscaner();
        },
        error: (err) => {
          console.log("Este código de barras no está registrado en la base de datos aún...");
          this.productoEncontrado = null;
          this.mostrarFormulario = true;
          this.pararEscaner();
        }
      });
    });
  }
}