import { Component, inject } from '@angular/core';
import Quagga from '@ericblade/quagga2';
import Tesseract from 'tesseract.js';
import { ProductosService } from '../../services/productos-service';
import { ProductoEscanerI } from '../../models/productoEscaneri';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth-service';
import { Router } from '@angular/router';

//se declaran fuera porque dentro de la clase fallan
const ALCOHOLES: string[] = ["ISOPROPYL ALCOHOL", "DENATURED ALCOHOL", "SD ALCOHOL 40", "WITCH HAZEL", "ISOPROPANOL", "ETHANOL", "SD ALCOHOL", "PROPANOL", "PROPYL ALCOHOL"];
const SILICONAS: string[] = ["AMODIMETHICONE", "BEHENOXY DIMETHICONE", "BIS-AMINOPROPYL DIMETHICONE", "BISAMINO PEG/PPG-41/3 AMINOETHY", "BIS-CETEARYL AMODIMETHICONE", "BIS-HYDRXY/METHOXY AMODIMETHICONE", "BIS-PHENYLPROPYL DIMETHICONE", "CYCLOHEXENE", "CETEARYL METHICONE", "CETYL DIMETHICONE", "CYCLOPENTASILOXANE", "DIMETHICONE", "DIMETHICONOL", "ISOHEXADECANE", "HEXAMETHYL DISILOXANE", "HEXAMETHYLDISILOXANE", "PHENYL TRIMETHICONE", "PIPERIDINYL DIMETHICONE", "PG-PROPYL DIMETHICONE", "PROPOXYTETRAMETHYL PIPERIDINYL DIMETHICONE", "STEAROXY DIMETHICONE", "STEARYL DIMETHICONE", "TRIMETHYLSILYLAMODIMETHICONE", "TRISILOXANE"];
const SULFATOS: string[] = ["DOICTYL SODIUM SULFOSUCCINATE", "SODIUM LAURETH SULFATE", "SODIUM TRIDECETH SULFATE", "SODIUM MYRETH SULFATE", "ETHYL PEG-15 COCAMINE SULFATE", "SODIUM ALKYLBENZENE SULFONATE", "SODIUM C14-C16 OLEFIN SULFONATE", "SODIUM DODECYL SULFATE", "SODIUM LAURYL SULFATE", "AMMONIUM LAURYL SULFATE", "AMMONIUM LAURETH SULFATE"];

@Component({
  selector: 'app-escaner',
  imports: [CommonModule,
    FormsModule,],
  templateUrl: './escaner.html',
  styleUrl: './escaner.css',
})
export class Escaner {
  private productosService: ProductosService = inject(ProductosService);
  private authService: AuthService = inject(AuthService);
  private router = inject(Router);

  public mostrarFormulario: boolean = false;
  public cargando: boolean = false;

  public codigoEscaneado: string = "";
  public ingredientesExtraidos: string = "";
  public nombreNuevo: string = "";
  public marcaNueva: string = "";
  public codigoTemporal: string = "";

  public productoEncontrado: ProductoEscanerI | null = null;


  comprobarAcceso(): void {
    if (this.authService.isLogguedIn()) this.escanear();
    else {
      alert("Debes iniciar sesión para escanear productos.");
      this.router.navigate(['/login']);
    }
  }

pararEscaner(): void {
    Quagga.stop();
  }
  
  escanear(): void {
    this.productoEncontrado = null;
    this.mostrarFormulario = false;
    this.codigoTemporal = "";

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
    }
  }

  private configurarDeteccion(): void {
    Quagga.onDetected((data) => {
      const codigo = data.codeResult.code || '';

      this.codigoTemporal = codigo;
      this.codigoEscaneado = codigo;

      console.log("Se ha detectado el siguiente codigo de barras:", this.codigoTemporal);

      this.productosService.getProducto(codigo).subscribe({
        next: (respuesta) => {
          this.productoEncontrado = respuesta;
          this.pararEscaner();
        },
        error: () => {
          console.log("Aún no ha sido registrado en la base de datos...");
          this.productoEncontrado = null;
          this.mostrarFormulario = true;
          this.pararEscaner();
        }
      });
    });
  }

  async procesarFotoIngredientes(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) return;

    const archivo = input.files[0];
    this.cargando = true;
    this.ingredientesExtraidos = "Analizando imagen...";

    try {
      const result = await Tesseract.recognize(
        archivo,
        'spa'
      );

      this.ingredientesExtraidos = result.data.text.replace(/\s+/g, ' ').trim();
    } catch (error) {
      console.error("Se ha producido el siguiente error:", error);
      this.ingredientesExtraidos = "No se pudo leer la imagen. Inténtalo de nuevo.";
    } finally {
      this.cargando = false;
    }
  }

  public guardarNuevoProducto(): void {
    const texto = this.ingredientesExtraidos.toUpperCase();

    const tieneAlcohol = ALCOHOLES.some(alc => texto.includes(alc));
    const tieneSilicona = SILICONAS.some(sil => texto.includes(sil));
    const tieneSulfatos = SULFATOS.some(sul => texto.includes(sul));

    const idUsuario = parseInt(localStorage.getItem('usuarioId') || '0');

    const paqueteEscaneo = {
      codigoBarras: this.codigoEscaneado,
      nombre: this.nombreNuevo,
      marca: this.marcaNueva,
      ingredientes: this.ingredientesExtraidos,
      alcohol: tieneAlcohol,
      silicona: tieneSilicona,
      sulfato: tieneSulfatos,
      esApto: !tieneAlcohol && !tieneSilicona && !tieneSulfatos || tieneSulfatos,
      usuarioId: idUsuario
    };

    this.productosService.registrarProducto(paqueteEscaneo).subscribe({
      next: (res: ProductoEscanerI) => {
        this.productoEncontrado = res;
        this.mostrarFormulario = false;
        alert("Producto analizado y guardado con éxito.");
      },
      error: (err) => console.error("Error al guardar:", err)
    });
  }
}