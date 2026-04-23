import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TestService } from '../../services/test-service';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-test',
  imports: [FormsModule, RouterModule],
  templateUrl: './test.html',
  styleUrl: './test.css',
})
export class Test {
  private testService = inject(TestService);

  public preguntas = toSignal(this.testService.getPreguntas(1), { initialValue: [] });

  public indicePregunta = signal(0);
  public respuestasUsuario: any[] = [];
  public testFinalizado = signal(false);
  //public resultadoFinal = signal('');
  public seleccionada: string | null = null;

  responder(opcion: string) {
    this.respuestasUsuario.push({
      id_pregunta: this.preguntas()[this.indicePregunta()].id,
      opcion: opcion.toLowerCase()
  });

    if (this.indicePregunta() < this.preguntas().length - 1) this.indicePregunta.update(i => i + 1);
    else this.enviarResultados();  
  }

  enviarResultados() {
    const idUsuario = parseInt(localStorage.getItem('usuarioId') || '0');
    
    const paquete = {
      id_usuario: idUsuario,
      id_cuestionario: 1, 
      respuestas: this.respuestasUsuario
    };

    console.log("Enviando este paquete:", paquete);

    this.testService.enviarTest(paquete).subscribe({
      next: (res) => {
        const info = this.recomendaciones[res.resultado];
        if (info) this.resultadoInfo.set(info);
      

        this.testFinalizado.set(true);
      },
      error: (err) => console.error("Error al enviar el test", err.error)
    });
  }

  siguientePregunta() {
    if (this.seleccionada) {
      this.responder(this.seleccionada);
      
      this.seleccionada = null;
      
      const radios = document.querySelectorAll('input[type="radio"]') as NodeListOf<HTMLInputElement>;
      radios.forEach(r => r.checked = false);
    }
  }

  public recomendaciones: any = {
    'Tipo 2': {
      titulo: 'Ondulado',
      descripcion: `Tu cabello tiende a ser más liso en la raíz y a perder la forma con facilidad debido al peso de los productos o la gravedad. Se encrespa si falta fijación, pero se ensucia rápido si usas aceites pesados.
      

        - Evita productos con mantecas densas. 
     
        - Los mejores productos para ti son los que tienen texturas ligeras como mousses /espumas, leches capilares o geles de consistencia líquida.
        
        - Se recomienda aplicar el producto con el cabello muy mojado usando la técnica de manos rezando y haz mucho "Plopping" con una toalla de microfibra para ayudar a que la onda se asiente en la raíz sin peso.`,
      
    },
    'Tipo 3': {
      titulo: 'Rizado',
      descripcion: `Tu cabello tiene un patrón de espiral definido que nace desde cerca de la raíz. El principal reto es la sequedad y la pérdida de definición por el clima o el roce.
      
        
        - Requiere un equilibrio constante entre hidratación y nutrición. Es vital hacer mascarillas profundas una vez por semana. 
        
        - Los mejores productos para tu melena son las cremas de peinado con fijación media y geles de definición que sellen la humedad. El uso de un leave-in es esencial.
        
        - Se recomienda utiliza la técnica de Defining con cepillo para separar los rizos en secciones. Define haciendo Scrunch apretar el pelo hacia arriba para fomentar el muelle y seca con difusor a temperatura media.`
    },
    'Tipo 4': {
      titulo: 'Afro',
      descripcion: `Tu cabello es el más frágil debido a su patrón apretado en "Z", lo que impide que el aceite natural del cuero cabelludo llegue a las puntas. Tiene un encogimiento alto y necesita máxima retención de agua.
      
      
        - Prioriza la nutrición extrema. El "Co-wash" (lavar con acondicionador) es ideal para no resecar. Necesitas aceites selladores para mantener la elasticidad.
        
        - Los mejores productos para tu cabello son las cremas espesas, mantecas de karité y aceites naturales. 
        
        - Utiliza el método L.O.C. (Liquid, Oil, Cream) para sellar la hidratación por capas. Define por secciones pequeñas haciendo "Finger Coils" (rizos con el dedo) o "Twists" para estirar un poco el patrón y evitar nudos.`
    },
    'Mixto 2C-3A': {
      titulo: 'Ondulado / Rizado',
      descripcion: `Tienes una melena versátil pero rebelde. Algunas zonas forman tirabuzones y otras se quedan en ondas abiertas.
      
      
        - No trates todo el cabello igual. aplica una crema ligera en las zonas onduladas y refuerza con un gel más fuerte en las zonas que pierden el rizo, generalmente la capa superior. 
        
        - Evita tocarlo mucho mientras se seca para no romper el patrón híbrido.
        
        - Los mejores productos para tu tipo de pelo son un spray hidratante de base acuosa como primer paso, seguido de un gel de fijación fuerte pero de textura ligera que aporte estructura sin añadir peso.
        
        - Aplica más producto de fijación en las capas externas. Usa un cepillo definidor para unificar el patrón y termina por usar una toalla para apretar el pelo y absorber el exceso de agua/producto, asegurando que la raíz se eleve.`
    },
    'Mixto 3C-4A': {
      titulo: 'Rizado / Afro',
      descripcion: `Tu melena tiene mucha personalidad y volumen, pero conviven rizos tipo "sacacorchos" con zonas de patrón en "Z" mucho más compactas y secas. El reto es controlar el encogimiento desigual y la falta de brillo.
      
      
        - Requiere máxima hidratación con el "Co-Wash" alternado con champús hidratantes. Es obligatorio el uso de mascarillas nutritivas semanales.
        
        - Los productos con texturas cremosas y densas, las cremas de peinado con fijación son tus mejores aliados, ya que hidratan como una crema pero fijan como un gel.
      
        - Aplica el método L.O.C. (Liquid, Oil, Cream). Primero humedece, luego sella con un aceite ligero y finalmente aplica la crema. Para unificar texturas, separar mechón por mechón con los dedos llenos de producto para estirar las zonas más encogidas y que toda la melena tenga una longitud y definición similar.`
    }
  };
  
  public resultadoInfo = signal<{titulo: string, descripcion: string} | null>(null);


}
