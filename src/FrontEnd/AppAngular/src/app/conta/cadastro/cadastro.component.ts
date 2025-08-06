import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Usuario } from '../models/usuario';
import { ContaService } from '../services/conta.service';
import { GenericValidator, ValidationMessages, DisplayMessage } from '../../utils/generic-form-validations';
import { CustomValidators } from 'ngx-custom-validators';
import { fromEvent, merge, Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cadastro',
  standalone: true,
  templateUrl: './cadastro.component.html',
  imports: [CommonModule, FormsModule, ReactiveFormsModule]
})
export class CadastroComponent implements OnInit, AfterViewInit {

  @ViewChildren(FormControlName, { read: ElementRef }) 
  formInputElements!: ElementRef[];

  errors: any[] = [];
  cadastroForm!: FormGroup;
  usuario!: Usuario;

  validationMessages: ValidationMessages;
  genericValidator: GenericValidator;
  displayMessage: DisplayMessage = {};

  constructor(
    private fb: FormBuilder,
    private contaService: ContaService,
    private router: Router 
  ) {
    this.validationMessages = {
      email: {
        required: 'Informe o e-mail',
        email: 'Email inválido'
      },
      password: {
        required: 'Informe a senha',
        rangeLength: 'A senha deve possuir entre 6 e 15 caracteres'
      }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.cadastroForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, CustomValidators.rangeLength([6, 15])]]
    });
  }

  ngAfterViewInit(): void {
    const controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    merge(...controlBlurs).subscribe(() => {
      this.displayMessage = this.genericValidator.processarMensagens(this.cadastroForm);
    });
  }

  adicionarConta(): void {
    if (this.cadastroForm.dirty && this.cadastroForm.valid) {
      this.usuario = Object.assign({}, this.usuario, this.cadastroForm.value);
      this.contaService.registrarusuario(this.usuario)
        .subscribe(
          sucesso => this.processarSucesso(sucesso),
          falha => this.processarFalha(falha)
        );
    }
  }

  processarSucesso(response: any) {
  this.cadastroForm.reset();
  this.errors = [];

  // Salva o token
  this.contaService.LocalStorage.salvarTokenUsuario(response.token);

  // Busca os dados do cliente
  this.contaService.getCliente().subscribe({
    next: (cliente) => {
    if (cliente && cliente.email) {
      this.contaService.LocalStorage.salvarUsuario(cliente);
    }
      this.router.navigate(['/home']);
    },
    error: (erro) => {
      console.error('Erro ao buscar dados do cliente', erro);
      this.router.navigate(['/home']);
    }
  });
}
  processarFalha(fail: any) {
    this.errors = [];

    if (fail.error?.errors) {
      for (let campo in fail.error.errors) {
        if (fail.error.errors.hasOwnProperty(campo)) {
          this.errors.push(...fail.error.errors[campo]);
        }
      }
    } else if (fail.error?.message) {
      this.errors.push(fail.error.message);
    } else {
      this.errors.push('Erro desconhecido ao processar requisição.');
    }
  }
}
