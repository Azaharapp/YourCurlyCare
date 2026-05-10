import { ChangeDetectorRef, Component, inject, output } from '@angular/core';
import Quagga from '@ericblade/quagga2';
import Tesseract from 'tesseract.js';
import { ProductosService } from '../../services/productos-service';
import { ProductoEscanerI } from '../../models/productoEscaneri';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth-service';
import { Router } from '@angular/router';
import { Productos } from "../admin/productos/productos";

@Component({
  selector: 'app-escaner',
  imports: [CommonModule,
    FormsModule, Productos],
  templateUrl: './escaner.html',
  styleUrl: './escaner.css',
})
export class Escaner {
  private productosService: ProductosService = inject(ProductosService);
  public authService: AuthService = inject(AuthService);
  private router = inject(Router);
  private cd = inject(ChangeDetectorRef);
  
  public mostrarFormulario: boolean = false;
  public cargando: boolean = false;

  public codigoEscaneado: string = "";
  public ingredientesExtraidos: string = "";
  public nombreNuevo: string = "";
  public marcaNueva: string = "";
  public codigoTemporal: string = "";

  public productoEncontrado: ProductoEscanerI | null = null;
  public mensajeAviso: string | null = null;

  public alcoholesEncontrados: string[] = [];
  public siliconasEncontradas: string[] = [];
  public sulfatosEncontrados: string[] = [];

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
          this.cargarDatosProducto(respuesta);
          this.pararEscaner();
        },
        error: () => {
          //console.log("Aún no ha sido registrado en la base de datos...");
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
    if (!this.nombreNuevo || !this.marcaNueva || !this.ingredientesExtraidos || this.cargando) return alert("Faltan campos por rellenar.");

    const analisis = this.analizarIngredientes(this.ingredientesExtraidos);

    this.alcoholesEncontrados = analisis.nombresAlcoholes;
    this.siliconasEncontradas = analisis.nombresSiliconas;
    this.sulfatosEncontrados = analisis.nombresSulfatos;

    const idGuardado = localStorage.getItem('usuarioId') || sessionStorage.getItem('usuarioId');
    const idUsuario = idGuardado ? parseInt(idGuardado) : 0;

    if (idUsuario === 0) return alert("No se ha podido identificar al usuario")

    const paqueteEscaneo = {
      codigoBarras: this.codigoEscaneado,
      nombre: this.nombreNuevo,
      marca: this.marcaNueva,
      ingredientes: this.ingredientesExtraidos,
      alcohol: analisis.tieneAlcohol,
      silicona: analisis.tieneSilicona,
      sulfato: analisis.tieneSulfatos,
      esApto: !analisis.tieneAlcohol && !analisis.tieneSilicona && !analisis.tieneSulfatos || analisis.tieneSulfatos,
      usuarioId: idUsuario
    };
    
    this.productosService.registrarProducto(paqueteEscaneo).subscribe({
      next: (res: ProductoEscanerI) => {
        this.cargarDatosProducto(res);
        this.mostrarFormulario = false;
        this.productoEncontrado = null;
        
        alert("Producto analizado y guardado con éxito.");
        
        this.cd.detectChanges(); 
      },
      error: (err) => console.error("Error al guardar:", err)
    });

    this.nombreNuevo = '';
    this.marcaNueva = '';
    this.ingredientesExtraidos = '';
  }

  //expresiones regulares para evitar confusiones con espacios (\\b) al principio y final de la palabra y comas (,) al final 
  analizarIngredientes(ingredientes: string) {
    const ALCOHOLES: string[] = ["ISOPROPYL ALCOHOL", "ALCOHOL DENAT", "DENATURED ALCOHOL", "SD ALCOHOL 40", "WITCH HAZEL", "ISOPROPANOL", "ETHANOL", "SD ALCOHOL", "PROPANOL", "PROPYL ALCOHOL"];
    const SILICONAS: string[] = ["AMODIMETHICONE", "BEHENOXY DIMETHICONE", "BIS-AMINOPROPYL DIMETHICONE", "BISAMINO PEG/PPG-41/3 AMINOETHY", "BIS-CETEARYL AMODIMETHICONE", "BIS-HYDRXY/METHOXY AMODIMETHICONE", "BIS-PHENYLPROPYL DIMETHICONE", "CYCLOHEXENE", "CETEARYL METHICONE", "CETYL DIMETHICONE", "CYCLOPENTASILOXANE", "DIMETHICONE", "DIMETHICONOL", "ISOHEXADECANE", "HEXAMETHYL DISILOXANE", "HEXAMETHYLDISILOXANE", "PHENYL TRIMETHICONE", "PIPERIDINYL DIMETHICONE", "PG-PROPYL DIMETHICONE", "PROPOXYTETRAMETHYL PIPERIDINYL DIMETHICONE", "STEAROXY DIMETHICONE", "STEARYL DIMETHICONE", "TRIMETHYLSILYLAMODIMETHICONE", "TRISILOXANE"];
    const SULFATOS: string[] = ["DOICTYL SODIUM SULFOSUCCINATE", "SODIUM LAURETH SULFATE", "SODIUM TRIDECETH SULFATE", "SODIUM MYRETH SULFATE", "ETHYL PEG-15 COCAMINE SULFATE", "SODIUM ALKYLBENZENE SULFONATE", "SODIUM C14-C16 OLEFIN SULFONATE", "SODIUM DODECYL SULFATE", "SODIUM LAURYL SULFATE", "AMMONIUM LAURYL SULFATE", "AMMONIUM LAURETH SULFATE"];

    const patronAlcohol = new RegExp(`\\b(${ALCOHOLES.join('|')})\\b\\.?,?`, 'g');
    const patronSilicona = new RegExp(`\\b(${SILICONAS.join('|')})\\b\\.?,?`, 'g');
    const patronSulfato = new RegExp(`\\b(${SULFATOS.join('|')})\\b\\.?,?`, 'g');

    const textoMayusculas = ingredientes.toUpperCase();

    const matchesAlcoholes = (textoMayusculas.match(patronAlcohol) || []).map(m => m.replace(',', '').trim());
    const matchesSiliconas = (textoMayusculas.match(patronSilicona) || []).map(m => m.replace(',', '').trim());
    const matchesSulfatos = (textoMayusculas.match(patronSulfato) || []).map(m => m.replace(',', '').trim());

    return {
      tieneAlcohol: matchesAlcoholes.length > 0,
      tieneSilicona: matchesSiliconas.length > 0,
      tieneSulfatos: matchesSulfatos.length > 0,
      nombresAlcoholes: matchesAlcoholes,
      nombresSiliconas: matchesSiliconas,
      nombresSulfatos: matchesSulfatos
    };
  }

  public cargarDatosProducto(producto: ProductoEscanerI): void {

    this.productoEncontrado = producto;

    if (producto.ingredientes && producto.ingredientes.trim() !== '') {
      const analisis = this.analizarIngredientes(producto.ingredientes);

      this.alcoholesEncontrados = analisis.nombresAlcoholes;
      this.siliconasEncontradas = analisis.nombresSiliconas;
      this.sulfatosEncontrados = analisis.nombresSulfatos;
    } else {
      this.alcoholesEncontrados = [];
      this.siliconasEncontradas = [];
      this.sulfatosEncontrados = [];
    }
  }
}